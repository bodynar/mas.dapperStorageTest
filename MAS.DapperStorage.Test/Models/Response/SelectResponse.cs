namespace MAS.DapperStorageTest.Models
{
    using System;
    using System.Collections.Generic;

    public class SelectResponse
    {
        public IEnumerable<IDictionary<string, object>> Entities { get; }

        public string EntityName { get; }

        public int Count { get; }

        public int Offset { get; }

        public IEnumerable<string> Columns { get; }

        public SelectResponse(IEnumerable<IDictionary<string, object>> entities, string entityName, int count, int offset, IEnumerable<string> columns)
        {
            Entities = entities ?? throw new ArgumentNullException(nameof(entities));
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Count = count;
            Offset = offset;
            Columns = columns ?? throw new ArgumentNullException(nameof(columns));
        }
    }
}
