namespace MAS.DapperStorageTest.Infrastructure
{
    using System.Data;

    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection();
    }
}
