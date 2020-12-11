namespace MAS.DapperStorageTest.Models
{
    using System.Collections.Generic;

    public class InsertRequest
    {
        public string EntityName { get; set; }

        public IDictionary<string, string> Values { get; set; }
    }
}
