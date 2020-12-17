namespace MAS.DappertStorageTest.Cqrs
{
    using System;

    public class QueryFilter
    {
        public string EntityName { get; }

        public string FieldName { get; }

        public string FilterValue { get; }

        public ComparisonType ComparisonType { get; }

        public QueryFilter(string entityName, string fieldName, string filterValue, ComparisonType comparisonType)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            FilterValue = filterValue ?? throw new ArgumentNullException(nameof(filterValue));
            ComparisonType = comparisonType;
        }
    }
}
