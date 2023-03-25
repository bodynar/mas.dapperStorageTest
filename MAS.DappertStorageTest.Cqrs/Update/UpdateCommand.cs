namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;

    using MAS.DapperStorageTest.Infrastructure.FilterBuilder;

    public class UpdateCommand : BaseCommand
    {
        public string EntityName { get; }

        // TODO IDictionary<string, object>
        public IDictionary<string, string> PropertyValues { get; }

        public FilterGroup FilterGroup { get; }

        public int RowsAffected { get; set; }

        public UpdateCommand(string entityName, IDictionary<string, string> propertyValues, FilterGroup filterGroup)
            : base()
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            PropertyValues = propertyValues ?? throw new ArgumentNullException(nameof(propertyValues));
            FilterGroup = filterGroup ?? throw new ArgumentNullException(nameof(filterGroup));
        }
    }
}
