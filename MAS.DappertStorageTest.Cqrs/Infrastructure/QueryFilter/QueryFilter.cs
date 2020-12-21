namespace MAS.DappertStorageTest.Cqrs
{
    using System;

    public class QueryFilter
    {
        public string Name { get; }

        public string EntityName { get; }

        public string FieldName { get; }

        public object Value { get; }

        public ComparisonType ComparisonType { get; }

        public QueryFilter(string name, string entityName, string fieldName, object value, ComparisonType comparisonType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            Value = value ?? throw new ArgumentNullException(nameof(value));
            ComparisonType = comparisonType;
        }
    }
}
