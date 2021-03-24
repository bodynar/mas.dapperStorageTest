namespace MAS.DapperStorageTest.Configuration
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Dapper;

    using MAS.DapperStorageTest.Infrastructure;

    public class DapperDbAdapter : IDbAdapter
    {
        public int Execute(IDbConnection connection, string sqlQuery, object arguments)
        {
            return connection.Execute(sqlQuery, arguments);
        }

        public IEnumerable<IDictionary<string, object>> Query(IDbConnection connection, string sqlQuery, object arguments)
        {
            return connection.Query(sqlQuery, arguments).Select(x => x as IDictionary<string, object>);
        }
    }
}
