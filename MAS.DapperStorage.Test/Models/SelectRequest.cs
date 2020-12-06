namespace MAS.DapperStorageTest.Models
{
    using System;
    using System.Collections.Generic;

    public class SelectRequest
    {
        public string EntityName { get; set; }

        public IEnumerable<string> Fields { get; set; }

        public Guid? EntityId { get; set; }

        public IDictionary<string, string> Filters { get; set; }
    }
}
