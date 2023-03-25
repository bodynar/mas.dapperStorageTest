namespace MAS.DapperStorageTest.Models
{
    using MAS.DapperStorageTest.Infrastructure.FilterBuilder;

    /// <summary>
    /// Filter item configuration
    /// </summary>
    public class ApiFilter
    {
        /// <summary>
        /// Filter nam,e
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Column name for comparison
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Comparison value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Comparison type
        /// </summary>
        public ComparisonType ComparisonType { get; set; }

        public static implicit operator FilterItem(ApiFilter apiFilter)
        {
            return new FilterItem
            {
                FieldName = apiFilter.FieldName,
                Value = apiFilter.Value,
                LogicalComparisonType = apiFilter.ComparisonType,
                Name = apiFilter.Name,
            };
        }
    }
}
