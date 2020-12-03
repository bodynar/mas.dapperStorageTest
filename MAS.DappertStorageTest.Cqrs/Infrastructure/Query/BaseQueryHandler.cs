namespace MAS.DappertStorageTest.Cqrs.Infrastructure
{
    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    public abstract class BaseQueryHandler<TQuery, TResult> : BaseCqrsHandler, IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        public BaseQueryHandler(IResolver resolver)
            : base(resolver)
        {
        }

        public TResult Handle(TQuery query)
        {
            var isValidEntityName = IsValidEntityName(query.EntityName);

            if (isValidEntityName)
            {
                return Proceed(query);
            }
            else
            {
                throw new DatabaseQueryException($"{query.EntityName} isn't presented in database.");
            }
        }

        public abstract TResult Proceed(TQuery query);
    }
}
