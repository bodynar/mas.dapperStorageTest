namespace MAS.DappertStorageTest.Cqrs.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Models;

    public class BaseCqrsHandler
    {
        private IEnumerable<string> DeclaredEntities { get; }

        protected IDbConnectionFactory DbConnectionFactory { get; }

        public BaseCqrsHandler(IDbConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory = dbConnectionFactory;
            DeclaredEntities = GetDeclaredEntities();
        }

        protected bool IsValidEntityName(string entityName)
        {
            return DeclaredEntities.Contains(entityName);
        }

        protected string GetTableName(string entityName)
            => $"{entityName}s";

        private IEnumerable<string> GetDeclaredEntities()
        {
            return typeof(EntityMarkerAttribute).Assembly
                .GetTypes()
                .Where(type => type.GetCustomAttributes(typeof(EntityMarkerAttribute), false).Any())
                .Select(x => x.Name);
        }
    }
}
