namespace MAS.DapperStorageTest.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Result of insert operation
    /// </summary>
    public class InsertResponse: BaseResponse
    {
        /// <summary>
        /// Identifier of created entity
        /// </summary>
        public Guid EntityId { get; }

        /// <summary>
        /// Initializing <see cref="InsertResponse"/>.
        /// </summary>
        /// <param name="entityId">Identifier of created entity</param>
        /// <param name="warnings">Warnings, appeared in process</param>
        public InsertResponse(Guid entityId, IEnumerable<string> warnings)
            : base(warnings)
        {
            EntityId = entityId;
        }
    }
}
