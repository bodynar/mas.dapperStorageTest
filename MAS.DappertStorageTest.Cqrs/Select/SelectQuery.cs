namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure.Cqrs;
    using MAS.DapperStorageTest.Infrastructure.Models;

    public class SelectQuery : IQuery<WrappedEntity>
    {
        public string EntityName { get; set; }

        public IEnumerable<string> Fields { get; }

        public Guid? EntityId { get; }

        public IDictionary<string, string> Filters { get; }

        public IEnumerable<SelectQueryFilter> NewFilters { get; }

        public SelectQuery(string entityName, IEnumerable<string> fields, Guid? entityId)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Fields = fields ?? Enumerable.Empty<string>();
            EntityId = entityId;
        }

        public SelectQuery(string entityName, IEnumerable<string> fields, IEnumerable<SelectQueryFilter> filters)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Fields = fields ?? Enumerable.Empty<string>();
            NewFilters = filters;
        }
    }

    public class SelectQueryFilter
    {
        public string FieldName { get; }

        public string FilterValue { get; }

        public ComparisonType ComparisonType { get; }

        public SelectQueryFilter(string fieldName, string filterValue, ComparisonType comparisonType)
        {
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            FilterValue = filterValue ?? throw new ArgumentNullException(nameof(filterValue));
            ComparisonType = comparisonType;
        }
    }
}
