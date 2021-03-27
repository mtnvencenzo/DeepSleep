namespace Samples.Simple.Api
{
    using DeepSleep.Auth;
    using DeepSleep.Web;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .UseDeepSleepJsonNegotiation()
                .UseDeepSleepServices((o) =>
                {
                    o.DefaultRequestConfiguration.AuthorizationProviders = new List<IAuthorizationComponent>
                    {
                        new ApiAuthorizationComponent<AdminRoleAuthorizationProvider>()
                    };
                    o.DefaultRequestConfiguration.CrossOriginConfig.AllowCredentials = true;
                    o.DefaultRequestConfiguration.CrossOriginConfig.AllowedHeaders = new[] { "Content-Type", "Authorization", "X-CustomHeader" };
                    o.DefaultRequestConfiguration.CrossOriginConfig.AllowedOrigins = new[] { "https://localhost:5001" };
                    o.DefaultRequestConfiguration.CrossOriginConfig.ExposeHeaders = new[] { "X-CustomHeader" };
                    o.DefaultRequestConfiguration.CrossOriginConfig.MaxAgeSeconds = 600;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeepSleep();
        }
    }
}
