namespace MAS.DapperStorageTest.Configuration
{
    using MAS.DapperStorageTest.Infrastructure;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using SimpleInjector;

    public static class ServicesConfiguration
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration, Container container)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var dbName = configuration.GetValue<string>("DatabaseName");
            var maxQueryCount = configuration.GetValue<int>("MaxQueryRows");

            var queryOptions = new DbConnectionQueryOptions
            {
                MaxRowCount = maxQueryCount
            };

            services.AddControllers()
                .Services
                    .AddSimpleInjector(container, opts => 
                        opts
                            .AddAspNetCore()
                            .AddControllerActivation()
                    )
            ;

            services.AddSingleton<IDbConnectionFactory>((_) => new DbConnectionFactory(connectionString, dbName, queryOptions));
        }
    }
}
