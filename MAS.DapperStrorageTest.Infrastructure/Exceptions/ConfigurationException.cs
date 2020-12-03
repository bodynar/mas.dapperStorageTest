namespace MAS.DapperStorageTest.Infrastructure
{
    using System;

    [Serializable]
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string type)
            : base($"Configuration exception. See {type}.") { }
    }
}
