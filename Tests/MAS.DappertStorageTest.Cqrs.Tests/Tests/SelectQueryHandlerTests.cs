namespace MAS.DappertStorageTest.Cqrs.Tests
{
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
        public void ShouldGenerateSqlQuery()
        {

        }

        [Fact]
        public void ShouldGenerateSqlQueryAndWarningsWhenQueryContainsNotValidColumnsForEntity()
        {

        }

        [Fact]
        public void ShouldGenerateWarningsWhenPagingConfigurationCountIsGreaterThanConfiguredMaxRowCount()
        {

        }

        [Fact]
        public void ShouldGenerateWarningsWhenPagingConfigurationOffsetIsLessThanZero()
        {

        }

        [Fact]
        public void ShouldGenerateSqlQueryWithPagingConfigureationWhenCountIsGreaterThanZero()
        {

        }

        [Fact]
        public void ShouldGenerateSqlQueryWithWhereFilterWhenFilterIsBuiltProperly()
        {

        }

        [Fact]
        public void ShouldGenerateWarningWhenOrderConfigurationContainsColumnsWhichNotExistsInEntity()
        {

        }

        [Fact]
        public void ShouldGenerateSqlQueryWithOrderWhenOrderConfigurationContainsValidColumns()
        {

        }
    }
}
