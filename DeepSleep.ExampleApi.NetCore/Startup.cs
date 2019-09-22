using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DeepSleep;
using DeepSleep.NetCore;
using DeepSleep.Example.Controllers;
using DeepSleep.Validation;

namespace DeepSleep.ExampleApi.NetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>Initializes a new instance of the <see cref="Startup"/> class.</summary>
        /// <param name="env">The env.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
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
            var config = new DefaultApiServiceConfiguration
            {
                RoutingTableResolver = GetRoutes,
                ExceptionHandler = ExceptionHandler,
                ApiValidationProvider = (s, p) =>
                {
                    return p
                        .ClearInvokers()
                        .RegisterInvoker<TypeBasedValidationInvoker>()
                        .RegisterInvoker<DataAnnotationsValidationInvoker>();
                }
            };

            services.AddScoped<HelloWorldController>();
            services.AddScoped<BodyNotNullValidator>();
            services.AddScoped<TestValidator>();

            services.UseApiCoreServices(config);
        }

        /// <summary>Configures the specified application.</summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseApiCoreHttp();
            app.UseForwardedHeaders();
        }

        /// <summary>Gets the exception handler.</summary>
        /// <value>The exception handler.</value>
        public Func<ApiRequestContext, Exception, Task<long>> ExceptionHandler
        {
            get
            {
                return new Func<ApiRequestContext, Exception, Task<long>>((c, e) =>
                {
                    Console.WriteLine(e.ToString());

                    var source = new TaskCompletionSource<long>();
                    source.SetResult(0);
                    return source.Task;
                });
            }
        }

        /// <summary>Gets the routes.</summary>
        /// <returns></returns>
        private IApiRoutingTable GetRoutes(IServiceProvider provider, IApiRoutingTable @default)
        {
            

            var table = new DefaultApiRoutingTable();


            ApiRouteReflectionLocator loader = new ApiRouteReflectionLocator(AppContext.BaseDirectory, "**.dll", System.IO.SearchOption.TopDirectoryOnly);
            var routes = loader.FindApiRoutes();

            foreach (var route in routes)
            {
                var controllerType = Type.GetType(route.ContrallerType);

                table.AddRoute(
                    template: route.Route,
                    httpMethod: route.HttpMethod.ToUpper(),
                    endpoint: route.MethodName,
                    name: $"{route.HttpMethod.ToUpper()}_{route.Route}",
                    controller: controllerType,
                    config: new ApiResourceConfig
                    {
                        AllowAnonymous = true,
                        CacheDirective = new HttpCacheDirective
                        {
                            Cacheability = HttpCacheType.NoCache,
                            CacheLocation = HttpCacheLocation.Private,
                            ExpirationSeconds = -5
                        },
                        Deprecated = false,
                    }).ConfigureAwait(false).GetAwaiter();
            }


            table.AddRoute(
                template: "api/helloworld",
                httpMethod: "GET",
                endpoint: "Get",
                name: "GET_api/helloworld",
                controller: typeof(HelloWorldController),
                config: new ApiResourceConfig
                {
                    AllowAnonymous = true,
                    CacheDirective = new HttpCacheDirective
                    {
                        Cacheability = HttpCacheType.NoCache,
                        CacheLocation = HttpCacheLocation.Private,
                        ExpirationSeconds = -5
                    },
                    Deprecated = false,
                    ResourceId = "83E5516A-DBD0-42B6-8EC1-6249952D7967",
                    SupportedLanguages = new string[]
                    {
                        "de-DE"
                    }
                }).ConfigureAwait(false).GetAwaiter();



            table.AddRoute(
                template: "api/helloworld/{from}",
                httpMethod: "POST",
                endpoint: "Post",
                name: "POST_api/helloworld/{from}",
                controller: typeof(HelloWorldController),
                config: new ApiResourceConfig
                {
                    AllowAnonymous = true,
                    CacheDirective = new HttpCacheDirective
                    {
                        Cacheability = HttpCacheType.NoCache,
                        CacheLocation = HttpCacheLocation.Private,
                        ExpirationSeconds = -5
                    },
                    Deprecated = true,
                    ResourceId = "3419D96D-37B5-4E21-B11B-528B8C2C1C66"
                }).ConfigureAwait(false).GetAwaiter();


            table.AddRoute(
                template: "api/helloworld/{from}",
                httpMethod: "PUT",
                endpoint: "PutSomething",
                name: "PUT_api/helloworld/{from}",
                controller: typeof(HelloWorldController),
                config: new ApiResourceConfig
                {
                    AllowAnonymous = true,
                    CacheDirective = new HttpCacheDirective
                    {
                        Cacheability = HttpCacheType.NoCache,
                        CacheLocation = HttpCacheLocation.Private,
                        ExpirationSeconds = -5
                    },
                    Deprecated = true,
                    ResourceId = "FA9263B4-AD48-442A-9B21-145CF2A1E1D9"
                }).ConfigureAwait(false).GetAwaiter();


            table.AddRoute(
                template: "api/helloworld",
                httpMethod: "DELETE",
                endpoint: "Delete",
                name: "DELETE_api/helloworld",
                controller: typeof(HelloWorldController),
                config: new ApiResourceConfig
                {
                    AllowAnonymous = true,
                    CacheDirective = new HttpCacheDirective
                    {
                        Cacheability = HttpCacheType.NoCache,
                        CacheLocation = HttpCacheLocation.Private,
                        ExpirationSeconds = -5
                    },
                    Deprecated = false,
                    ResourceId = "D5834885-3777-4196-9A63-0A9D27E4D3BC"
                }).ConfigureAwait(false).GetAwaiter();


            table.AddRoute(
                template: "api/helloworld/hello",
                httpMethod: "PATCH",
                endpoint: "DoThePatch",
                name: "PATCH_api/helloworld/hello",
                controller: typeof(HelloWorldPatchController),
                config: new ApiResourceConfig
                {
                    AllowAnonymous = true,
                    CacheDirective = new HttpCacheDirective
                    {
                        Cacheability = HttpCacheType.NoCache,
                        CacheLocation = HttpCacheLocation.Private,
                        ExpirationSeconds = -5
                    },
                    Deprecated = false,
                    ResourceId = "6A7B88BC-8F26-4687-BE5A-29FCE17938E2"
                }).ConfigureAwait(false).GetAwaiter();

            return table;
        }
    }
}
