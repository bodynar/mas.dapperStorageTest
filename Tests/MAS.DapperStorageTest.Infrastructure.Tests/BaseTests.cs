namespace MAS.DapperStorageTest.Infrastructure.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Moq;

    public class BaseTests
    {
        protected ILogger Logger { get; private set; }

        protected List<KeyValuePair<LogLevel, string>> Logs { get; private set; }

        public BaseTests()
        {
            Init();
        }

        protected IEnumerable<string> GetLogs(LogLevel logLevel)
            => Logs.Where(logItem => logItem.Key == logLevel).Select(x => x.Value);

        #region Not public API

        private void Init()
        {
            var loggerMock = new Mock<ILogger>();

            loggerMock
                .Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<string>()))
                .Callback<LogLevel, string>((level, message) => Logs.Add(new KeyValuePair<LogLevel, string>(level, message)));

            loggerMock
                .Setup(x => x.Debug(It.IsAny<string>()))
                .Callback<string>(message => Logs.Add(new KeyValuePair<LogLevel, string>(LogLevel.Debug, message)));

            loggerMock
                .Setup(x => x.Info(It.IsAny<string>()))
                .Callback<string>(message => Logs.Add(new KeyValuePair<LogLevel, string>(LogLevel.Info, message)));

            loggerMock
                .Setup(x => x.Warn(It.IsAny<string>()))
                .Callback<string>(message => Logs.Add(new KeyValuePair<LogLevel, string>(LogLevel.Warn, message)));

            loggerMock
                .Setup(x => x.Error(It.IsAny<string>()))
                .Callback<string>(message => Logs.Add(new KeyValuePair<LogLevel, string>(LogLevel.Error, message)));

            Logger = loggerMock.Object;
            Logs = new List<KeyValuePair<LogLevel, string>>();
        }

        #endregion
    }
}
