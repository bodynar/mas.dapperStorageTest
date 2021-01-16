namespace MAS.DapperStorageTest.Models
{
    /// <summary>
    /// Result of delete command
    /// </summary>
    public class DeleteResponse
    {
        /// <summary>
        /// Amount of deleted entities
        /// </summary>
        public int RowsAffected { get; }

        /// <summary>
        /// Initializing <see cref="DeleteResponse"/>
        /// </summary>
        /// <param name="rowsAffected">Amount of deleted entities</param>
        public DeleteResponse(int rowsAffected)
        {
            RowsAffected = rowsAffected;
        }
    }
}
