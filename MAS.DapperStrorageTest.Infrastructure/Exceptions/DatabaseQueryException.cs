namespace MAS.DapperStorageTest.Infrastructure
{
    using System;

    [Serializable]
    public class DatabaseQueryException : Exception
    {
        public DatabaseQueryException(string message) : base(message) { }
    }
}
