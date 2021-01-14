namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;

    public class SelectQueryResponse
    {
        public string EntityName { get; }

        public IEnumerable<IDictionary<string, object>> Entities { get; }

        public int Count { get; }

        public int Offset { get; }

        public IEnumerable<string> Columns { get; }

        public IEnumerable<OrderOption> OrderingColumns { get; }

        public IEnumerable<string> Warnings { get; }

        public SelectQueryResponse(
            string entityName, IEnumerable<IDictionary<string, object>> entities,
            int count, int offset,
            IEnumerable<string> columns, IEnumerable<OrderOption> orderingColumns,
            IEnumerable<string> warnings)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Entities = entities ?? throw new ArgumentNullException(nameof(entities));
            Count = count;
            Offset = offset;
            Columns = columns ?? throw new ArgumentNullException(nameof(columns));
            OrderingColumns = orderingColumns ?? throw new ArgumentNullException(nameof(orderingColumns));
        }
    }
}
