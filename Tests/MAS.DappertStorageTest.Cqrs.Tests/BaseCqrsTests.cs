namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Dynamic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Tests;

    using Moq;

    using Xunit;

    public class BaseCqrsTests : BaseTests
    {
        /// <summary>
        /// Mock of DbConnectionFactory
        /// </summary>
        protected IDbConnectionFactory DbConnectionFactory { get; private set; }

        /// <summary>
        /// Mock of FilterBuilder
        /// </summary>
        protected IFilterBuilder FilterBuilder { get; private set; }

        /// <summary>
        /// Mock of DbAdapter
        /// </summary>
        protected IDbAdapter DbAdapter { get; private set; }

        /// <summary>
        /// Tested query options
        /// </summary>
        protected DbConnectionQueryOptions MockQueryOptions { get; private set; } = new DbConnectionQueryOptions() { MaxRowCount = 100 };

        /// <summary>
        /// Empty QueryFilterGroup without fields and groups
        /// </summary>
        protected QueryFilterGroup EmptyFilterGroup { get; } =
            new QueryFilterGroup("Test", "Test", FilterJoinType.None, Enumerable.Empty<QueryFilterItem>());

        /// <summary>
        /// Strict filter builder first argument result
        /// </summary>
        protected string FilterBuilderSqlResult { get; set; }

        /// <summary>
        /// Strict filter builder second argument result
        /// </summary>
        protected IDictionary<string, object> FilterBuilderArgumentsResult { get; set; }

        /// <summary>
        /// Static result of executing command Execute of mock DbAdapter
        /// </summary>
        protected int DbAdapterExecuteResult { get; } = 100;

        #region Private members

        private IEnumerable<string> ParamNamesToExcludeFromCheck
            => new[] { "NewEntityId", "NewEntityCreatedAt", "UpdateEntityModifiedOn" };

        private KeyValuePair<string, dynamic>? LastQuery { get; set; }

        private KeyValuePair<string, dynamic>? LastCommand { get; set; }

        #endregion

        /// <summary>
        /// Base cqrs tests configuration
        /// </summary>
        public BaseCqrsTests()
        {
            Init();
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
        /// Assert sql configuration arguments.
        /// Asserts equality of argument keys and values sequently
        /// </summary>
        /// <param name="expected">Expected arguments</param>
        /// <param name="actual">Actual arguments</param>
        protected void AssertArguments(IEnumerable<KeyValuePair<string, object>> expected, object actual)
        {
            if (actual is ExpandoObject)
            {
                AssertArguments(expected, actual as ExpandoObject);
            }
            else
            {
                var objectKeys = actual.GetType().GetProperties();
                var objectKeyNames = objectKeys.Select(x => x.Name).Except(ParamNamesToExcludeFromCheck);

                var hasNotPresentedKeys = expected.Any(pair => !objectKeyNames.Contains(pair.Key));

                Assert.False(hasNotPresentedKeys);

                foreach (var key in objectKeys)
                {
                    var actualValue = key.GetValue(actual);
                    var expectedValue = expected.First(x => x.Key == key.Name).Value;

                    Assert.Equal(expectedValue, actualValue);
                }
            }
        }

        #region Private methods

        private void AssertArguments(IEnumerable<KeyValuePair<string, object>> expected, ExpandoObject actual)
        {
            var actualArguments = actual.Where(x => !ParamNamesToExcludeFromCheck.Contains(x.Key));
            var objectKeyNames = actualArguments.Select(x => x.Key);

            var hasNotPresentedKeys = expected.Any(pair => !objectKeyNames.Contains(pair.Key));

            Assert.False(hasNotPresentedKeys);

            foreach (var pair in actualArguments)
            {
                var expectedValue = expected.First(x => x.Key == pair.Key).Value;

                Assert.Equal(expectedValue, pair.Value);
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

            mockDbConnectionFactory
                .SetupGet(x => x.QueryOptions)
                .Returns(MockQueryOptions);

            mockFilterBuilder
                .Setup(x => x.Build(It.IsAny<QueryFilterGroup>()))
                .Returns(() =>
                {
                    var expandoObject = new ExpandoObject();

                    if (FilterBuilderArgumentsResult != null)
                    {
                        foreach (var pair in FilterBuilderArgumentsResult)
                        {
                            var result = expandoObject.TryAdd(pair.Key, pair.Value);
                        }
                    }

                    return (FilterBuilderSqlResult, expandoObject);
                });

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

                    return DbAdapterExecuteResult;
                });

            DbConnectionFactory = mockDbConnectionFactory.Object;
            FilterBuilder = mockFilterBuilder.Object;
            DbAdapter = mockDbAdapter.Object;
        }

        #endregion
    }
}
