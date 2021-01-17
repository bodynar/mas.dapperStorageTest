namespace MAS.DappertStorageTest.Cqrs.Infrastructure
{
    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    public abstract class BaseQueryHandler<TQuery, TResult> : BaseCqrsHandler, IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        public BaseQueryHandler(IDbConnectionFactory dbConnectionFactory, IDbAdapter dbAdapter, IFilterBuilder filterBuilder)
            : base(dbConnectionFactory, dbAdapter, filterBuilder)
        {
        }

        public BaseQueryHandler(IDbConnectionFactory dbConnectionFactory, IDbAdapter dbAdapter)
            : base(dbConnectionFactory, dbAdapter)
        {
        }

        public abstract TResult Handle(TQuery query);
    }
}
