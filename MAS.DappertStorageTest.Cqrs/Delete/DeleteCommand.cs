namespace MAS.DappertStorageTest.Cqrs
{
    using System;

    using MAS.DapperStorageTest.Infrastructure.Cqrs;

    public class DeleteCommand: ICommand
    {
        public Guid EntityId { get; }

        public string EntityName { get; }

        public DeleteCommand(Guid entityId, string entityName)
        {
            EntityId = entityId;
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
        }
    }
}
