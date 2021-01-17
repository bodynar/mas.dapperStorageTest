namespace MAS.DapperStorageTest.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;

    // todo: find a better name
    public interface IDbAdapter
    {
        IEnumerable<IDictionary<string, object>> Query(IDbConnection connection, string sqlQuery, object arguments);

        int Execute(IDbConnection connection, string sqlQuery, object arguments);
    }
}
