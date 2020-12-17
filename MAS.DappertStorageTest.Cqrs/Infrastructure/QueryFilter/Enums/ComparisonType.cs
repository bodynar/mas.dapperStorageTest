namespace MAS.DappertStorageTest.Cqrs
{
    public enum ComparisonType
    {
        [ComparisonOperator("NONE")]
        None = 0,

        [ComparisonOperator("EQUAL")]
        Equal = 1,

        [ComparisonOperator("NOT EQUAL")]
        NotEqual = 2
    }
}
