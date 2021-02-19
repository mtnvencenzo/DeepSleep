namespace DeepSleep.Api.Web.Tests
{
    using DeepSleep;
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using DeepSleep.Web;
    using global::Api.DeepSleep.Web5_0;
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
        protected readonly int uriBindingErrorStatusCode = 404;
        protected readonly int bodyBindingErrorStatusCode = 422;
        protected IServiceProvider serviceProvider;
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

            this.serviceProvider = services.BuildServiceProvider();

            startup.Configure(new ApplicationBuilder(this.serviceProvider));
        }

        public IServiceProvider ServiceProvider => this.serviceProvider;

        protected async Task<ApiRequestContext> Invoke(HttpContext httpContext)
        {
            var contextPipeline = new ApiRequestContextPipelineComponent(null);
            var contextResolver = this.ServiceProvider.GetService<IApiRequestContextResolver>();
            var requestPipeline = this.ServiceProvider.GetService<IApiRequestPipeline>();
            var serviceConfiguration = this.ServiceProvider.GetService<IDeepSleepServiceConfiguration>();

            await contextPipeline.Invoke(httpContext, contextResolver, requestPipeline, serviceConfiguration);
            return contextResolver.GetContext();
        }
    }
}
