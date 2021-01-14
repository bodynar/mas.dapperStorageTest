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

        public DbConnectionQueryOptions QueryOptions { get; }

        public DbConnectionFactory(string connectionString, string databaseName, DbConnectionQueryOptions queryOptions)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            QueryOptions = queryOptions ?? throw new ArgumentNullException(nameof(queryOptions));
        }

        public IDbConnection CreateDbConnection()
            => new SqlConnection(ConnectionString);
    }
}
