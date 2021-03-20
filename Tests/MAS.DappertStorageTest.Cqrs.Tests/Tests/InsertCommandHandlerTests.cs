namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Models;

    using Xunit;

    public sealed class InsertCommandHandlerTests : BaseCqrsTests
    {
        [Fact]
        public void ShouldThrowDatabaseOperationExceptionWhenEntityNameIsNotValid()
        {
            var entityName = "TEST";
            var expectedExceptionMessage = "Entity name \"TEST\" is not valid or isn't presented in database.";
            var command = new InsertCommand(entityName, new Dictionary<string, object>());
            var handler = new InsertCommandHandler(DbConnectionFactory, DbAdapter);

            Exception exception =
                Record.Exception(
                    () => handler.Handle(command)
                );

            Assert.NotNull(exception);
            Assert.IsType<DatabaseOperationException>(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void ShouldThrowDatabaseOperationExceptionWhenEntityFieldsAreNotValid()
        {
            var entityName = nameof(Passenger);
            var expectedExceptionMessage = "Entity name \"Passenger\" does not contains these fields: [Test, TestField].";
            var command = new InsertCommand(entityName, new Dictionary<string, object>() { { "Test", "" }, { "TestField", "" } });
            var handler = new InsertCommandHandler(DbConnectionFactory, DbAdapter);

            Exception exception =
                Record.Exception(
                    () => handler.Handle(command)
                );

            Assert.NotNull(exception);
            Assert.IsType<DatabaseOperationException>(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void ShouldGenerateSqlQuery()
        {
            var entityName = nameof(Passenger);
            var expectedArguments = new Dictionary<string, object> { { "NewEntityFirstName", "TestedFirstName" }, { "NewEntityLastName", "TestedLastName" } };
            var propertyValues = new Dictionary<string, object>() { { "FirstName", "TestedFirstName" }, { "LastName", "TestedLastName" } };
            var expectedSqlQuery = "INSERT INTO [Passenger] ([Id], [CreatedOn], [FirstName], [LastName]) VALUES (@NewEntityId, @NewEntityCreatedAt, @NewEntityFirstName, @NewEntityLastName)";
            var command = new InsertCommand(entityName, propertyValues);
            var handler = new InsertCommandHandler(DbConnectionFactory, DbAdapter);

            handler.Handle(command);
            var lastCommand = GetLastCommand();

            AssertSqlQuery(expectedSqlQuery, lastCommand.Key);
            AssertArguments(expectedArguments, lastCommand.Value);
        }

        [Fact]
        public void ShouldGenerateWarningsWhenEntityFieldsContainsDefaultColumns()
        {
            var expectedWarningsCount = 1;
            var expectedWarningMessage = "Cannot set value for default columns: [Id, CreatedOn]";
            var entityName = nameof(Passenger);
            var command = new InsertCommand(entityName, new Dictionary<string, object>() { { "FirstName", "TestedFirstName" }, { "LastName", "TestedLastName" }, { "Id", "TestedId" }, { "CreatedOn", "Today" } });
            var handler = new InsertCommandHandler(DbConnectionFactory, DbAdapter);

            handler.Handle(command);

            Assert.Equal(expectedWarningsCount, command.Warnings.Count);
            Assert.Equal(expectedWarningMessage, command.Warnings.First());
        }
    }
}
