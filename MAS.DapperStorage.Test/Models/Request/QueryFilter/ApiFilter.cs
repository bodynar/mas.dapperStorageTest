namespace MAS.DapperStorageTest.Models
{
    using MAS.DappertStorageTest.Cqrs;

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

        public static implicit operator QueryFilterItem(ApiFilter apiFilter)
        {
            return new QueryFilterItem(apiFilter.Name, apiFilter.FieldName, apiFilter.Value, apiFilter.ComparisonType);
        }
    }
}
