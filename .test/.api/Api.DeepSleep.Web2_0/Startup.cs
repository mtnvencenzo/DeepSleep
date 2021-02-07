namespace Api.DeepSleep.Web2_0
{
    using Api.DeepSleep.Controllers;
    using global::DeepSleep.Web;
    using global::DeepSleep.Validation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::DeepSleep;
    using global::DeepSleep.Configuration;

    public class Startup
    {
        /// <summary>Initializes a new instance of the <see cref="Startup" /> class.</summary>
        /// <param name="env">The env.</param>
        public Startup(IHostingEnvironment env)
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
            this.ConfigureDependentServices(services);
        }

        /// <summary>Configures the dependent services.</summary>
        /// <param name="services">The services.</param>
        public void ConfigureDependentServices(IServiceCollection services)
        {
            ServiceStartup.RegisterServices(services);

            services
                .AddLogging()
                .UseDeepSleepPingEndpoint("ping")
                .UseDeepSleepJsonNegotiation()
                .UseDeepSleepXmlNegotiation()
                .UseDeepSleepMultipartFormDataNegotiation()
                .UseDeepSleepFormUrlEncodedNegotiation()
                .UseDeepSleepOpenApi((o) =>
                {
                    o.V2RouteTemplate = "openapi/v2/doc";
                    o.V3RouteTemplate = "openapi/v3/doc";
                    o.XmlDocumentationFileNames.Add("Api.DeepSleep.Models.xml");
                    o.XmlDocumentationFileNames.Add("Api.DeepSleep.Controllers.xml");
                    o.XmlDocumentationFileNames.Add("deepsleep.xml");
                    o.XmlDocumentationFileNames.Add("deepsleep.web.xml");
                })
                .UseDeepSleepDataAnnotationValidations()
                .UseDeepSleepServices((o) =>
                {
                    o.DiscoveryStrategies = ServiceStartup.DiscoveryStrategies();
                    o.DefaultRequestConfiguration = ServiceStartup.DefaultRequestConfiguration();
                    o.WriteConsoleHeader = false;
                    o.OnException = (ctx, ex) =>
                    {
                        var context = ctx.GetContext();
                        if (context.Items.ContainsKey("exceptionHandlerCount"))
                        {
                            var count = (int)context.Items["exceptionHandlerCount"];
                            context.Items["exceptionHandlerCount"] = count++;
                        }
                        else
                        {
                            context.Items["exceptionHandlerCount"] = 1;
                        }

                        return Task.CompletedTask;
                    };
                    o.OnRequestProcessed = (ctx) =>
                    {
                        var context = ctx.GetContext();
                        if (context.Items.ContainsKey("requestHandlerCount"))
                        {
                            var count = (int)context.Items["requestHandlerCount"];
                            context.Items["requestHandlerCount"] = count++;
                        }
                        else
                        {
                            context.Items["requestHandlerCount"] = 1;
                        }

                        return Task.CompletedTask;
                    };
                });

            if (ServicePreprocessor != null)
            {
                ServicePreprocessor(services);
            }
        }

        /// <summary>Configures the specified application.</summary>
        /// <param name="app">The application.</param>
        public void Configure(IApplicationBuilder app)
        {
            app
                .UseDeepSleep()
                .UseForwardedHeaders();
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<IServiceCollection> ServicePreprocessor { get; set; }
    }
}
