namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure.Cqrs;
    using MAS.DapperStorageTest.Infrastructure.Models;

    //public enum ComparisonType
    //{

    //}

    public class SelectQuery : IQuery<WrappedEntity>
    {
        public string EntityName { get; set; }

        public IEnumerable<string> Fields { get; }

        public Guid? EntityId { get; }

        public IDictionary<string, string> Filters { get; }

        public SelectQuery(string entityName, IEnumerable<string> fields, Guid? entityId, IDictionary<string, string> filters)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Fields = fields ?? Enumerable.Empty<string>();
            EntityId = entityId;
            Filters = filters;
        }
    }

    //public class SelectQueryFilter
    //{
    //    public string FieldName { get; }

    //    public string FilterValue { get; }

    //    public ComparisonType ComparisonType { get; }
    //}
}
