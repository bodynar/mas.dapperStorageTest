namespace MAS.DappertStorageTest.Cqrs.Tests
{
    using System.Data;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Tests;

    using Moq;

    public class BaseCqrsTests: BaseTests
    {
        protected IDbConnectionFactory DbConnectionFactory { get; private set; }

        protected IFilterBuilder FilterBuilder { get; private set; }

        public BaseCqrsTests()
        {
            Init();
        }

        private void Init()
        {
            var mockConnection = new Mock<IDbConnection>();
            var mockDbConnectionFactory = new Mock<IDbConnectionFactory>();

            var mockFilterBuilder = new Mock<IFilterBuilder>();

            mockDbConnectionFactory
                .Setup(x => x.CreateDbConnection())
                .Returns(mockConnection.Object);

            mockFilterBuilder
                .Setup(x => x.Build(It.IsAny<QueryFilterGroup>()))
                .Returns(() => (string.Empty, new System.Dynamic.ExpandoObject())); // TODO: configure it from outside

            DbConnectionFactory = mockDbConnectionFactory.Object;
            FilterBuilder = mockFilterBuilder.Object;
        }
    }
}
