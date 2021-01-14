namespace MAS.DapperStorageTest.Infrastructure
{
    using System.Data;

    public interface IDbConnectionFactory
    {
        string DatabaseName { get; }

        DbConnectionQueryOptions QueryOptions { get; }

        IDbConnection CreateDbConnection();
    }

    public class DbConnectionQueryOptions
    {
        public int MaxRowCount { get; set; }
    }
}
