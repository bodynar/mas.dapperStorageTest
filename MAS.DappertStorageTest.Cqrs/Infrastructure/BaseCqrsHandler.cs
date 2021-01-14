namespace MAS.DappertStorageTest.Cqrs.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Models;

    public abstract class BaseCqrsHandler
    {
        private static IEnumerable<string> DeclaredEntities { get; set; }

        protected static IEnumerable<string> DefaultEntityFields { get; private set; }

        protected IDbConnectionFactory DbConnectionFactory { get; }

        protected IFilterBuilder FilterBuilder { get; }

        static BaseCqrsHandler()
        {
            InitDatabaseModelsStaticData();
        }

        public BaseCqrsHandler(IDbConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        public BaseCqrsHandler(IDbConnectionFactory dbConnectionFactory, IFilterBuilder filterBuilder)
            : this(dbConnectionFactory)
        {
            FilterBuilder = filterBuilder ?? throw new ArgumentNullException(nameof(filterBuilder));
        }

        #region Protected members

        protected IEnumerable<string> GetNotValidFieldsForEntity(string entityName, IEnumerable<string> fieldNames)
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
            return $"USE [{DbConnectionFactory.DatabaseName}];{Environment.NewLine}{queryPart}";
        }

        protected (string, ExpandoObject) BuildWhereFilter(string entityName, QueryFilterGroup filter)
        {
            var filterFieldNames = GetFilterFiledNames(filter);

            EnsureFieldsAreValidForEntity(entityName, filterFieldNames);

            return FilterBuilder.Build(filter);
        }

        #endregion

        #region Not public API

        private static void InitDatabaseModelsStaticData()
        {
            DefaultEntityFields = typeof(Entity).GetProperties().Select(x => x.Name);

            /*
             * Так как Entity и все наследники, представленные в бд
             * в данном проекте (и последующих) не используются по факту (кроме данной проверки),
             * то их необходимость обусловлена только необходимостью валидировать некоторые члены cqrs запросов\команд.
             * По аналогии с метаданными Terrasoft Creatio.
             */

            DeclaredEntities = typeof(EntityMarkerAttribute).Assembly
                .GetTypes()
                .Where(type => type.GetCustomAttributes(typeof(EntityMarkerAttribute), false).Any())
                .Select(x => x.Name)
                .ToList();
        }

        private bool IsValidEntityName(string entityName)
        {
            return DeclaredEntities.Contains(entityName);
        }

        private IEnumerable<string> GetFilterFiledNames(QueryFilterGroup queryFilter)
        {
            var fieldNames = new List<string>();

            if (queryFilter.InnerGroups.Any())
            {
                foreach (var filterItem in queryFilter.InnerGroups)
                {
                    var innerFieldNames = GetFilterFiledNames(filterItem);
                    fieldNames.AddRange(innerFieldNames);
                }
            }
            else
            {
                fieldNames.AddRange(queryFilter.Filters.Select(x => x.FieldName));
            }

            return fieldNames.Distinct().OrderBy(x => x);
        }

        #endregion
    }
}
