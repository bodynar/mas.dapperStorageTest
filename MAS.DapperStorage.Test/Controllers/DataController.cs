namespace MAS.DapperStorageTest.Controllers
{
    using System;
    using System.Collections.Generic;

    using MAS.DapperStorageTest.Infrastructure;
    using MAS.DapperStorageTest.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class DataController : ControllerBase
    {
        private ILogger<DataController> Logger { get; }

        public IResolver Resolver { get; }

        public DataController(
            ILogger<DataController> logger,
            IResolver resolver
        )
        {
            Logger = logger;
            Resolver = resolver;
        }

        [HttpGet]
        public string Get()
            => "Hello world";

        [HttpGet("[action]")]
        public IEnumerable<object> Entities([FromQuery] string entityName, int? skip = null, int? count = null)
        {
            Logger.LogDebug($"{nameof(Entities)} called. Params: \"{entityName}\".");

            EnsureNotNull(entityName, nameof(entityName));

            if (skip.HasValue || count.HasValue)
            {
                var isDefinedBoth = skip.HasValue && count.HasValue;

                if (!isDefinedBoth)
                {
                    throw new ArgumentException("");
                }
            }

            IEnumerable<object> result = null;

            Logger.LogDebug($"{nameof(Entities)} evaluated. Params: \"{entityName}\". Result: \"{string.Join(" ,", result)}\"");

            return result;
        }

        [HttpPost("[action]")]
        public Guid Add([FromBody]AddEntityModel addEntityModel)
        {
            return Guid.Empty;
        }

        private static void EnsureNotNull<TValue>(TValue value, string paramName)
            where TValue : class
        {
            if (value == null || value == default)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
