namespace MAS.DapperStorageTest.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class BaseResponse
    {
        public IEnumerable<string> Warnings { get; }

        public BaseResponse(IEnumerable<string> warnings)
        {
            Warnings = warnings ?? Enumerable.Empty<string>();
        }
    }
}
