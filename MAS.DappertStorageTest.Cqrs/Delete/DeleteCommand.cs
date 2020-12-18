namespace MAS.DappertStorageTest.Cqrs
{
    using System;

    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    public class DeleteCommand: ICommand
    {
        public Guid? EntityId { get; }

        public string EntityName { get; }

        public QueryFilterGroup FilterGroup { get; }

        public int RowsAffected { get; set; }

        public DeleteCommand(string entityName, Guid entityId)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            EntityId = entityId;
        }

        public DeleteCommand(string entityName, QueryFilterGroup filterGroup)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            FilterGroup = filterGroup ?? throw new ArgumentNullException(nameof(filterGroup));
        }
    }
}
