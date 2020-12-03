namespace MAS.DapperStorageTest.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.DapperStorageTest.Models;
    using MAS.DapperStorageTest.Infrastructure.Models;

    public static class EntityExtensions
    {
        public static WrappedEntity Wrap(this Entity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var typeName = entity.GetType().Name;

            if (typeName == nameof(Entity))
            {
                throw new ArgumentException("Type exception");
            }

            var defaultProperties = GetEntityDefaultProperties();

            var entityProperties =
                entity.GetType()
                    .GetProperties()
                    .Where(property => defaultProperties.Contains(property.Name))
                    .Select(property => new
                    {
                        Name = property.Name,
                        Value = property.GetValue(entity)
                    })
                    .ToDictionary(x => x.Name, y => y.Value);

            return new WrappedEntity(entity, typeName, entityProperties);
        }

        private static IEnumerable<string> GetEntityDefaultProperties()
        {
            return typeof(Entity).GetProperties().Select(property => property.Name);
        }
    }
}
