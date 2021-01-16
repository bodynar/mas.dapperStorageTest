namespace MAS.DapperStorageTest.Controllers
{
    using System;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure.Cqrs;
    using MAS.DapperStorageTest.Models;
    using MAS.DappertStorageTest.Cqrs;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json"), Consumes("application/json")]
    public class DataController : ControllerBase
    {
        public ICommandProcessor CommandProcessor { get; }

        public IQueryProcessor QueryProcessor { get; }

        public DataController(ICommandProcessor commandProcessor, IQueryProcessor queryProcessor)
        {
            CommandProcessor = commandProcessor;
            QueryProcessor = queryProcessor;
        }

        [HttpGet]
        public string Index()
            => nameof(DataController);

        [HttpPost("[action]")]
        public InsertResponse Insert([FromBody] InsertRequest insertRequest)
        {
            EnsureNotNull(insertRequest, nameof(insertRequest));

            var command = new InsertCommand(insertRequest.EntityName, insertRequest.Values);
            CommandProcessor.Execute(command);

            return new InsertResponse(command.EntityId, command.Warnings);
        }

        [HttpGet("[action]")]
        public SelectResponse Select([FromBody] SelectRequest selectRequest)
        {
            EnsureNotNull(selectRequest, nameof(selectRequest));

            var result = QueryProcessor.Execute(
                new SelectQuery(
                    selectRequest.EntityName, selectRequest.Columns, selectRequest.Filters,
                    selectRequest.OrderingColumns.Select(x => (OrderOption)x),
                    selectRequest.Count, selectRequest.Offset
                )
            );

            return new SelectResponse(result.Entities, result.EntityName, result.Count, result.Offset, result.Columns, result.Warnings);
        }

        [HttpPost("[action]")]
        public DeleteResponse Delete([FromBody] DeleteRequest deleteRequest)
        {
            EnsureNotNull(deleteRequest, nameof(deleteRequest));

            var command =
                deleteRequest.EntityId.HasValue
                ? new DeleteCommand(deleteRequest.EntityName, deleteRequest.EntityId.Value)
                : new DeleteCommand(deleteRequest.EntityName, deleteRequest.Filters);
            CommandProcessor.Execute(command);

            return new DeleteResponse(command.RowsAffected);
        }

        [HttpPost("[action]")]
        [HttpPatch("[action]")]
        public UpdateResponse Update([FromBody] UpdateRequest updateRequest)
        {
            EnsureNotNull(updateRequest, nameof(updateRequest));

            var command = new UpdateCommand(updateRequest.EntityName, updateRequest.Values, updateRequest.Filters);
            CommandProcessor.Execute(command);

            return new UpdateResponse(command.RowsAffected, command.Warnings);
        }

        private static void EnsureNotNull<TValue>(TValue value, string paramName)
            where TValue : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
