namespace MAS.DapperStorageTest.Models
{
    using System;
    using System.Collections.Generic;

    public class InsertResponse: BaseResponse
    {
        public Guid EntityId { get; }

        public InsertResponse(Guid entityId, IEnumerable<string> warnings)
            : base(warnings)
        {
            EntityId = entityId;
        }
    }
}
