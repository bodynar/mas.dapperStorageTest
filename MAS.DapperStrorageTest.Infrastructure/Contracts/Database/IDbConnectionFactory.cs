namespace MAS.DapperStorageTest.Infrastructure
{
    using System.Data;

    public interface IDbConnectionFactory
    {
        string DatabaseName { get; }

        IDbConnection CreateDbConnection();
    }
}
