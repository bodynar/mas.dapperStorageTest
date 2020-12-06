namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Linq;

    using Dapper;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class InsertCommandHandler : BaseCommandHandler<InsertCommand>
    {
        public InsertCommandHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        public override void Handle(InsertCommand command)
        {
            EnsureEntityNameIsValid(command.EntityName);

            var fieldNames = string.Join(", ", command.PropertyValues.Keys.Select(key => $"[{key}]"));
            var fieldValues = string.Join(", ", command.PropertyValues.Select(value => value));
            var id = Guid.NewGuid();

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                var sqlQuery = $"INSERT INTO [{command.EntityName}] ([Id], {fieldNames}) VALUES (@NewEntityId, {fieldValues})";
                connection.Execute(sqlQuery, new { NewEntityId = id });
            }
        }
    }
}
