namespace MAS.DapperStorageTest.Configuration
{
    using System.Data;

    using MAS.DapperStorageTest.Infrastructure;

    using Microsoft.Data.SqlClient;

    internal class DbConnectionFactory : IDbConnectionFactory
    {
        private string ConnectionString { get; }

        public DbConnectionFactory(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IDbConnection CreateDbConnection()
            => new SqlConnection(ConnectionString);
    }
}
