﻿namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System;

    using MAS.DapperStorageTest.Infrastructure;

    using Xunit;

    public sealed class DeleteTests : BaseCqrsTests
    {
        //[Fact]
        //public void ShouldThrowDatabaseOperationExceptionWhenEntityNameIsNotValid()
        //{
        //    var entityName = "TESTENTITYNAME";
        //    var expectedExceptionMessage = "Entity name \"TESTENTITYNAME\" is not valid or isn't presented in database.";
        //    var command = new DeleteCommand(entityName, Guid.Empty);
        //    var handler = new DeleteCommandHandler(DbConnectionFactory, FilterBuilder);

        //    var exception =
        //        Record.Exception(
        //            () => handler.Handle(command)
        //        );

        //    Assert.NotNull(exception);
        //    Assert.IsType<DatabaseOperationException>(exception);
        //    Assert.Equal(expectedExceptionMessage, exception.Message);
        //}

        //[Fact]
        //public void Should()
        //{
        //    // TODO: replace this config with InMemoryRepo (sql lite or ormlite)

        //    var entityName = "Driver";
        //    var command = new DeleteCommand(entityName, Guid.Empty);
        //    var handler = new DeleteCommandHandler(DbConnectionFactory, FilterBuilder);

        //    //handler.Handle(command);
        //}
    }
}
