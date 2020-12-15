namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;

    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    public class DeleteCommand: ICommand
    {
        public Guid? EntityId { get; }

        public string EntityName { get; }

        public IEnumerable<QueryFilter> Filters { get; }

        public int RowsAffected { get; set; }

        public DeleteCommand(string entityName, Guid entityId)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            EntityId = entityId;
        }

        public DeleteCommand(string entityName, IEnumerable<QueryFilter> filters)
        {
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
            Filters = filters ?? throw new ArgumentNullException(nameof(filters));
        }
    }
}
