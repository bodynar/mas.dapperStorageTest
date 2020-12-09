namespace MAS.DapperStorageTest.Configuration
{
    using System;
    using System.IO;
    using System.Reflection;

    using MAS.DapperStorageTest.Infrastructure;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class DbInitializer
    {
        private const string DatabaseInitFilename = "DbInit.sql";

        public static IHost InitializeDbIfNotExists(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();

                    if (dbConnectionFactory != null)
                    {
                        var sqlDatabaseInitScript = GetSqlDataBaseInitScript();

                        if (!string.IsNullOrEmpty(sqlDatabaseInitScript))
                        {
                            // TODO: When storageTest doesn't exist - here exception [StorageTest] not exist
                            using (var connection = dbConnectionFactory.CreateDbConnection())
                            {
                                var command = connection.CreateCommand();

                                command.CommandText = sqlDatabaseInitScript;

                                connection.Open();
                                if (connection.State == System.Data.ConnectionState.Open)
                                {
                                    command.ExecuteNonQuery();
                                    connection.Close();
                                    
                                    // TODO: When storageTest not included in connection string and doesn't exist - here sql exception [StorageTest] not exists
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var a = 10;
                }
            }

            return host;
        }

        private static string GetSqlDataBaseInitScript()
        {
            var result = string.Empty;

            try
            {
                var path = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    DatabaseInitFilename
                );

                result = File.ReadAllText(path);
            }
            catch {}

            return result;
        }
    }
}
