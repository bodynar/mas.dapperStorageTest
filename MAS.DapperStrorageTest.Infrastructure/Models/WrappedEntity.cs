namespace MAS.DapperStorageTest.Infrastructure.Models
{
    using System;
    using System.Collections.Generic;

    using MAS.DapperStorageTest.Models;

    public class WrappedEntity : Entity
    {
        public IDictionary<string, object> PropertyValues { get; private set; }

        public string TypeName { get; private set; }

        public WrappedEntity(Entity entity, string typeName, IDictionary<string, object> propertyValues)
        {
            Id = entity?.Id ?? throw new ArgumentNullException(nameof(entity));
            CreatedOn = entity.CreatedOn;
            ModifiedOn = entity.ModifiedOn;

            TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            PropertyValues = propertyValues ?? throw new ArgumentNullException(nameof(propertyValues));
        }
    }
}
