namespace MAS.DappertStorageTest.Cqrs
{
    using System;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Infrastructure.FilterBuilder;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class UpdateCommandHandler : BaseCommandHandler<UpdateCommand>
    {
        public UpdateCommandHandler(IDbConnectionFactory dbConnectionFactory, IDbAdapter dbAdapter, IFilterBuilder filterBuilder)
            : base(dbConnectionFactory, dbAdapter, filterBuilder)
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

            var (whereSqlStatement, arguments) = FilterBuilder.Build(command.FilterGroup);

            var commandArgs = arguments.ToDictionary(x => x.Key, x => x.Value);

            var setStatement =
                string.Join(", ",
                    fields.Select(field =>
                    {
                        var argumentKey = $"UpdateEntity{field.Key}";
                        commandArgs.TryAdd(argumentKey, field.Value);

                        return $"[{field.Key}] = @{argumentKey}";
                    })
                );

            var updateDate = DateTime.UtcNow;

            commandArgs.Add("UpdateEntityModifiedOn", updateDate);
            setStatement += $", [ModifiedOn] = @UpdateEntityModifiedOn";

            var sqlQuery = BuildQuery($"UPDATE [{command.EntityName}] SET {setStatement} WHERE {whereSqlStatement}");
            var result = 0;

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                result = DbAdapter.Execute(connection, sqlQuery, commandArgs);
            }

            command.RowsAffected = result;
        }
    }
}
