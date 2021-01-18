namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Tests;

    using Moq;

    using Xunit;

    public class BaseCqrsTests : BaseTests
    {
        protected IDbConnectionFactory DbConnectionFactory { get; private set; }

        protected IFilterBuilder FilterBuilder { get; private set; }

        protected IDbAdapter DbAdapter { get; private set; }

        private KeyValuePair<string, dynamic>? LastQuery { get; set; }

        private KeyValuePair<string, dynamic>? LastCommand { get; set; }

        public BaseCqrsTests()
        {
            Init();
        }

        /// <summary>
        /// Assert sql query equality
        /// </summary>
        /// <param name="expectedSqlQuery">Expected sql statements</param>
        /// <param name="actualSqlQuery">Actual sql statements</param>
        protected void AssertSqlQuery(string expectedSqlQuery, string actualSqlQuery)
        {
            expectedSqlQuery = $"USE [{DbConnectionFactory.DatabaseName}];{Environment.NewLine}{expectedSqlQuery}";
            Assert.Equal(expectedSqlQuery, actualSqlQuery);
        }

        /// <summary>
        /// Get configuration of last query
        /// </summary>
        /// <exception cref="Exception">No last query configuration data found.</exception>
        /// <returns>Last query configuration</returns>
        protected KeyValuePair<string, dynamic> GetLastQuery()
        {
            if (!LastQuery.HasValue)
            {
                throw new Exception($"{nameof(LastQuery)} is null.");
            }

            var copy = new KeyValuePair<string, dynamic>(LastQuery.Value.Key, LastQuery.Value.Value);
            LastQuery = null;

            return copy;
        }

        /// <summary>
        /// Get configuration of last command
        /// </summary>
        /// <exception cref="Exception">No last command configuration data found.</exception>
        /// <returns>Last command configuration</returns>
        protected KeyValuePair<string, dynamic> GetLastCommand()
        {
            if (!LastCommand.HasValue)
            {
                throw new Exception($"{nameof(LastCommand)} is null.");
            }

            var copy = new KeyValuePair<string, dynamic>(LastCommand.Value.Key, LastCommand.Value.Value);
            LastCommand = null;

            return copy;
        }

        /// <summary>
        /// Assert sql configuration arguments.
        /// Asserts equality of argument keys and values sequently
        /// </summary>
        /// <param name="arguments">Actual arguments</param>
        /// <param name="valuePairs">Expected arguments</param>
        protected void AssertArguments(object arguments, IEnumerable<KeyValuePair<string, object>> valuePairs)
        {
            var objectKeys = arguments.GetType().GetProperties();
            var objectKeyNames = objectKeys.Select(x => x.Name.ToLower());

            var hasNotPresentedKeys = valuePairs.Any(pair => !objectKeyNames.Contains(pair.Key.ToLower()));

            Assert.False(hasNotPresentedKeys);

            foreach (var key in objectKeys)
            {
                var actualValue = key.GetValue(arguments);
                var expectedValue = valuePairs.First(x => x.Key == key.Name).Value;

                Assert.Equal(expectedValue, actualValue);
            }
        }

        private void Init()
        {
            var mockConnection = new Mock<IDbConnection>();
            var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();
            var mockDbAdapter = new Mock<IDbAdapter>();

            var mockFilterBuilder = new Mock<IFilterBuilder>();

            mockDbConnectionFactory
                .Setup(x => x.CreateDbConnection())
                .Returns(mockConnection.Object);

            mockDbConnectionFactory
                .SetupGet(x => x.DatabaseName)
                .Returns("Test");

            mockFilterBuilder
                .Setup(x => x.Build(It.IsAny<QueryFilterGroup>()))
                .Returns(() => (string.Empty, new System.Dynamic.ExpandoObject()));

            mockDbAdapter
                .Setup(x => x.Query(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .Returns<IDbConnection, string, dynamic>((_, sqlQuery, arguments) =>
                {
                    if (LastQuery != null)
                    {
                        throw new Exception($"{nameof(LastQuery)} isn't null");
                    }

                    LastQuery = new KeyValuePair<string, dynamic>(sqlQuery, arguments);

                    return Enumerable.Empty<IDictionary<string, dynamic>>();
                });

            mockDbAdapter
                .Setup(x => x.Execute(It.IsAny<IDbConnection>(), It.IsAny<string>(), It.IsAny<object>()))
                .Returns<IDbConnection, string, dynamic>((_, sqlQuery, arguments) =>
                {
                    if (LastCommand != null)
                    {
                        throw new Exception($"{nameof(LastCommand)} isn't null");
                    }

                    LastCommand = new KeyValuePair<string, dynamic>(sqlQuery, arguments);

                    return default;
                });

            DbConnectionFactory = mockDbConnectionFactory.Object;
            FilterBuilder = mockFilterBuilder.Object;
            DbAdapter = mockDbAdapter.Object;
        }
    }
}
