namespace MAS.DapperStorageTest.Infrastructure.FilterBuilder
{
    /// <summary>
    /// Filter.
    /// Represents single sql column value logical comparison
    /// </summary>
    public class FilterItem
    {
        /// <summary>
        /// Unique name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Column name
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Comparison value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Value comparison type
        /// </summary>
        public ComparisonType LogicalComparisonType { get; set; }

        /// <summary>
        /// Create instance of <see cref="FilterItem"/>
        /// </summary>
        public FilterItem() { }

        /// <summary>
        /// Create instance of <see cref="FilterItem"/>
        /// </summary>
        /// <param name="name">Unique filter name</param>
        /// <param name="fieldName">Column name</param>
        /// <param name="value">Comparison value</param>
        /// <param name="comparisonType">Value comparison type</param>
        public FilterItem(string name, string fieldName, object value, ComparisonType comparisonType)
        {
            Name = name;
            FieldName = fieldName;
            Value = value;
            LogicalComparisonType = comparisonType;
        }
    }
}
