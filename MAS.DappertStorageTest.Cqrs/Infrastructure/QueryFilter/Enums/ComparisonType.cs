namespace MAS.DappertStorageTest.Cqrs
{
    public enum ComparisonType
    {
        [SqlOperator("NONE")]
        None = 0,

        [SqlOperator("=")]
        Equal = 1,

        [SqlOperator("!=")]
        NotEqual = 2
    }
}
