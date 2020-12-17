namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class QueryFilterGroup
    {
        public string EntityName { get; }

        public string FilterName { get; }

        public FilterJoinType FilterJoinType { get; }

        public IEnumerable<QueryFilter> Filters { get; }

        public IEnumerable<QueryFilterGroup> InnerGroups { get; }

        public bool IsEmpty
            => !Filters.Any() && !InnerGroups.Any();

        public QueryFilterGroup(string entityName, string filterName, FilterJoinType filterJoinType, IEnumerable<QueryFilter> filters)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            FilterName = filterName ?? throw new ArgumentNullException(nameof(filterName));
            Filters = filters ?? throw new ArgumentNullException(nameof(filters));
            FilterJoinType = filterJoinType;
        }

        public QueryFilterGroup(string entityName, string filterName, FilterJoinType filterJoinType, IEnumerable<QueryFilterGroup> innerGroups)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            FilterName = filterName ?? throw new ArgumentNullException(nameof(filterName));
            InnerGroups = innerGroups ?? throw new ArgumentNullException(nameof(innerGroups));
            FilterJoinType = filterJoinType;
        }
    }
}
