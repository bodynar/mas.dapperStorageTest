namespace MAS.DapperStorageTest
{
    using System;

    using MAS.DappertStorageTest.Cqrs;

    public class ConnectionData : IConnectionData
    {
        public string ConnectionString { get; }

        public ConnectionData(string connectionString)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
    }
}
