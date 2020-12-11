namespace MAS.DapperStorageTest.Configuration
{
    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Cqrs;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    using Microsoft.Extensions.Configuration;

    using SimpleInjector;

    public static class ContainerConfiguration
    {
        public static Container Configure(this Container container, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // TODO
            container.Register(typeof(IResolver), typeof(Resolver), Lifestyle.Singleton);
            container.Register<IDbConnectionFactory>(() => new DbConnectionFactory(connectionString), Lifestyle.Singleton);

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
