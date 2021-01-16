namespace MAS.DapperStorageTest.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Result of update command
    /// </summary>
    public class UpdateResponse : BaseResponse
    {
        /// <summary>
        /// Amount of updated entities
        /// </summary>
        public int RowsAffected { get; }

        /// <summary>
        /// Initializing <see cref="UpdateResponse"/>
        /// </summary>
        /// <param name="rowsAffected">Amount of updated entities</param>
        /// <param name="warnings">Warnings, appeared in process</param>
        public UpdateResponse(int rowsAffected, IEnumerable<string> warnings)
            : base(warnings)
        {
            RowsAffected = rowsAffected;
        }
    }
}
