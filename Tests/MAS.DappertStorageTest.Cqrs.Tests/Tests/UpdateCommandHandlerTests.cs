namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Models;

    using Xunit;

    public class UpdateCommandHandlerTests : CommonCqrsTests
    {
        [Fact]
        public void ShouldThrowDatabaseOperationExceptionWhenEntityNameIsNotValid()
        {
            var entityName = "TEST";
            var command = new UpdateCommand(entityName, new Dictionary<string, string>(), EmptyFilterGroup);
            var handler = new UpdateCommandHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            ShouldThrowDatabaseOperationExceptionWhenEntityNameIsNotValidInternal(entityName, command, handler);
        }

        [Fact]
        public void ShouldThrowDatabaseOperationExceptionWhenEntityFieldsAreNotValid()
        {
            var entityName = nameof(Passenger);
            var propertyValues = new Dictionary<string, string>() { { "Test", "" }, { "TestField", "" } };
            var command = new UpdateCommand(entityName, propertyValues, EmptyFilterGroup);
            var handler = new UpdateCommandHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            ShouldThrowDatabaseOperationExceptionWhenEntityFieldsAreNotValidInternal(entityName, propertyValues.Keys, command, handler);
        }

        [Fact]
        public void ShouldGenerateSqlQueryWithoutWarnings()
        {
            FilterBuilderSqlResult = "(TEST)";
            FilterBuilderArgumentsResult = new Dictionary<string, object>() { { "TestedFilter", "TestedFilterValue" } };

            var entityName = nameof(Passenger);
            var propertyValues = new Dictionary<string, string>() { { "FirstName", "NewValueFirstName" }, { "LastName", "NewValueLastName" } };
            var expectedArguments = new Dictionary<string, object> { { "UpdateEntityFirstName", "NewValueFirstName" }, { "UpdateEntityLastName", "NewValueLastName" }, { "TestedFilter", "TestedFilterValue" } };
            var expectedSqlQuery = "UPDATE [Passenger] SET [FirstName] = @UpdateEntityFirstName, [LastName] = @UpdateEntityLastName, [ModifiedOn] = @UpdateEntityModifiedOn WHERE (TEST)";
            var expectedWarningsCount = 0;

            var command = new UpdateCommand(entityName, propertyValues, EmptyFilterGroup);
            var handler = new UpdateCommandHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            handler.Handle(command);
            var lastCommand = GetLastCommand();

            AssertSqlQuery(expectedSqlQuery, lastCommand.Key);
            AssertArguments(expectedArguments, lastCommand.Value);
            Assert.Equal(expectedWarningsCount, command.Warnings.Count);
        }

        [Fact]
        public void ShouldGenerateSqlQueryAndAddWarningsWhenEntityFieldsContainsDefaultColumns()
        {
            FilterBuilderSqlResult = "(TEST)";
            FilterBuilderArgumentsResult = new Dictionary<string, object>() { { "TestedFilter", "TestedFilterValue" } };
            var entityName = nameof(Passenger);
            var expectedWarning = "Cannot set value for default columns: [CreatedOn, Id]";
            var expectedWarningsCount = 1;
            var propertyValues = new Dictionary<string, string>() { { "FirstName", "NewValueFirstName" }, { "LastName", "NewValueLastName" }, { "CreatedOn", "Some other value" }, { "Id", "New id" } };
            var command = new UpdateCommand(entityName, propertyValues, EmptyFilterGroup);
            var handler = new UpdateCommandHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            handler.Handle(command);
            var lastCommand = GetLastCommand();

            Assert.NotEmpty(command.Warnings);
            Assert.Equal(expectedWarningsCount, command.Warnings.Count);
            Assert.Equal(expectedWarning, command.Warnings.First());
        }
    }
}
