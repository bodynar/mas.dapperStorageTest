namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;

    public class InsertCommand : BaseCommand
    {
        public string EntityName { get; }

        public IDictionary<string, object> PropertyValues { get; }

        public Guid EntityId { get; set; }

        public InsertCommand(string entityName, IDictionary<string, object> propertyValues)
            : base()
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            PropertyValues = propertyValues ?? throw new ArgumentNullException(nameof(propertyValues));
        }
    }
}
