namespace MAS.DappertStorageTest.Cqrs.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.Cqrs;
    using MAS.DapperStorageTest.Models;

    public class BaseCqrsHandler
    {
        #region Private fields

        private Lazy<IQueryProcessor> _queryProcessor
            => new Lazy<IQueryProcessor>(Resolver.Resolve<IQueryProcessor>);

        private Lazy<ICommandProcessor> _commandProcessor
            => new Lazy<ICommandProcessor>(Resolver.Resolve<ICommandProcessor>);
        
        private IEnumerable<string> DeclaredEntities { get; }

        #endregion

        protected IResolver Resolver { get; }

        protected IQueryProcessor QueryProcessor
            => _queryProcessor.Value;

        protected ICommandProcessor CommandProcessor
            => _commandProcessor.Value;

        protected string ConnectionString { get; }

        public BaseCqrsHandler(IResolver resolver)
        {
            Resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));

            ConnectionString = Resolver.Resolve<IConnectionData>()?.ConnectionString ?? throw new ConfigurationException(nameof(IConnectionData));
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
