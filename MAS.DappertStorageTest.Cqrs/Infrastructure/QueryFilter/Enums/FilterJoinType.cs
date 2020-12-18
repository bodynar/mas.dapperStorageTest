namespace MAS.DappertStorageTest.Cqrs
{
    public enum FilterJoinType
    {
        None = 0,

        [SqlOperator("AND")]
        And = 1,

        [SqlOperator("OR")]
        Or = 2
    }
}
