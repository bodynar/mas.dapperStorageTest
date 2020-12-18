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
        public string Select([FromBody] SelectRequest selectRequest)
        {
            EnsureNotNull(selectRequest, nameof(selectRequest));

            var query = new SelectQuery(selectRequest.EntityName, selectRequest.Fields, selectRequest.EntityId);
            var result = QueryProcessor.Execute(query);

            return result.ToString();
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
