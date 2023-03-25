namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure.Cqrs;
    using MAS.DapperStorageTest.Infrastructure.FilterBuilder;
    using MAS.DapperStorageTest.Infrastructure.Sql;

    public class SelectQuery : IQuery<SelectQueryResponse>
    {
        public string EntityName { get; }

        public int Count { get; set; }

        public int Offset { get; set; }

        public IEnumerable<string> Columns { get; }

        public FilterGroup FilterGroup { get; }

        public IEnumerable<OrderOption> OrderingColumns { get; }

        public SelectQuery(string entityName, IEnumerable<string> columns, FilterGroup filterGroup, IEnumerable<OrderOption> orderingColumns, int count, int offset)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Columns = columns ?? Enumerable.Empty<string>();
            FilterGroup = filterGroup;
            OrderingColumns = orderingColumns ?? Enumerable.Empty<OrderOption>();
            Count = count;
            Offset = offset;
        }
    }
}
