namespace MAS.DappertStorageTest.Cqrs
{
    using System.Linq;

    using Dapper;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Extensions;
    using MAS.DapperStorageTest.Infrastructure.Models;
    using MAS.DapperStorageTest.Models;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class SelectQueryHandler : BaseQueryHandler<SelectQuery, WrappedEntity>
    {
        public SelectQueryHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        public override WrappedEntity Handle(SelectQuery query)
        {
            EnsureEntityNameIsValid(query.EntityName);
            EnsureFieldsAreValidForEntity(query.EntityName, query.Fields);

            // TODO: fix type cast
            // do not use types, just map dapper result
            Entity entity =
                query.EntityId.HasValue
                    ? GetById(query)
                    : GetByFilters(query);

            return entity.Wrap();
        }

        #region Not public API

        private Entity GetById(SelectQuery query)
        {
            var queryColumns = !query.Fields.Any() ? "*" : string.Join(", ", query.Fields.Select(columnName => $"[{columnName}]"));
            var sqlQuery = BuildQuery($"SELECT {queryColumns} FROM [{query.EntityName}] WHERE [Id] = @id");
            Entity result;

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                result = connection.QueryFirstOrDefault(sqlQuery, new { Id = query.EntityId });
            }

            return result;
        }

        private Entity GetByFilters(SelectQuery query)
        {
            var (whereCondition, arguments) = BuildWhereFilter(query.EntityName, query.NewFilters);
            var queryColumns = !query.Fields.Any() ? "*" : string.Join(", ", query.Fields.Select(columnName => $"[{columnName}]"));

            var sqlQuery = BuildQuery($"SELECT {queryColumns} FROM [{query.EntityName}] WHERE {whereCondition}");
            Entity result;

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                result = connection.QueryFirstOrDefault<Entity>(sqlQuery, arguments);
            }

            return result;
        }

        #endregion
    }
}
