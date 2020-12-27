namespace MAS.DappertStorageTest.Cqrs
{
    using System;

    public class QueryFilterItem
    {
        public string Name { get; }

        public string FieldName { get; }

        public object Value { get; }

        public ComparisonType ComparisonType { get; }

        public QueryFilterItem(string name, string fieldName, object value, ComparisonType comparisonType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            Value = value ?? throw new ArgumentNullException(nameof(value));
            ComparisonType = comparisonType;
        }
    }
}
