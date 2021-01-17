namespace MAS.DappertStorageTest.Cqrs
{
    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class DeleteCommandHandler : BaseCommandHandler<DeleteCommand>
    {
        public DeleteCommandHandler(IDbConnectionFactory dbConnectionFactory, IDbAdapter dbAdapter, IFilterBuilder filterBuilder)
            : base(dbConnectionFactory, dbAdapter, filterBuilder)
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

        #region Not public API

        private int DeleteById(DeleteCommand command)
        {
            var sqlQuery = BuildQuery($"DELETE FROM [{command.EntityName}] WHERE Id = @Id");
            var result = 0;

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                result = DbAdapter.Execute(connection, sqlQuery, new { Id = command.EntityId });
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
                result = DbAdapter.Execute(connection, sqlQuery, arguments);
            }

            return result;
        }

        #endregion
    }
}
