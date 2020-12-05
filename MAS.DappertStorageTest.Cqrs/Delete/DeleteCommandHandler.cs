namespace MAS.DappertStorageTest.Cqrs
{
    using Dapper;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    public class DeleteCommandHandler : BaseCommandHandler<DeleteCommand>
    {
        public DeleteCommandHandler(IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory)
        {
        }

        public override void Handle(DeleteCommand command)
        {
            var tableName = GetTableName(command.EntityName);

            using (var connection = DbConnectionFactory.CreateDbConnection())
            {
                var sqlQuery = $"DELETE FROM {tableName} WHERE Id = @id";
                connection.Execute(sqlQuery, new { command.EntityId });
            }
        }
    }
}
