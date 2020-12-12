namespace MAS.DapperStorageTest.Models
{
    using System;
    using System.Collections.Generic;

    public enum ComparisonType
    {
        Equal,
        NotEqual
    }

    public class SelectRequest
    {
        public string EntityName { get; set; }

        public IEnumerable<string> Fields { get; set; }

        public Guid? EntityId { get; set; }

        public IEnumerable<SelectQueryFilter> SelectQueryFilters { get; }
    }

    public class SelectQueryFilter
    {
        public string FieldName { get; }

        public string FilterValue { get; }

        public ComparisonType ComparisonType { get; }

        public SelectQueryFilter(string fieldName, string filterValue, ComparisonType comparisonType)
        {
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            FilterValue = filterValue ?? throw new ArgumentNullException(nameof(filterValue));
            ComparisonType = comparisonType;
        }
    }
}
