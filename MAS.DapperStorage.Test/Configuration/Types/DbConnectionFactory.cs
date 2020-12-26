namespace MAS.DapperStorageTest.Configuration
{
    using System;
    using System.Data;

    using MAS.DapperStorageTest.Infrastructure;

    using Microsoft.Data.SqlClient;

    internal class DbConnectionFactory : IDbConnectionFactory
    {
        private string ConnectionString { get; }

        public string DatabaseName { get; }

        public DbConnectionFactory(string connectionString, string databaseName)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
        }

        public IDbConnection CreateDbConnection()
            => new SqlConnection(ConnectionString);
    }
}
