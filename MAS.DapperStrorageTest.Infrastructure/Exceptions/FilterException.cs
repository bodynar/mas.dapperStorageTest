namespace MAS.DapperStorageTest.Infrastructure
{
    using System;

    [Serializable]
    public class FilterException : System.Exception
    {
        public FilterException(Type callerType, string message)
            : this(callerType.Name, message) { }

        public FilterException(string callerTypeName, string message)
            : base($"{callerTypeName}: {message}") { }
    }
}
