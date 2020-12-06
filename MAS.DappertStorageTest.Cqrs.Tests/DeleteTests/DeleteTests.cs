namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System;

    using MAS.DapperStorageTest.Infrastructure;

    using Xunit;

    public sealed class DeleteTests : BaseCqrsTests
    {
        [Fact]
        public void ShouldThrowDatabaseOperationExceptionWhenEntityNameIsNotValid()
        {
            var entityName = "TESTENTITYNAME";
            var expectedExceptionMessage = "Entity name \"TESTENTITYNAME\" is not valid or isn't presented in database.";
            var command = new DeleteCommand(Guid.Empty, entityName);
            var handler = new DeleteCommandHandler(new MockDbConnectionFactory());

            var exception =
                Record.Exception(
                    () => handler.Handle(command)
                );

            Assert.NotNull(exception);
            Assert.IsType<DatabaseOperationException>(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void Should()
        {
            // TODO: replace this config with InMemoryRepo (sql lite or ormlite)

            var dbConnectionFactory = new MockDbConnectionFactory();
            var handler = new DeleteCommandHandler(dbConnectionFactory);
            var entityName = "Driver";
            var command = new DeleteCommand(Guid.Empty, entityName);

            handler.Handle(command);
        }
    }
}
