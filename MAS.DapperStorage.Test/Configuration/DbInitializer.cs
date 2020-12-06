namespace MAS.DapperStorageTest.Configuration
{
    using System;

    using MAS.DapperStorageTest.Infrastructure;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class DbInitializer
    {
        public static IHost InitializeDbIfNotExists(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();

                    using (var connection = dbConnectionFactory.CreateDbConnection())
                    {
                        // todo: init db from sql file
                        // add if else in sql
                        var b = 10;
                    }
                }
                catch (Exception ex)
                {
                    var a = 10;
                }
            }

            return host;
        }
    }
}
