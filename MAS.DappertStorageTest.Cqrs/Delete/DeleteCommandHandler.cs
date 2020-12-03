namespace MAS.DappertStorageTest.Cqrs
{
    using Dapper;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DappertStorageTest.Cqrs.Infrastructure;

    using Microsoft.Data.SqlClient;

    public class DeleteCommandHandler : BaseCommandHandler<DeleteCommand>
    {
        public DeleteCommandHandler(IResolver resolver)
            : base(resolver)
        {
        }

        public override void Proceed(DeleteCommand command)
        {
            var tableName = GetTableName(command.EntityName);

            using (var connection = new SqlConnection(ConnectionString))
            {
                var sqlQuery = $"DELETE FROM {tableName} WHERE Id = @id";
                connection.Execute(sqlQuery, new { command.EntityId });
            }
        }
    }
}
