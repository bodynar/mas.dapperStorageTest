namespace MAS.DapperStorageTest.Models
{
    using System.Collections.Generic;

    public class UpdateResponse : BaseResponse
    {
        public int RowsAffected { get; }

        public UpdateResponse(int rowsAffected, IEnumerable<string> warnings)
            : base(warnings)
        {
            RowsAffected = rowsAffected;
        }
    }
}
