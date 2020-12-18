namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Dynamic;
    using System.Linq;

    using Dapper;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class InsertCommandHandler : BaseCommandHandler<InsertCommand>
    {
        public InsertCommandHandler(IDbConnectionFactory dbConnectionFactory, IFilterBuilder filterBuilder)
            : base(dbConnectionFactory, filterBuilder)
        {
        }

        public override void Handle(InsertCommand command)
        {
            EnsureEntityNameIsValid(command.EntityName);

            var fields = command.PropertyValues.Where(pair => !DefaultEntityFields.Contains(pair.Key));
            var fieldNames = string.Join(", ", fields.Select(pair => $"[{pair.Key}]"));
            var fieldValueBindings = string.Join(", ", fields.Select(pair => $"@NewEntity{pair.Key}"));

            dynamic arguments = new ExpandoObject();
            arguments.NewEntityId = Guid.NewGuid();
            arguments.NewEntityCreatedAt = DateTime.UtcNow;

            foreach (var field in fields)
            {
                arguments.TryAdd($"NewEntity{field.Key}", field.Value);
            }

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                var sqlQuery = BuildQuery($"INSERT INTO [{command.EntityName}] ([Id], [CreatedOn], {fieldNames}) VALUES (@NewEntityId, @NewEntityCreatedAt, {fieldValueBindings})");
                connection.Execute(sqlQuery, (object)arguments);
            }

            command.EntityId = arguments.NewEntityId;
        }
    }
}
