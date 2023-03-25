﻿namespace MAS.DapperStorageTest.Infrastructure.FilterBuilder
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Filter group.
    /// Represents SQL filter conditions
    /// </summary>
    public class FilterGroup
    {
        /// <summary>
        /// Unique name
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc cref="FilterJoinType"/>
        public FilterJoinType LogicalJoinType { get; set; }

        /// <summary>
        /// Alias for root table
        /// </summary>
        public string TableAlias { get; set; }

        /// <summary>
        /// Filter items
        /// </summary>
        public IEnumerable<FilterItem> Items { get; set; } = Enumerable.Empty<FilterItem>();

        /// <summary>
        /// Nested filter groups
        /// </summary>
        public IEnumerable<FilterGroup> NestedGroups { get; set; } = Enumerable.Empty<FilterGroup>();

        /// <summary>
        /// Is current group contains any filters or nested groups
        /// </summary>
        public bool IsEmpty
            => (Items == null && NestedGroups == null)
            || !(Items.Any() || NestedGroups.Any()); // props are both null or empty

        /// <summary>
        /// Create instance of <see cref="FilterGroup"/>
        /// </summary>
        public FilterGroup() { }

        /// <summary>
        /// Create instance of <see cref="FilterGroup"/>
        /// </summary>
        /// <param name="name">Unique filter group name</param>
        /// <param name="joinType">Sql filter join type</param>
        /// <param name="items">Filter items</param>
        public FilterGroup(string name, FilterJoinType joinType, IEnumerable<FilterItem> items)
            : this()
        {
            Name = name;
            LogicalJoinType = joinType;
            Items = items;
        }

        /// <summary>
        /// Create instance of <see cref="FilterGroup"/>
        /// </summary>
        /// <param name="name">Unique filter group name</param>
        /// <param name="joinType">Sql filter join type</param>
        /// <param name="groups">Nested filter groups</param>
        public FilterGroup(string name, FilterJoinType joinType, IEnumerable<FilterGroup> groups)
            : this()
        {
            Name = name;
            LogicalJoinType = joinType;
            NestedGroups = groups;
        }

        /// <summary>
        /// Get all column names used in filter hierarchy
        /// </summary>
        /// <returns>Column names if filtergroup is not empty; otherwise <see cref="Enumerable.Empty{TResult}"/></returns>
        public IEnumerable<string> GetFilterColumns()
        {
            if (IsEmpty)
            {
                return Enumerable.Empty<string>();
            }

            var columnNames = Items.Select(x => x.FieldName);

            if (NestedGroups.Any())
            {
                foreach (var group in NestedGroups)
                {
                    columnNames = columnNames.Union(group.GetFilterColumns());
                }
            }

            return columnNames.Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
        }
    }
}
