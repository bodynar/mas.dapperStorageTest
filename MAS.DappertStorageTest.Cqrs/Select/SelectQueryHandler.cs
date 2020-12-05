namespace MAS.DappertStorageTest.Cqrs
{
    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Models;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class SelectQueryHandler : BaseQueryHandler<SelectQuery, WrappedEntity>
    {
        public SelectQueryHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        public override WrappedEntity Handle(SelectQuery query)
        {
            var tableName = GetTableName(query.EntityName);

            throw new System.NotImplementedException();
        }
    }
}
