namespace MAS.DapperStorageTest.Configuration
{
    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Cqrs;
    using MAS.DappertStorageTest.Cqrs;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    using Microsoft.Extensions.Configuration;

    using SimpleInjector;

    public static class ContainerConfiguration
    {
        public static Container Configure(this Container container, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var dbName = configuration.GetValue<string>("DatabaseName");
            var maxQueryCount = configuration.GetValue<int>("MaxQueryRows");

            var queryOptions = new DbConnectionQueryOptions
            {
                MaxRowCount = maxQueryCount
            };

            // TODO
            container.Register<IResolver, Resolver>(Lifestyle.Singleton);
            container.Register<IDbAdapter, DapperDbAdapter>(Lifestyle.Singleton);
            container.Register<IDbConnectionFactory>(() => new DbConnectionFactory(connectionString, dbName, queryOptions), Lifestyle.Singleton);
            container.Register<IFilterBuilder, MySqlFilterBuilder>();
            container.Register<ILogger, Logger>(Lifestyle.Singleton);

            container.Register(
                typeof(IQueryHandler<,>),
                typeof(BaseCqrsHandler).Assembly);

            container.Register(
                typeof(ICommandHandler<>),
                typeof(BaseCqrsHandler).Assembly);

            container.Register(typeof(IQueryProcessor), typeof(QueryProcessor), Lifestyle.Singleton);
            container.Register(typeof(ICommandProcessor), typeof(CommandProcessor), Lifestyle.Singleton);

            return container;
        }
    }
}
