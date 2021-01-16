namespace MAS.DapperStorageTest.Models
{
    using System;

    public class DeleteRequest
    {
        public Guid? EntityId { get; set; }

        public string EntityName { get; set; }

        public ApiFilterGroup Filters { get; set; }
    }
}
