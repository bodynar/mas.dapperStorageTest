namespace MAS.DapperStorageTest.Models
{
    using System;

    public class InsertResponse
    {
        public Guid EntityId { get; }

        public InsertResponse(Guid entityId)
        {
            EntityId = entityId;
        }
    }
}
