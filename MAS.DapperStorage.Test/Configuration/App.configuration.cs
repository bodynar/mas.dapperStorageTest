namespace MAS.DapperStorageTest.Configuration
{
    using MAS.DapperStorageTest.Middleware;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;

    using SimpleInjector;

    public static class AppConfiguration
    {
        public static void Configure(this IApplicationBuilder app, Container container, IConfiguration configuration, bool isDevelopment)
        {
            if (isDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseMiddleware<ExceptionHandlerMiddleware>()
                .UseHttpsRedirection()
                .UseSimpleInjector(container, options => { })
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "api",
                        pattern: "api/{controller}/{action}/{id?}"
                    );
                });

            container
                .Configure(configuration)
                .Verify();
        }
    }
}
