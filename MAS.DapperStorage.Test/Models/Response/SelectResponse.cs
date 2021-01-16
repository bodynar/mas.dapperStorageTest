namespace MAS.DapperStorageTest.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Result of select query
    /// </summary>
    public class SelectResponse : BaseResponse
    {
        /// <summary>
        /// Selected entities
        /// <para>Field name - field value</para>
        /// </summary>
        public IEnumerable<IDictionary<string, object>> Entities { get; }

        /// <summary>
        /// Entity name
        /// </summary>
        public string EntityName { get; }

        /// <summary>
        /// Selected entities count
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Offset of selection
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Column names
        /// </summary>
        public IEnumerable<string> Columns { get; }

        /// <summary>
        /// Initializing <see cref="SelectResponse"/>
        /// </summary>
        /// <param name="entities">Entities</param>
        /// <param name="entityName">Entity name</param>
        /// <param name="count">Entities count</param>
        /// <param name="offset">Offset of selection</param>
        /// <param name="columns">Column names</param>
        /// <param name="warnings">Warnings, appeared in process</param>
        public SelectResponse(
            IEnumerable<IDictionary<string, object>> entities, string entityName,
            int count, int offset,
            IEnumerable<string> columns, IEnumerable<string> warnings)
            : base(warnings)
        {
            Entities = entities ?? throw new ArgumentNullException(nameof(entities));
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Count = count;
            Offset = offset;
            Columns = columns ?? throw new ArgumentNullException(nameof(columns));
        }
    }
}
