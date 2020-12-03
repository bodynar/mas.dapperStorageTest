namespace MAS.DappertStorageTest.Cqrs
{
    using System;

    using MAS.DapperStorageTest.Infrastructure.Cqrs;
    using MAS.DapperStorageTest.Infrastructure.Models;

    public class SelectQuery: IQuery<WrappedEntity>
    {
        public Guid Id { get; }

        public string EntityName { get; }

        public SelectQuery(Guid id, string entityName)
        {
            Id = id;
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
        }
    }
}
