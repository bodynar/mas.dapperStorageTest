namespace MAS.DapperStorageTest.Infrastructure
{
    using System;

    [Serializable]
    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException(string message) : base(message) { }
    }
}
