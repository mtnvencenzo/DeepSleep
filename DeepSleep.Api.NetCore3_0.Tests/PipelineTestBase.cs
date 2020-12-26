namespace DeepSleep.Api.NetCore.Tests
{
    using DeepSleep;
    using DeepSleep.Configuration;
    using DeepSleep.NetCore;
    using global::Api.DeepSleep.NetCore3_0;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    public abstract class PipelineTestBase : TestBase
    {
        private Startup startup;

        protected void SetupEnvironment(Action<IServiceCollection> servicePreprocessor = null)
        {
            var services = new ServiceCollection();
            var basePath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Startup)).Location);

            var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            hostingEnvironmentMock.Setup(m => m.EnvironmentName).Returns("Development");
            hostingEnvironmentMock.Setup(m => m.ContentRootPath).Returns(basePath);

            startup = new Startup(hostingEnvironmentMock.Object)
            {
                ServicePreprocessor = servicePreprocessor
            };

            startup.ConfigureServices(services);
            startup.Configure(new ApplicationBuilder(startup.ServiceProvider));
        }

        public IServiceProvider ServiceProvider => startup?.ServiceProvider;

        protected async Task<ApiRequestContext> Invoke(HttpContext httpContext)
        {
            var contextPipeline = new ApiRequestContextPipelineComponent(null);
            var contextResolver = this.ServiceProvider.GetService<IApiRequestContextResolver>();
            var requestPipeline = this.ServiceProvider.GetService<IApiRequestPipeline>();
            var serviceConfiguration = this.ServiceProvider.GetService<IApiServiceConfiguration>();

            await contextPipeline.Invoke(httpContext, contextResolver, requestPipeline, serviceConfiguration);
            return contextResolver.GetContext();
        }
    }
}
