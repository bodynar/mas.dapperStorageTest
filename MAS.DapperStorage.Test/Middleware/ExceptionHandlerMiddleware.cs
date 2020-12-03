namespace MAS.DapperStorageTest.Middleware
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    using Newtonsoft.Json;

    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate nextHandler;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            nextHandler = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await nextHandler(httpContext).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e).ConfigureAwait(false);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var serializedException = JsonConvert.SerializeObject(new
            {
                Success = false,
                Message = exception.Message,
            });

            return context.Response.WriteAsync(serializedException);
        }
    }
}
