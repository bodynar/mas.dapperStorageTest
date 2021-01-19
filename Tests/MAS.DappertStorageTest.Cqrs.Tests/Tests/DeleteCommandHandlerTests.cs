namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System;
    using System.Collections.Generic;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Models;

    using Xunit;

    public sealed class DeleteCommandHandlerTests : BaseCqrsTests
    {
        [Fact]
        public void ShouldThrowDatabaseOperationExceptionWhenEntityNameIsNotValid()
        {
            var entityName = "TESTENTITYNAME";
            var expectedExceptionMessage = "Entity name \"TESTENTITYNAME\" is not valid or isn't presented in database.";
            var command = new DeleteCommand(entityName, Guid.Empty);
            var handler = new DeleteCommandHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            var exception =
                Record.Exception(
                    () => handler.Handle(command)
                );

            Assert.NotNull(exception);
            Assert.IsType<DatabaseOperationException>(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void ShouldBuildSqlCommandFromIdParameter()
        {
            var testedGuid = Guid.NewGuid();
            var expectedSql = "DELETE FROM [Passenger] WHERE Id = @Id";
            var expectedArguments = new[] { new KeyValuePair<string, object>("Id", testedGuid) };
            var entityName = nameof(Passenger);
            var command = new DeleteCommand(entityName, testedGuid);
            var handler = new DeleteCommandHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            handler.Handle(command);
            var lastCommand = GetLastCommand();

            AssertSqlQuery(expectedSql, lastCommand.Key);
            Assert.NotNull(lastCommand.Value);
            AssertArguments(lastCommand.Value, expectedArguments);
        }

        [Fact]
        public void ShouldThrowArgumentExceptionWhenFilterGroupIsEmpty()
        {
            var entityName = nameof(Passenger);
            var expectedExceptionMessage = "Filter doesn't contains any expression.";
            var command = new DeleteCommand(entityName, EmptyFilterGroup);
            var handler = new DeleteCommandHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            Exception exception =
                Record.Exception(
                    () => handler.Handle(command)
                );

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void ShouldBuildSqlCommandFromFilterParameter()
        {
            FilterBuilderSqlResult = "Test";
            var expectedSql = "DELETE FROM [Passenger] WHERE Test";
            var entityName = nameof(Passenger);
            var command = new DeleteCommand(entityName, TestedFilterGroup);
            var handler = new DeleteCommandHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            handler.Handle(command);
            var lastCommand = GetLastCommand();

            AssertSqlQuery(expectedSql, lastCommand.Key);
        }
    }
}
