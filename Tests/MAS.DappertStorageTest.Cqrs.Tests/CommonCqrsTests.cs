namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System.Collections.Generic;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Cqrs;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    using Xunit;

    public class CommonCqrsTests : BaseCqrsTests
    {
        /// <summary>
        /// Common test for case when entity name is checking and it's not valid
        /// </summary>
        /// <typeparam name="TCommand">Tested command type</typeparam>
        /// <param name="entityName">Entity name</param>
        /// <param name="command">Tested command instance</param>
        /// <param name="handler">Command handler instance</param>
        protected void ShouldThrowDatabaseOperationExceptionWhenEntityNameIsNotValidInternal<TCommand>(string entityName,
            TCommand command, BaseCommandHandler<TCommand> handler)
            where TCommand : ICommand
        {
            var expectedExceptionMessage = $"Entity name \"{entityName}\" is not valid or isn't presented in database.";

            var exception =
                Record.Exception(
                    () => handler.Handle(command)
                );

            Assert.NotNull(exception);
            Assert.IsType<DatabaseOperationException>(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        /// <summary>
        /// Common test for case when entity columns is checking and some of it isn't valid
        /// </summary>
        /// <typeparam name="TCommand">Tested command type</typeparam>
        /// <param name="entityName">Entity name</param>
        /// <param name="notValidColumns">Not valid column names</param>
        /// <param name="command">Tested command instance</param>
        /// <param name="handler">Command handler instance</param>
        protected void ShouldThrowDatabaseOperationExceptionWhenEntityFieldsAreNotValidInternal<TCommand>(string entityName, IEnumerable<string> notValidColumns,
            TCommand command, BaseCommandHandler<TCommand> handler)
            where TCommand : ICommand
        {
            var expectedExceptionMessage = $"Entity name \"{entityName}\" does not contains these fields: [{string.Join(", ", notValidColumns)}].";

            var exception =
                Record.Exception(
                    () => handler.Handle(command)
                );

            Assert.NotNull(exception);
            Assert.IsType<DatabaseOperationException>(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        /// <summary>
        /// Common test for case when entity columns is checking and some of it isn't valid
        /// </summary>
        /// <typeparam name="TQuery">Tested query type</typeparam>
        /// <param name="entityName">Entity name</param>
        /// <param name="notValidColumns">Not valid column names</param>
        /// <param name="command">Tested query instance</param>
        /// <param name="handler">Query handler instance</param>
        protected void ShouldThrowDatabaseOperationExceptionWhenEntityFieldsAreNotValidInternal<TQuery>(string entityName, IEnumerable<string> notValidColumns,
            TQuery command, BaseQueryHandler<TQuery, object> handler)
            where TQuery : IQuery<object>
        {
            var expectedExceptionMessage = $"Entity name \"{entityName}\" does not contains these fields: [{string.Join(", ", notValidColumns)}].";

            var exception =
                Record.Exception(
                    () => handler.Handle(command)
                );

            Assert.NotNull(exception);
            Assert.IsType<DatabaseOperationException>(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }
    }
}
