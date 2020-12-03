namespace MAS.DapperStorageTest.Exceptions
{
    using System;

    [Serializable]
    public class DataServiceException : System.Exception
    {
        public DataServiceException() { }

        public DataServiceException(string message) : base(message) { }

        public DataServiceException(string message, System.Exception inner) : base(message, inner) { }

        protected DataServiceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
