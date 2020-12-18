namespace MAS.DappertStorageTest.Cqrs
{
    public enum ComparisonType
    {
        [SqlOperator("NONE")]
        None = 0,

        [SqlOperator("EQUAL")]
        Equal = 1,

        [SqlOperator("NOT EQUAL")]
        NotEqual = 2
    }
}
