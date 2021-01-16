namespace MAS.DapperStorageTest.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Configuration for select query
    /// </summary>
    public class SelectRequest
    {
        /// <summary>
        /// Name of entity (table name in database)
        /// </summary>
        [Required]
        public string EntityName { get; set; }

        /// <summary>
        /// Entities count to gather
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Offset from 0 to start gathering entities after filtering
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Filter configuration
        /// </summary>
        public ApiFilterGroup Filters { get; set; }

        /// <summary>
        /// Column names
        /// </summary>
        public IEnumerable<string> Columns { get; set; } = Enumerable.Empty<string>();

        /// <summary>
        /// Order configuration
        /// </summary>
        public IEnumerable<ApiOrderOption> OrderingColumns { get; set; } = Enumerable.Empty<ApiOrderOption>();
    }
}
