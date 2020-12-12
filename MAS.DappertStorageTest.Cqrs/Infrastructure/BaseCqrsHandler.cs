namespace MAS.DappertStorageTest.Cqrs.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Models;

    public class BaseCqrsHandler
    {
        private const string UseDataBaseStatement = "USE [DapperStorageTest]";

        private IEnumerable<string> DeclaredEntities { get; }

        protected static IEnumerable<string> DefaultEntityFields { get; private set; }

        protected IDbConnectionFactory DbConnectionFactory { get; }

        static BaseCqrsHandler()
        {
            InitDefaultEntityFields();
        }

        public BaseCqrsHandler(IDbConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory = dbConnectionFactory;
            DeclaredEntities = GetDeclaredEntities();
        }

        #region Protected members

        protected void EnsureFieldsAreValidForEntity(string entityName, IEnumerable<string> fieldNames)
        {
            var notValidFields = GetNotValidFieldsForEntity(entityName, fieldNames);

            if (notValidFields.Any())
            {
                throw new DatabaseOperationException($"Entity name \"{entityName}\" does not contains these fields: [{string.Join(", ", notValidFields)}].");
            }
        }

        protected void EnsureEntityNameIsValid(string entityName)
        {
            var isValidEntity = IsValidEntityName(entityName);

            if (!isValidEntity)
            {
                throw new DatabaseOperationException($"Entity name \"{entityName}\" is not valid or isn't presented in database.");
            }
        }

        protected string BuildQuery(string queryPart)
        {
            return $"{UseDataBaseStatement};{queryPart}";
        }

        protected string GetComparisonOperator(ComparisonType comparisonType)
        {
            // TODO

            return string.Empty;
        }

        #endregion

        #region Not public API

        private static void InitDefaultEntityFields()
        {
            DefaultEntityFields = typeof(Entity).GetProperties().Select(x => x.Name);
        }

        private bool IsValidEntityName(string entityName)
        {
            return DeclaredEntities.Contains(entityName);
        }

        private IEnumerable<string> GetDeclaredEntities()
        {
            return typeof(EntityMarkerAttribute).Assembly
                .GetTypes()
                .Where(type => type.GetCustomAttributes(typeof(EntityMarkerAttribute), false).Any())
                .Select(x => x.Name)
                .ToList();
        }

        private IEnumerable<string> GetNotValidFieldsForEntity(string entityName, IEnumerable<string> fieldNames)
        {
            var entity = typeof(EntityMarkerAttribute).Assembly
                .GetTypes()
                .Where(type => type.GetCustomAttributes(typeof(EntityMarkerAttribute), false).Any())
                .FirstOrDefault(type => type.Name == entityName);

            var propertyNames = entity
                .GetProperties()
                .Select(x => x.Name);

            return fieldNames.Where(fieldName => !propertyNames.Contains(fieldName));
        }

        #endregion
    }
}
