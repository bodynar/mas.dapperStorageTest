namespace MAS.DapperStorageTest.Configuration
{
    using System;
    using System.IO;
    using System.Reflection;

    using MAS.DapperStorageTest.Infrastructure;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;

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
                    .AddSwaggerGen(opts => {
                        opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Data service", Version = "v1" });

                        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                        opts.IncludeXmlComments(xmlPath);
                    })
                    .AddSingleton<IDbConnectionFactory>((_) => new DbConnectionFactory(connectionString, dbName, queryOptions))
            ;
        }
    }
}
