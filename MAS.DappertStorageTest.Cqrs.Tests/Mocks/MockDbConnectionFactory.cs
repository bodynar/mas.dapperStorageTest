namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System.Data;

    using MAS.DapperStorageTest.Infrastructure;

    internal sealed class MockDbConnectionFactory : IDbConnectionFactory
    {
        public MockDbConnection Connection { get; } = new MockDbConnection();

        public IDbConnection CreateDbConnection()
        {
            return Connection;
        }
    }
}
