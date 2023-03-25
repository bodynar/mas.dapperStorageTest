namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure.Sql;
    using MAS.DapperStorageTest.Models;

    using Xunit;

    public class SelectQueryHandlerTests : CommonCqrsTests
    {
        [Fact]
        public void ShouldThrowDatabaseOperationExceptionWhenEntityNameIsNotValid()
        {
            var entityName = "TESTENTITYNAME";
            var query = new SelectQuery(entityName, null, null, null, 1, 1);
            var handler = new SelectQueryHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            ShouldThrowDatabaseOperationExceptionWhenEntityNameIsNotValidInternal(entityName, query, handler);
        }

        [Fact]
        public void ShouldGenerateSqlQueryWithSpecifiedColumns()
        {
            var entityName = nameof(Passenger);
            var columns = new[] { nameof(Passenger.Id), nameof(Passenger.BirthDate) };
            var expectedArguments = Enumerable.Empty<KeyValuePair<string, object>>();
            var expectedSqlQuery = "SELECT [Id], [BirthDate] FROM [Passenger]";
            var query = new SelectQuery(entityName, columns, null, null, 0, 0);
            var handler = new SelectQueryHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            handler.Handle(query);
            var lastQuery = GetLastQuery();

            AssertSqlQuery(expectedSqlQuery, lastQuery.Key);
            AssertArguments(expectedArguments, lastQuery.Value);
        }

        [Fact]
        public void ShouldGenerateSqlQueryWithAllColumns()
        {
            var entityName = nameof(Passenger);
            var columns = Enumerable.Empty<string>();
            var expectedArguments = Enumerable.Empty<KeyValuePair<string, object>>();
            var expectedSqlQuery = "SELECT * FROM [Passenger]";
            var query = new SelectQuery(entityName, columns, null, null, 0, 0);
            var handler = new SelectQueryHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            handler.Handle(query);
            var lastQuery = GetLastQuery();

            AssertSqlQuery(expectedSqlQuery, lastQuery.Key);
            AssertArguments(expectedArguments, lastQuery.Value);
        }

        [Fact]
        public void ShouldGenerateSqlQueryWithWhereCondition()
        {
            FilterBuilderSqlResult = "(TEST)";
            var entityName = nameof(Passenger);
            var columns = Enumerable.Empty<string>();
            var testedFilterGroup = GetTestFilterGroup();
            var expectedArguments = Enumerable.Empty<KeyValuePair<string, object>>();
            var expectedSqlQuery = "SELECT * FROM [Passenger] WHERE (TEST)";
            var query = new SelectQuery(entityName, columns, testedFilterGroup, null, 0, 10);
            var handler = new SelectQueryHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            handler.Handle(query);
            var lastQuery = GetLastQuery();

            AssertSqlQuery(expectedSqlQuery, lastQuery.Key);
            AssertArguments(expectedArguments, lastQuery.Value);
        }

        [Fact]
        public void ShouldGenerateSqlQueryWithSorting()
        {
            var entityName = nameof(Passenger);
            var columns = Enumerable.Empty<string>();
            var orderColumns = new[] { new OrderOption(nameof(Passenger.MiddleName), OrderDirection.Ascending), new OrderOption(nameof(Passenger.LastName), OrderDirection.Ascending) };
            var expectedArguments = Enumerable.Empty<KeyValuePair<string, object>>();
            var expectedSqlQuery = "SELECT * FROM [Passenger] ORDER BY [MiddleName] ASC, [LastName] ASC";
            var query = new SelectQuery(entityName, columns, null, orderColumns, 0, 0);
            var handler = new SelectQueryHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            handler.Handle(query);
            var lastQuery = GetLastQuery();

            AssertSqlQuery(expectedSqlQuery, lastQuery.Key);
            AssertArguments(expectedArguments, lastQuery.Value);
        }

        [Fact]
        public void ShouldGenerateSqlQueryWithPaging()
        {
            var entityName = nameof(Passenger);
            var columns = Enumerable.Empty<string>();
            var expectedArguments = Enumerable.Empty<KeyValuePair<string, object>>();
            var expectedSqlQuery = "SELECT * FROM [Passenger] ORDER BY [Id] ASC OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY";
            var query = new SelectQuery(entityName, columns, null, null, 10, 10);
            var handler = new SelectQueryHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            handler.Handle(query);
            var lastQuery = GetLastQuery();

            AssertSqlQuery(expectedSqlQuery, lastQuery.Key);
            AssertArguments(expectedArguments, lastQuery.Value);
        }

        [Fact]
        public void ShouldGenerateSqlQueryWithWhereConditionOrderingAndSorting()
        {
            FilterBuilderSqlResult = "(TEST)";
            var entityName = nameof(Passenger);
            var columns = Enumerable.Empty<string>();
            var orderColumns = new[] { new OrderOption(nameof(Passenger.MiddleName), OrderDirection.Ascending), new OrderOption(nameof(Passenger.LastName), OrderDirection.Ascending) };
            var testedFilterGroup = GetTestFilterGroup();
            var expectedArguments = Enumerable.Empty<KeyValuePair<string, object>>();
            var expectedSqlQuery = "SELECT * FROM [Passenger] WHERE (TEST) ORDER BY [MiddleName] ASC, [LastName] ASC, [Id] ASC OFFSET 15 ROWS FETCH NEXT 15 ROWS ONLY";
            var query = new SelectQuery(entityName, columns, testedFilterGroup, orderColumns, 15, 15);
            var handler = new SelectQueryHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            handler.Handle(query);
            var lastQuery = GetLastQuery();

            AssertSqlQuery(expectedSqlQuery, lastQuery.Key);
            AssertArguments(expectedArguments, lastQuery.Value);
        }

        [Fact]
        public void ShouldGenerateWarningsWhenQueryContainsNotValidColumnsForEntity()
        {
            var entityName = nameof(Passenger);
            var columns = new[] { "SomeNotExistingColumn", "SomeOtherNotExistingColumn" };
            var expectedWarningsCount = 1;
            var expectedWarning = "Not found columns: [SomeNotExistingColumn, SomeOtherNotExistingColumn].";
            var query = new SelectQuery(entityName, columns, null, null, 10, 10);
            var handler = new SelectQueryHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            var result = handler.Handle(query);

            Assert.NotNull(result);
            Assert.NotNull(result.Warnings);
            Assert.Equal(expectedWarningsCount, result.Warnings.Count());
            Assert.Equal(expectedWarning, result.Warnings.First());
        }

        [Fact]
        public void ShouldGenerateWarningsWhenPagingConfigurationCountIsGreaterThanConfiguredMaxRowCount()
        {
            var entityName = nameof(Passenger);
            var expectedWarningsCount = 1;
            var expectedWarning = $"Row count cannot be greater than {DbConnectionFactory.QueryOptions.MaxRowCount}.";
            var query = new SelectQuery(entityName, Enumerable.Empty<string>(), null, null, int.MaxValue, 10);
            var handler = new SelectQueryHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            var result = handler.Handle(query);

            Assert.NotNull(result);
            Assert.NotNull(result.Warnings);
            Assert.Equal(expectedWarningsCount, result.Warnings.Count());
            Assert.Equal(expectedWarning, result.Warnings.First());
        }

        [Fact]
        public void ShouldGenerateWarningsWhenPagingConfigurationOffsetIsLessThanZero()
        {
            var entityName = nameof(Passenger);
            var expectedWarningsCount = 1;
            var expectedWarning = "Row offset cannot be less than 0.";
            var query = new SelectQuery(entityName, Enumerable.Empty<string>(), null, null, 10, -10);
            var handler = new SelectQueryHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            var result = handler.Handle(query);

            Assert.NotNull(result);
            Assert.NotNull(result.Warnings);
            Assert.Equal(expectedWarningsCount, result.Warnings.Count());
            Assert.Equal(expectedWarning, result.Warnings.First());
        }

        [Fact]
        public void ShouldGenerateWarningWhenOrderConfigurationContainsColumnsWhichNotExistsInEntity()
        {
            var entityName = nameof(Passenger);
            var orderColumns = new[] { new OrderOption("TestedColumn1", OrderDirection.Ascending), new OrderOption("TestedColumn2", OrderDirection.Ascending) };
            var expectedWarningsCount = 1;
            var expectedWarning = "Not found columns for ordering: [TestedColumn1, TestedColumn2].";
            var query = new SelectQuery(entityName, Enumerable.Empty<string>(), null, orderColumns, 10, 10);
            var handler = new SelectQueryHandler(DbConnectionFactory, DbAdapter, FilterBuilder);

            var result = handler.Handle(query);

            Assert.NotNull(result);
            Assert.NotNull(result.Warnings);
            Assert.Equal(expectedWarningsCount, result.Warnings.Count());
            Assert.Equal(expectedWarning, result.Warnings.First());
        }
    }
}
