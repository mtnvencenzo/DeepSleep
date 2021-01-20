namespace Api.DeepSleep.Web.OpenApiCheck
{
    using global::DeepSleep.Validation;
    using global::DeepSleep.Web;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName?.ToLower()}.json", optional: true)
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
            services
                .UseOpenApiServices(
                    info: null,
                    v2RouteTemplate: "openapi/v2/doc",
                    v3RouteTemplate: "openapi/v3/doc",
                    prefixNamesWithNamespace: false,
                    includeHeadOperationsForGets: true,
                    xmlDocumentationFileNames: new List<string>
                    {
                        "Api.DeepSleep.Web.OpenApiCheck.xml",
                        "deepsleep.xml",
                        "deepsleep.web.xml"
                    })
                .UseDataAnnotationValidations(continuation: ValidationContinuation.OnlyIfValid, validateAllProperties: true)
                .UseApiCoreServices();
        }

        /// <summary>Configures the specified application.</summary>
        /// <param name="app">The application.</param>
        public void Configure(IApplicationBuilder app)
        {
            app
                .UseApiCoreHttp()
                .UseForwardedHeaders();
        }
    }
}
