namespace MAS.DapperStorageTest.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class SelectRequest
    {
        public string EntityName { get; set; }

        public int Count { get; set; }

        public int Offset { get; set; }

        public ApiFilterGroup Filters { get; set; }

        public IEnumerable<string> Columns { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<string> OrderingColumns { get; set; } = Enumerable.Empty<string>();
    }
}
