namespace MAS.DappertStorageTest.Cqrs
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class SelectQueryHandler : BaseQueryHandler<SelectQuery, SelectQueryResponse>
    {
        public SelectQueryHandler(IDbConnectionFactory dbConnectionFactory, IDbAdapter dbAdapter, IFilterBuilder filterBuilder)
            : base(dbConnectionFactory, dbAdapter, filterBuilder)
        {
        }

        public override SelectQueryResponse Handle(SelectQuery query)
        {
            EnsureEntityNameIsValid(query.EntityName);
            var warnings = new List<string>();

            var notValidFields = GetNotValidFieldsForEntity(query.EntityName, query.Columns);

            if (notValidFields.Any())
            {
                warnings.Add($"Not found columns: [{string.Join(", ", notValidFields)}].");
            }

            var validColumns = query.Columns.Where(column => !notValidFields.Contains(column));

            var entities = GetByFilters(query, validColumns, warnings);

            return new SelectQueryResponse(query.EntityName, entities, query.Count, query.Offset, query.Columns, query.OrderingColumns, warnings);
        }

        #region Not public API

        private IEnumerable<IDictionary<string, object>> GetByFilters(SelectQuery query, IEnumerable<string> columns, ICollection<string> warnings)
        {
            var queryColumns = !columns.Any() ? "*" : string.Join(", ", columns.Select(columnName => $"[{columnName}]"));
            var sqlQueryParts = new List<string>() { $"SELECT {queryColumns} FROM [{query.EntityName}]" };

            var whereCondition = string.Empty;
            object arguments = new { };

            if (query.FilterGroup != null)
            {
                var (builtFilterWhereStatement, builtFilterArguments) = BuildWhereFilter(query.EntityName, query.FilterGroup);

                if (!string.IsNullOrEmpty(builtFilterWhereStatement) && builtFilterArguments != null)
                {
                    (whereCondition, arguments) = (builtFilterWhereStatement, builtFilterArguments);
                }
            }

            if (!string.IsNullOrEmpty(whereCondition))
            {
                sqlQueryParts.Add($"WHERE {whereCondition}");
            }

            var orderByAndPagingSqlPart = GetOrderAndPagingSqlPart(query, warnings);

            if (!string.IsNullOrEmpty(orderByAndPagingSqlPart))
            {
                sqlQueryParts.Add(orderByAndPagingSqlPart);
            }

            var sqlQueryPartsInRow = string.Join(" ", sqlQueryParts);
            var sqlQuery = BuildQuery(sqlQueryPartsInRow);

            IEnumerable<IDictionary<string, object>> result;

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                result = DbAdapter.Query(connection, sqlQuery, arguments);
            }

            return result;
        }

        private string GetOrderAndPagingSqlPart(SelectQuery query, ICollection<string> warnings)
        {
            var orderBySqlPart = GetOrderingSqlPart(query, warnings);

            var pageSqlPart = GetPageSqlPart(query, warnings, orderBySqlPart);

            if (!string.IsNullOrEmpty(pageSqlPart) && !string.IsNullOrEmpty(orderBySqlPart))
            {
                return $"{orderBySqlPart}, {pageSqlPart}";
            }

            return !string.IsNullOrEmpty(orderBySqlPart)
                ? orderBySqlPart
                : pageSqlPart;
        }

        private string GetPageSqlPart(SelectQuery query, ICollection<string> warnings, string orderByPart)
        {
            if (query.Count > 0 && query.Count > DbConnectionFactory.QueryOptions.MaxRowCount)
            {
                warnings.Add($"Row count cannot be greater than {DbConnectionFactory.QueryOptions.MaxRowCount}.");
                query.Count = DbConnectionFactory.QueryOptions.MaxRowCount;
            }

            if (query.Offset < 0)
            {
                warnings.Add("Row offset cannot be less than 0.");
                query.Offset = 0;
            }

            if (query.Count > 0)
            {
                return string.IsNullOrEmpty(orderByPart)
                    ? $"ORDER BY [Id] ASC OFFSET {query.Offset} ROWS FETCH NEXT {query.Count} ROWS ONLY"
                    : $"[Id] ASC OFFSET {query.Offset} ROWS FETCH NEXT {query.Count} ROWS ONLY";
            }

            return string.Empty;
        }

        private string GetOrderingSqlPart(SelectQuery query, ICollection<string> warnings)
        {
            if (query.OrderingColumns.Any())
            {
                var notValidOrderingColumns = GetNotValidFieldsForEntity(query.EntityName, query.OrderingColumns.Select(x => x.Column));

                if (notValidOrderingColumns.Any())
                {
                    warnings.Add($"Not found columns for ordering: [{string.Join(", ", notValidOrderingColumns)}].");
                }
                var validOrderColumns = query.OrderingColumns.Where(opt => !notValidOrderingColumns.Contains(opt.Column));

                if (validOrderColumns.Any())
                {
                    var sql = validOrderColumns.Select(opt => $"[{opt.Column}] {opt.OrderDirection.GetSqlOperator()}");

                    return $"ORDER BY {string.Join(", ", sql)}";
                }
            }

            return string.Empty;
        }

        #endregion
    }
}
