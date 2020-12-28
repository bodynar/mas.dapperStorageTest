namespace MAS.DapperStorageTest.Models
{
    using MAS.DappertStorageTest.Cqrs;

    public class ApiFilter
    {
        public string Name { get; set; }

        public string FieldName { get; set; }

        public string Value { get; set; }

        public ComparisonType ComparisonType { get; set; }

        public static implicit operator QueryFilterItem(ApiFilter apiFilter)
        {
            return new QueryFilterItem(apiFilter.Name, apiFilter.FieldName, apiFilter.Value, apiFilter.ComparisonType);
        }
    }
}
