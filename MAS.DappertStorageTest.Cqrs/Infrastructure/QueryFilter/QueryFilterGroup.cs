namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class QueryFilterGroup
    {
        public string EntityName { get; }

        public string Name { get; }

        public FilterJoinType FilterJoinType { get; }

        public IEnumerable<QueryFilter> Filters { get; }

        public IEnumerable<QueryFilterGroup> InnerGroups { get; }

        public bool IsEmpty
            => !Filters.Any() && !InnerGroups.Any();

        public QueryFilterGroup(string entityName, string name, FilterJoinType filterJoinType, IEnumerable<QueryFilter> filters)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Filters = filters ?? throw new ArgumentNullException(nameof(filters));
            FilterJoinType = filterJoinType;
        }

        public QueryFilterGroup(string entityName, string name, FilterJoinType filterJoinType, IEnumerable<QueryFilterGroup> innerGroups)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            InnerGroups = innerGroups ?? throw new ArgumentNullException(nameof(innerGroups));
            FilterJoinType = filterJoinType;
        }
    }
}
