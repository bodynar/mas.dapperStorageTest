namespace MAS.DappertStorageTest.Cqrs
{
    using System;

    public class QueryFilter
    {
        public string FieldName { get; }

        public string FilterValue { get; }

        public ComparisonType ComparisonType { get; }

        public QueryFilter(string fieldName, string filterValue, ComparisonType comparisonType)
        {
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            FilterValue = filterValue ?? throw new ArgumentNullException(nameof(filterValue));
            ComparisonType = comparisonType;
        }
    }
}
