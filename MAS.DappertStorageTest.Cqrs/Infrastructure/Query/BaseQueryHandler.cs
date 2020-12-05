namespace MAS.DappertStorageTest.Cqrs.Infrastructure
{
    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    public abstract class BaseQueryHandler<TQuery, TResult> : BaseCqrsHandler, IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        public BaseQueryHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        public abstract TResult Handle(TQuery query);
    }
}
