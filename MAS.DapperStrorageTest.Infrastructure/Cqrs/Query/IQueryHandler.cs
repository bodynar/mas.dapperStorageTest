﻿namespace MAS.DapperStorageTest.Infrastructure.Cqrs
{
    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}
