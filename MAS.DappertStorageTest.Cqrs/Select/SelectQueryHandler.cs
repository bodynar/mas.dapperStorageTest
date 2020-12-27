namespace MAS.DappertStorageTest.Cqrs
{
    using System.Collections.Generic;
    using System.Linq;

    using Dapper;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class SelectQueryHandler : BaseQueryHandler<SelectQuery, SelectQueryResponse>
    {
        public SelectQueryHandler(IDbConnectionFactory dbConnectionFactory, IFilterBuilder filterBuilder)
            : base(dbConnectionFactory, filterBuilder)
        {
        }

        public override SelectQueryResponse Handle(SelectQuery query)
        {
            EnsureEntityNameIsValid(query.EntityName);
            EnsureFieldsAreValidForEntity(query.EntityName, query.Columns);

            var entities = GetByFilters(query);

            if (query.OrderingColumns.Any())
            {
                var firstOrdering = entities.OrderBy(entity => entity[query.OrderingColumns.First()]);

                foreach (var orderingColumn in query.OrderingColumns.Skip(1))
                {
                    firstOrdering = firstOrdering.ThenBy(entity => entity[orderingColumn]);
                }

                entities = firstOrdering.ToList();
            }

            return new SelectQueryResponse(query.EntityName, entities, query.Count, query.Offset, query.Columns, query.OrderingColumns);
        }

        #region Not public API

        private IEnumerable<IDictionary<string, object>> GetByFilters(SelectQuery query)
        {
            var (whereCondition, arguments) = BuildWhereFilter(query.EntityName, query.FilterGroup);
            var queryColumns = !query.Columns.Any() ? "*" : string.Join(", ", query.Columns.Select(columnName => $"[{columnName}]"));

            var sqlQuery = BuildQuery($"SELECT {queryColumns} FROM [{query.EntityName}] WHERE {whereCondition}");
            IEnumerable<dynamic> result;

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                result = connection.Query(sqlQuery, arguments); // TODO: Fix NotSupportedException: The member IdFilterEntityId of type System.Text.Json.JsonElement cannot be used as a parameter value
            }

            return result.Select(entity => entity as IDictionary<string, object>);
        }

        #endregion
    }
}
