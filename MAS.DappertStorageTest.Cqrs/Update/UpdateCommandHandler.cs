namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dapper;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class UpdateCommandHandler : BaseCommandHandler<UpdateCommand>
    {
        public UpdateCommandHandler(IDbConnectionFactory dbConnectionFactory, IFilterBuilder filterBuilder)
            : base(dbConnectionFactory, filterBuilder)
        {
        }

        public override void Handle(UpdateCommand command)
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

            var (whereSqlStatement, arguments) = FilterBuilder.Build(command.FilterGroup);

            var setStatement =
                string.Join(", ",
                    fields.Select(field =>
                    {
                        var argumentKey = $"UpdateEntity{field.Key}";
                        arguments.TryAdd(argumentKey, field.Value);

                        return $"[{field.Key}] = @{argumentKey}";
                    })
                );

            var updateDate = DateTime.UtcNow;

            arguments.TryAdd("UpdateEntityModifiedOn", updateDate);
            setStatement += $", [ModifiedOn] = @UpdateEntityModifiedOn";

            var sqlQuery = BuildQuery($"UPDATE [{command.EntityName}] SET {setStatement} WHERE {whereSqlStatement}");
            var result = 0;

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                result = connection.Execute(sqlQuery, arguments);
            }

            command.RowsAffected = result;
        }
    }
}
