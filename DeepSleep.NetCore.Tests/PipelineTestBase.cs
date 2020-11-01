namespace DeepSleep.NetCore.Tests
{
    using DeepSleep.Configuration;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    public abstract class PipelineTestBase
    {
        protected readonly IServiceProvider serviceProvider;
        protected readonly IApiServiceConfiguration serviceConfiguration;
        protected readonly string host = "www.deepsleep.ut";

        public PipelineTestBase()
        {
            this.serviceConfiguration = new DefaultApiServiceConfiguration();
            var services = new ServiceCollection();
            services.UseApiCoreServices(this.serviceConfiguration);

            this.serviceProvider = services.BuildServiceProvider();
        }

        protected async Task<ApiRequestContext> Invoke(HttpContext httpContext)
        {
            var contextPipeline = new ApiRequestContextPipelineComponent(null);
            var contextResolver = this.serviceProvider.GetService<IApiRequestContextResolver>();
            var requestPipeline = this.serviceProvider.GetService<IApiRequestPipeline>();

            await contextPipeline.Invoke(httpContext, contextResolver, requestPipeline, this.serviceConfiguration, null);
            return contextResolver.GetContext();
        }
    }
}
