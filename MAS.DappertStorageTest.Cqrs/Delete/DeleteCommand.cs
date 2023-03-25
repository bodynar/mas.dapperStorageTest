namespace MAS.DappertStorageTest.Cqrs
{
    using System;

    using MAS.DapperStorageTest.Infrastructure.FilterBuilder;

    public class DeleteCommand: BaseCommand
    {
        public Guid? EntityId { get; }

        public string EntityName { get; }

        public FilterGroup FilterGroup { get; }

        public int RowsAffected { get; set; }

        public DeleteCommand(string entityName, Guid entityId)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            EntityId = entityId;
        }

        public DeleteCommand(string entityName, FilterGroup filterGroup)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            FilterGroup = filterGroup ?? throw new ArgumentNullException(nameof(filterGroup));
        }
    }
}
