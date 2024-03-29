﻿namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System.Collections.Generic;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    using Xunit;

    /// <summary>
    /// Common tests for base cqrs members logic
    /// </summary>
    public class CommonCqrsTests : BaseCqrsTests
    {
        /// <summary>
        /// When entity name is being checked and it's not valid
        /// </summary>
        /// <typeparam name="TCommand">Tested command type</typeparam>
        /// <param name="entityName">Entity name</param>
        /// <param name="command">Tested command instance</param>
        /// <param name="handler">Command handler instance</param>
        protected void ShouldThrowDatabaseOperationExceptionWhenEntityNameIsNotValidInternal<TCommand>(string entityName,
            TCommand command, ICommandHandler<TCommand> handler)
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
        /// When entity name is being checked and it's not valid
        /// </summary>
        /// <typeparam name="TQuery">Tested query type</typeparam>
        /// <param name="entityName">Entity name</param>
        /// <param name="query">Tested query instance</param>
        /// <param name="handler">Query handler instance</param>
        protected void ShouldThrowDatabaseOperationExceptionWhenEntityNameIsNotValidInternal<TQuery, TResult>(string entityName,
            TQuery query, IQueryHandler<TQuery, TResult> handler)
            where TQuery : IQuery<TResult>
        {
            var expectedExceptionMessage = $"Entity name \"{entityName}\" is not valid or isn't presented in database.";

            var exception =
                Record.Exception(
                    () => handler.Handle(query)
                );

            Assert.NotNull(exception);
            Assert.IsType<DatabaseOperationException>(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        /// <summary>
        /// When entity columns is being checked and some of it isn't valid
        /// </summary>
        /// <typeparam name="TCommand">Tested command type</typeparam>
        /// <param name="entityName">Entity name</param>
        /// <param name="notValidColumns">Not valid column names</param>
        /// <param name="command">Tested command instance</param>
        /// <param name="handler">Command handler instance</param>
        protected void ShouldThrowDatabaseOperationExceptionWhenEntityFieldsAreNotValidInternal<TCommand>(string entityName, IEnumerable<string> notValidColumns,
            TCommand command, ICommandHandler<TCommand> handler)
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
    }
}
