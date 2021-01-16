namespace MAS.DapperStorageTest.Models
{
    public class DeleteResponse
    {
        public int RowsAffected { get; }

        public DeleteResponse(int rowsAffected)
        {
            RowsAffected = rowsAffected;
        }
    }
}
