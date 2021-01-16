﻿namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
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
            EnsureFieldsAreValidForEntity(command.EntityName, command.PropertyValues.Select(x => x.Key));

            var fields = command.PropertyValues.Where(pair => !DefaultEntityFields.Contains(pair.Key));

            if (fields.Count() != command.PropertyValues.Count)
            {
                var notValidKeys = command.PropertyValues.Where(pair => DefaultEntityFields.Contains(pair.Key)).Select(x => x.Key);

                command.Warnings.Add($"Cannot set value for default columns: [{string.Join(", ", notValidKeys)}]");
            }

            var fieldNames = string.Join(", ", fields.Select(pair => $"[{pair.Key}]"));
            var fieldValueBindings = string.Join(", ", fields.Select(pair => $"@NewEntity{pair.Key}"));

            var entityId = Guid.NewGuid();
            var arguments = new ExpandoObject();
            arguments.TryAdd("NewEntityId", entityId);
            arguments.TryAdd("NewEntityCreatedAt", DateTime.UtcNow);

            foreach (var field in fields)
            {
                arguments.TryAdd($"NewEntity{field.Key}", field.Value);
            }

            var sqlQuery = BuildQuery($"INSERT INTO [{command.EntityName}] ([Id], [CreatedOn], {fieldNames}) VALUES (@NewEntityId, @NewEntityCreatedAt, {fieldValueBindings})");

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                connection.Execute(sqlQuery, arguments);
            }

            command.EntityId = entityId;
        }
    }
}
