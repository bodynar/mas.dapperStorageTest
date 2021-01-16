namespace MAS.DapperStorageTest.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Configuration for update command
    /// </summary>
    public class UpdateRequest
    {
        /// <summary>
        /// Name of entity (table name in database)
        /// </summary>
        [Required]
        public string EntityName { get; set; }

        /// <summary>
        /// Entity data
        /// <para>Field name - field value pairs</para>
        /// </summary>
        [Required]
        public IDictionary<string, string> Values { get; set; }

        /// <summary>
        /// Filters configuration
        /// </summary>
        public ApiFilterGroup Filters { get; set; }
    }
}
