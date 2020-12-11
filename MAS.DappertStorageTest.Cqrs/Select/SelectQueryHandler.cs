namespace MAS.DappertStorageTest.Cqrs
{
    using System.Collections.Generic;
    using System.Dynamic;
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
            // TODO: fix type cast
            // do not use types, just map dapper result
            Entity entity =
                query.EntityId.HasValue
                    ? GetById(query)
                    : GetByFilters(query);

            return entity.Wrap();
        }

        private Entity GetById(SelectQuery query)
        {
            var queryColumns = !query.Fields.Any() ? "*" : string.Join(", ", query.Fields.Select(columnName => $"[{columnName}]"));
            var sqlQuery = BuildQuery($"SELECT {queryColumns} FROM [{query.EntityName}] WHERE [Id] = @id");

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                var res = connection.QueryFirstOrDefault(sqlQuery, new { Id = query.EntityId });
                return res as Entity;
            }
        }

        private Entity GetByFilters(SelectQuery query)
        {
            EnsureFieldsAreValidForEntity(query.EntityName, query.Filters.Keys);

            var whereSqlParts = new List<string>();
            var arguments = new ExpandoObject();

            foreach (var pair in query.Filters)
            {
                whereSqlParts.Add($"[{pair.Key}] = @Entity{pair.Key}");
                arguments.TryAdd($"Entity{pair.Key}", pair.Value);
            }
            // TODO: rework filters (add groups or just eject into object)
            var queryColumns = !query.Fields.Any() ? "*" : string.Join(", ", query.Fields.Select(columnName => $"[{columnName}]"));

            var sqlQuery = BuildQuery($"SELECT {queryColumns} FROM [{query.EntityName}] WHERE {string.Join(", ", whereSqlParts)}");
            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                return connection.QueryFirstOrDefault<Entity>(sqlQuery, arguments);
            }
        }
    }
}
