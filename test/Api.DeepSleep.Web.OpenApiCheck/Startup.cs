namespace Api.DeepSleep.Web.OpenApiCheck
{
    using global::DeepSleep.OpenApi;
    using global::DeepSleep.Configuration;
    using global::DeepSleep.Web;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Api.DeepSleep.Web.OpenApiCheck.Controllers;

    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>Initializes a new instance of the <see cref="Startup" /> class.</summary>
        /// <param name="env">The env.</param>
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>Gets the configuration.</summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>Configures the services.</summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ContextEndpoints>();
            services.Configure<IISServerOptions>((o) =>
            {
                o.AllowSynchronousIO = true;
            });

            services
                .AddLogging()
                .UseDeepSleepJsonNegotiation()
                .UseDeepSleepOpenApi((o) =>
                {
                    o.V2RouteTemplate = "openapi/v2/doc";
                    o.V3RouteTemplate = "openapi/v3/doc";
                    o.XmlDocumentationFileNames.Add("Api.DeepSleep.Web.OpenApiCheck.xml");
                    o.IgnoreObsoleteEndpoints = true;
                    o.IgnoreObsoleteProperties = true;
                    o.RouteFilter = (r) => true;
                })
                .UseDeepSleepDataAnnotationValidations()
                .UseDeepSleepServices((o) =>
                {
                    o.DefaultRequestConfiguration = DefaultRequestConfiguration();
                    o.WriteConsoleHeader = false;
                    o.ExcludePaths = new[]
                    {
                        @".*\.html"
                    };
                });
        }

        /// <summary>Configures the specified application.</summary>
        /// <param name="app">The application.</param>
        public void Configure(IApplicationBuilder app)
        {
            app
                .UseDeepSleep()
                .UseForwardedHeaders()
                .UseStaticFiles();

            app.Build();
        }

        /// <summary>Defaults the request configuration.</summary>
        /// <returns></returns>
        public static IDeepSleepRequestConfiguration DefaultRequestConfiguration()
        {
            return new DeepSleepRequestConfiguration
            {
                AllowAnonymous = true
            };
        }
    }
}
