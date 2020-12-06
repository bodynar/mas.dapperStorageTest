namespace MAS.DappertStorageTest.Cqrs
{
    using System.Collections.Generic;
    using System.Dynamic;

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

            var tableName = GetTableName(query.EntityName);

            Entity entity =
                query.EntityId.HasValue
                    ? GetById(query, tableName)
                    : GetByFilters(query, tableName);

            return entity.Wrap();
        }

        private Entity GetById(SelectQuery query, string tableName)
        {
            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                return connection.QueryFirstOrDefault<Entity>($"SELECT * FROM [${tableName}] WHERE [Id] = @id", new { Id = query.EntityId });
            }
        }

        private Entity GetByFilters(SelectQuery query, string tableName)
        {
            EnsureFieldsAreValidForEntity(query.EntityName, query.Filters.Keys);

            var whereSqlParts = new List<string>();
            var arguments = new ExpandoObject();

            foreach (var pair in query.Filters)
            {
                whereSqlParts.Add($"[{pair.Key}] = @Entity{pair.Key}");
                arguments.TryAdd(pair.Key, pair.Value);
            }

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                return connection.QueryFirstOrDefault<Entity>($"SELECT * FROM [{tableName}] WHERE {string.Join(", ", whereSqlParts)}", arguments);
            }
        }
    }
}
