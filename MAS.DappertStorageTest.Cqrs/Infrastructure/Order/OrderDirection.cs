namespace MAS.DappertStorageTest.Cqrs
{
    public enum OrderDirection
    {
        [SqlOperator("ASC")]
        Ascending = 0,

        [SqlOperator("DESC")]
        Descending = 1,
    }
}
