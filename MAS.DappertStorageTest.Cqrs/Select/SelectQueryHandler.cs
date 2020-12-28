namespace MAS.DappertStorageTest.Cqrs
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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

        private IEnumerable<IDictionary<string, object>> GetByFilters(SelectQuery query)
        {
            var queryColumns = !query.Columns.Any() ? "*" : string.Join(", ", query.Columns.Select(columnName => $"[{columnName}]"));
            var sqlQueryBuilder = new StringBuilder($"SELECT {queryColumns} FROM [{query.EntityName}]");

            var whereCondition = string.Empty;
            object arguments = new { };

            if (query.FilterGroup != null)
            {
                (whereCondition, arguments) = BuildWhereFilter(query.EntityName, query.FilterGroup);
            }

            sqlQueryBuilder.Append(string.IsNullOrEmpty(whereCondition) ? "" : $" WHERE {whereCondition}");
            sqlQueryBuilder.Append(query.Count != 0 ? $" ORDER BY [Id] OFFSET {query.Offset} ROWS FETCH NEXT {query.Count} ROWS ONLY" : "");

            var sqlQuery = BuildQuery(sqlQueryBuilder.ToString());

            IEnumerable<dynamic> result;

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                result = connection.Query(sqlQuery, arguments);
            }

            return result.Select(entity => entity as IDictionary<string, object>);
        }
    }
}
