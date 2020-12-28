namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    public class SelectQuery : IQuery<SelectQueryResponse>
    {
        public string EntityName { get; }

        public int Count { get; }

        public int Offset { get; }

        public IEnumerable<string> Columns { get; }

        public QueryFilterGroup FilterGroup { get; }

        public IEnumerable<string> OrderingColumns { get; }

        public SelectQuery(string entityName, IEnumerable<string> columns, QueryFilterGroup filterGroup, IEnumerable<string> orderingColumns, int count, int offset)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Columns = columns ?? Enumerable.Empty<string>();
            FilterGroup = filterGroup;
            OrderingColumns = orderingColumns ?? Enumerable.Empty<string>();
            Count = count;
            Offset = offset;
        }
    }
}
