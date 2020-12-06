namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;

    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    public class InsertCommand : ICommand
    {
        public string EntityName { get; }

        public IDictionary<string, object> PropertyValues { get; }

        public InsertCommand(string entityName, IDictionary<string, object> propertyValues)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            PropertyValues = propertyValues ?? throw new ArgumentNullException(nameof(propertyValues));
        }
    }
}
