namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class MySqlFilterBuilder : IFilterBuilder
    {
        public MySqlFilterBuilder()
        {
        }

        public (string, ExpandoObject) Build(QueryFilterGroup queryFilterGroup)
        {
            var arguments = new ExpandoObject();
            var resultSql = BuildWhereFilter(queryFilterGroup, arguments);

            return (resultSql, arguments);
        }

        private string BuildWhereFilter(QueryFilterGroup filterGroup, ExpandoObject arguments)
        {
            if (filterGroup.InnerGroups.Any())
            {
                // TODO: log empty groups
                // TODO: wrap groups

                var accomulatedResult = string.Empty;
                var innerFilters = filterGroup.InnerGroups.Where(x => x.FilterJoinType != FilterJoinType.None).OrderBy(x => x.FilterJoinType);

                foreach (var filterGroupItem in innerFilters)
                {
                    var sqlFilter = BuildWhereFilter(filterGroupItem, arguments);
                    var filterJointypeOperator = filterGroupItem.FilterJoinType.GetSqlOperator();

                    accomulatedResult +=
                        string.IsNullOrEmpty(accomulatedResult) 
                        ? $"({sqlFilter})"
                        : $"{Environment.NewLine}{filterJointypeOperator} ({sqlFilter})";
                }

                return accomulatedResult;
            }
            else
            {
                return BuildWhereFilterGroupFromFields(filterGroup, arguments);
            }
        }

        private string BuildWhereFilterGroupFromFields(QueryFilterGroup filterGroup, ExpandoObject arguments)
        {
            if (filterGroup.InnerGroups.Any())
            {
                throw new ArgumentException($"{nameof(BuildWhereFilterGroupFromFields)} can be used only with deepest filter group.");
            }
            
            var sqlFilter = string.Empty;
            var whereSqlParts = new List<string>();

            // TODO: log empty filters
            var filterItems = filterGroup.Filters.Where(filter => filter.ComparisonType != ComparisonType.None);

            foreach (var filter in filterItems)
            {
                var comparisonOperator = filter.ComparisonType.GetSqlOperator();

                if (!string.IsNullOrEmpty(comparisonOperator))
                {
                    whereSqlParts.Add($"[{filter.FieldName}] {comparisonOperator} @{filter.Name}Entity{filter.FieldName}");
                    arguments.TryAdd($"{filter.Name}Entity{filter.FieldName}", filter.Value);
                }
                else
                {
                    // TODO: log
                    continue;
                }
            }

            var joinOperator = filterGroup.FilterJoinType.GetSqlOperator();
            
            return $"({string.Join($"{Environment.NewLine}{joinOperator} ", whereSqlParts)})";
        }
    }
}
