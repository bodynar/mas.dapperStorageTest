namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class MySqlFilterBuilder : IFilterBuilder
    {
        private ILogger Logger { get; }

        public MySqlFilterBuilder(ILogger logger)
        {
            Logger = logger;
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
                var accomulatedResult = string.Empty;
                var innerFilters = filterGroup.InnerGroups.Where(x => x.FilterJoinType != FilterJoinType.None).OrderBy(x => x.FilterJoinType);

                if (innerFilters.Count() != filterGroup.InnerGroups.Count())
                {
                    var emptyFilters = filterGroup.InnerGroups.Where(x => x.FilterJoinType == FilterJoinType.None).Select(x => x.Name);
                    Logger.Warn($"[{nameof(MySqlFilterBuilder)}] Empty filter groups: [{string.Join(", ", emptyFilters)}]");
                }

                var filterJointypeOperator = filterGroup.FilterJoinType.GetSqlOperator();

                foreach (var filterGroupItem in innerFilters)
                {
                    var sqlFilter = BuildWhereFilter(filterGroupItem, arguments);

                    accomulatedResult +=
                        string.IsNullOrEmpty(accomulatedResult)
                        ? $"{sqlFilter}"
                        : $"{Environment.NewLine}{filterJointypeOperator} {sqlFilter}";
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

            var filterItems = filterGroup.Filters.Where(filter => filter.ComparisonType != ComparisonType.None);

            if (filterItems.Count() != filterGroup.Filters.Count())
            {
                var emptyFilters = filterGroup.Filters.Where(x => x.ComparisonType == ComparisonType.None).Select(x => x.Name);
                Logger.Warn($"[{nameof(MySqlFilterBuilder)}] Empty filter items: [{string.Join(", ", emptyFilters)}]");
            }

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
                    Logger.Warn($"[{nameof(MySqlFilterBuilder)}] Comparison operator not valid \"{filter.ComparisonType}\".");
                    continue;
                }
            }

            var joinOperator = filterGroup.FilterJoinType.GetSqlOperator();

            return $"({string.Join($"{Environment.NewLine}{joinOperator} ", whereSqlParts)})";
        }
    }
}
