namespace MAS.DapperStorageTest.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base of api response
    /// </summary>
    public abstract class BaseResponse
    {
        /// <summary>
        /// Warnings, appeared in process
        /// </summary>
        public IEnumerable<string> Warnings { get; }

        /// <summary>
        /// Initializing <see cref="BaseResponse"/>
        /// </summary>
        /// <param name="warnings">Warnings, appeared in process</param>
        public BaseResponse(IEnumerable<string> warnings)
        {
            Warnings = warnings ?? Enumerable.Empty<string>();
        }
    }
}
