namespace MAS.DappertStorageTest.Cqrs
{
    using Dapper;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class DeleteCommandHandler : BaseCommandHandler<DeleteCommand>
    {
        public DeleteCommandHandler(IDbConnectionFactory dbConnectionFactory, IFilterBuilder filterBuilder)
            : base(dbConnectionFactory, filterBuilder)
        {
        }

        public override void Handle(DeleteCommand command)
        {
            EnsureEntityNameIsValid(command.EntityName);

            var rowsAffected =
                command.EntityId.HasValue
                    ? DeleteById(command)
                    : DeleteByFilters(command);

            command.RowsAffected = rowsAffected;
        }

        private int DeleteById(DeleteCommand command)
        {
            var sqlQuery = BuildQuery($"DELETE FROM [{command.EntityName}] WHERE Id = @Id");
            var result = 0;

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                connection.Execute(sqlQuery, new { Id = command.EntityId });
            }

            return result;
        }

        private int DeleteByFilters(DeleteCommand command)
        {
            var (whereCondition, arguments) = BuildWhereFilter(command.EntityName, command.FilterGroup);
            var sqlQuery = BuildQuery($"DELETE FROM [{command.EntityName}] WHERE {whereCondition}");
            var result = 0;

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                connection.Execute(sqlQuery, arguments);
            }

            return result;
        }
    }
}
