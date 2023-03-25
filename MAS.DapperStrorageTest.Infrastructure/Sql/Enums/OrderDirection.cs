namespace MAS.DapperStorageTest.Infrastructure.Sql
{
    public enum OrderDirection
    {
        [SqlOperator("ASC")]
        Ascending = 0,

        [SqlOperator("DESC")]
        Descending = 1,
    }
}
