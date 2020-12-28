namespace MAS.DapperStorageTest.Controllers
{
    using System;

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
        public Guid Insert([FromBody] InsertRequest insertRequest)
        {
            EnsureNotNull(insertRequest, nameof(insertRequest));

            var command = new InsertCommand(insertRequest.EntityName, insertRequest.Values);
            CommandProcessor.Execute(command);

            return command.EntityId;
        }

        [HttpGet("[action]")]
        public SelectResponse Select([FromBody] SelectRequest selectRequest)
        {
            EnsureNotNull(selectRequest, nameof(selectRequest));

            var result = QueryProcessor.Execute(
                new SelectQuery(selectRequest.EntityName, selectRequest.Columns, selectRequest.Filters, selectRequest.OrderingColumns, selectRequest.Count, selectRequest.Offset)
            );

            return new SelectResponse(result.Entities, result.EntityName, result.Count, result.Offset, result.Columns);
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
