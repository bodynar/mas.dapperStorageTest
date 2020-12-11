namespace MAS.DapperStorageTest.Configuration
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

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
                            var dbInitScriptParts =
                                Regex.Split(sqlDatabaseInitScript, "END[^;]*;")
                                    .Select((x, y) => y == 0 ? $"{x} END;" : x)
                                    .ToList();

                            if (dbInitScriptParts.Count == 2)
                            {
                                using (var connection = dbConnectionFactory.CreateDbConnection())
                                {
                                    foreach (var sqlScript in dbInitScriptParts)
                                    {
                                        using (var command = connection.CreateCommand())
                                        {
                                            command.CommandText = sqlScript;

                                            if (connection.State != System.Data.ConnectionState.Open)
                                            {
                                                connection.Open();
                                            }

                                            if (connection.State == System.Data.ConnectionState.Open)
                                            {
                                                command.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // TODO: add logger here
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
            catch { }

            return result;
        }
    }
}
