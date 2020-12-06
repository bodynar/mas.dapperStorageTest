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

            Entity entity =
                query.EntityId.HasValue
                    ? GetById(query)
                    : GetByFilters(query);

            return entity.Wrap();
        }

        private Entity GetById(SelectQuery query)
        {
            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                return connection.QueryFirstOrDefault<Entity>($"SELECT * FROM [${query.EntityName}] WHERE [Id] = @id", new { Id = query.EntityId });
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
                arguments.TryAdd(pair.Key, pair.Value);
            }

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                return connection.QueryFirstOrDefault<Entity>($"SELECT * FROM [{query.EntityName}] WHERE {string.Join(", ", whereSqlParts)}", arguments);
            }
        }
    }
}
