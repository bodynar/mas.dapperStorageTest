namespace MAS.DapperStorageTest
{
    using MAS.DapperStorageTest.Configuration;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using SimpleInjector;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Container Container { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Container = new Container();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure(Configuration, Container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Configure(Container, Configuration, env.IsDevelopment());
        }
    }
}
