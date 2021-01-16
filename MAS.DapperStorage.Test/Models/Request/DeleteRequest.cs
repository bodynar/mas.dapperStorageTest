namespace MAS.DapperStorageTest.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Configuration for delete command
    /// </summary>
    public class DeleteRequest
    {
        /// <summary>
        /// Name of entity (table name in database)
        /// </summary>
        [Required]
        public string EntityName { get; set; }

        /// <summary>
        /// Specific entity id
        /// </summary>
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Filters configuration
        /// </summary>
        public ApiFilterGroup Filters { get; set; }
    }
}
