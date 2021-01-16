namespace MAS.DapperStorageTest.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using MAS.DapperStorageTest.Infrastructure;
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

        /// <summary>
        /// Get name of current service represented by controller
        /// </summary>
        /// <returns>Name of service represented by controller</returns>
        [HttpGet]
        public string Index()
            => "DataService";

        /// <summary>
        /// Inserts entity into database
        /// </summary>
        /// <exception cref="ArgumentNullException">Parameter insertRequest isn't defined</exception>
        /// <exception cref="DatabaseOperationException">Entity name isn't valid</exception>
        /// <exception cref="DatabaseOperationException">Insert configuration contains field which not exists in entity schema</exception>
        /// <param name="insertRequest">Insert command configuration</param>
        /// <returns>Result of insert operation</returns>
        [HttpPost("[action]")]
        [HttpPut("[action]")]
        public InsertResponse Insert([FromBody][Required] InsertRequest insertRequest)
        {
            EnsureNotNull(insertRequest, nameof(insertRequest));

            var command = new InsertCommand(insertRequest.EntityName, insertRequest.Values);
            CommandProcessor.Execute(command);

            return new InsertResponse(command.EntityId, command.Warnings);
        }

        /// <summary>
        /// Selects entities from database by provided configuration using filters
        /// </summary>
        /// <exception cref="ArgumentNullException">Parameter selectRequest isn't defined</exception>
        /// <exception cref="DatabaseOperationException">Entity name isn't valid</exception>
        /// <exception cref="DatabaseOperationException">FIlter contains field which not exists in entity schema</exception>
        /// <param name="selectRequest">Select query configuration</param>
        /// <returns>Result of select operation</returns>
        [HttpGet("[action]")]
        public SelectResponse Select([FromBody][Required] SelectRequest selectRequest)
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

        /// <summary>
        /// Update specified entites values using filters
        /// </summary>
        /// <exception cref="ArgumentNullException">Parameter updateRequest isn't defined</exception>
        /// <exception cref="DatabaseOperationException">Entity name isn't valid</exception>
        /// <exception cref="DatabaseOperationException">Update configuration contains field which not exists in entity schema</exception>
        /// <exception cref="DatabaseOperationException">FIlter contains field which not exists in entity schema</exception>
        /// <param name="updateRequest">Update command configuration</param>
        /// <returns>Result of update operation</returns>
        [HttpPost("[action]")]
        [HttpPatch("[action]")]
        public UpdateResponse Update([FromBody][Required] UpdateRequest updateRequest)
        {
            EnsureNotNull(updateRequest, nameof(updateRequest));

            var command = new UpdateCommand(updateRequest.EntityName, updateRequest.Values, updateRequest.Filters);
            CommandProcessor.Execute(command);

            return new UpdateResponse(command.RowsAffected, command.Warnings);
        }

        /// <summary>
        /// Delete specified entity.
        /// Entity could be specified by id or filters.
        /// </summary>
        /// <exception cref="ArgumentNullException">Parameter deleteRequest isn't defined</exception>
        /// <exception cref="DatabaseOperationException">Entity name isn't valid</exception>
        /// <exception cref="DatabaseOperationException">FIlter contains field which not exists in entity schema</exception>
        /// <param name="deleteRequest">Delete command configuration</param>
        /// <returns>Result of delete operation</returns>
        [HttpPost("[action]")]
        [HttpDelete("[action]")]
        public DeleteResponse Delete([FromBody][Required] DeleteRequest deleteRequest)
        {
            EnsureNotNull(deleteRequest, nameof(deleteRequest));

            var command =
                deleteRequest.EntityId.HasValue
                    ? new DeleteCommand(deleteRequest.EntityName, deleteRequest.EntityId.Value)
                    : new DeleteCommand(deleteRequest.EntityName, deleteRequest.Filters);
            CommandProcessor.Execute(command);

            return new DeleteResponse(command.RowsAffected);
        }

        #region Not public API

        private static void EnsureNotNull<TValue>(TValue value, string paramName)
            where TValue : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        #endregion
    }
}
