namespace MAS.DapperStorageTest.Infrastructure
{
    public enum LogLevel
    {
        Default = 0,
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4
    }

    public interface ILogger
    {
        void Log(LogLevel logLevel, string message);

        void Debug(string message);

        void Info(string message);

        void Warn(string message);

        void Error(string message);
    }
}
