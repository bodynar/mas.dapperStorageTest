namespace MAS.DapperStorageTest.Configuration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using SimpleInjector;

    public static class ServicesConfiguration
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration, Container container)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddControllers()
                .Services
                    .AddSimpleInjector(container, opts => 
                        opts
                            .AddAspNetCore()
                            .AddControllerActivation()
                    )
            ;
        }
    }
}
