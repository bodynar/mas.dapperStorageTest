namespace MAS.DapperStorageTest.Models
{
    using System.Collections.Generic;

    public class UpdateRequest
    {
        public string EntityName { get; set; }

        public IDictionary<string, string> Values { get; set; }

        public ApiFilterGroup Filters { get; set; }
    }
}
