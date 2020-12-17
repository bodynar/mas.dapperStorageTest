namespace MAS.DappertStorageTest.Cqrs
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class FilterBuilder : IFilterBuilder
    {
        private const string SqlWhereFilterItemPattern = "({0})";

        public (string, ExpandoObject) Build(IEnumerable<QueryFilterGroup> queryFilterGroup)
        {
            // TODO: log none filters
            queryFilterGroup = queryFilterGroup.Where(filter => filter.FilterJoinType != FilterJoinType.None && !filter.IsEmpty);

            var arguments = new ExpandoObject();
            var whereSqlParts = new List<string>();

            foreach (var filterGroup in queryFilterGroup)
            {

                // get internal sql and wrap it into parenthesis () and add operator AND\OR
            }

            return (string.Join(", ", whereSqlParts), arguments);

            throw new System.NotImplementedException();
        }

        private string BuildWhereFilterGroup(QueryFilterGroup filterGroup, ExpandoObject arguments)
        {
            var sqlFilter = string.Empty;

            if (filterGroup.InnerGroups.Any())
            {

            }
            else
            {
                var filterItems = filterGroup.Filters.Where(filter => filter.ComparisonType != ComparisonType.None);
                var fieldNames = filterItems.Select(filter => filter.FieldName);

                var whereSqlParts = new List<string>();

                foreach (var filter in filterItems)
                {
                    var comparisonOperator = filter.ComparisonType.GetComparisonOperator();

                    if (!string.IsNullOrEmpty(comparisonOperator))
                    {
                        whereSqlParts.Add($"[{filter.FieldName}] {comparisonOperator} @Entity{filter.FieldName}");
                        arguments.TryAdd($"Entity{filter.FieldName}", filter.FilterValue);
                    }
                    else
                    {
                        // TODO: log
                        continue;
                    }
                }

                var joinOperator = ""; // TODO

                sqlFilter = string.Format(SqlWhereFilterItemPattern, string.Join($"{joinOperator} ", whereSqlParts));
            }

            return sqlFilter;
        }
    }
}
