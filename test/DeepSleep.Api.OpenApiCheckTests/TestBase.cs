namespace DeepSleep.Api.OpenApiCheckTests
{
    using global::Api.DeepSleep.Web.OpenApiCheck;
    using DeepSleep.Api.OpenApiCheckTests.Mocks;
    using DeepSleep.Web;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using DeepSleep.Pipeline;
    using DeepSleep.Configuration;
    using Moq;
    using Microsoft.AspNetCore.Hosting;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.FileProviders;

    public abstract class TestBase
    {
        protected Startup startup;
        protected IServiceProvider serviceProvider;
        protected HttpContext httpContext;
        protected readonly string host = "ut-test.com";
        protected readonly string applicationJson = "application/json";
        protected readonly string applicationYaml = "application/yaml";

        protected async Task<HttpResponseMessage> GetResponseMessageTask(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using var httpContext = new MockHttpContext(this.serviceProvider, request);
            await Invoke(httpContext).ConfigureAwait(false);

            var response = new HttpResponseMessage((HttpStatusCode)httpContext.Response.StatusCode);

            if (httpContext.Response.Body != null)
            {
                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

                var ms = new MemoryStream();
                httpContext.Response.Body.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                ms.Position = 0;

                httpContext.Response.RegisterForDispose(ms);

                response.Content = new StreamContent(ms);
            }

            if (httpContext.Response.Headers != null)
            {
                foreach (var header in httpContext.Response.Headers)
                {
                    if (header.Key == "Expires" || header.Key == "Content-Type" || header.Key == "Content-Length")
                    {
                        response.Content.Headers.Add(header.Key, header.Value.ToArray());
                    }
                    else
                    {
                        response.Headers.Add(header.Key, header.Value.ToArray());
                    }
                }
            }



            return response;
        }

        protected async Task<ApiRequestContext> Invoke(HttpContext httpContext)
        {
            var contextPipeline = new ApiRequestContextPipelineComponent(null);
            var contextResolver = this.serviceProvider.GetService<IApiRequestContextResolver>();
            var requestPipeline = this.serviceProvider.GetService<IApiRequestPipeline>();
            var serviceConfiguration = this.serviceProvider.GetService<IDeepSleepServiceConfiguration>();

            await contextPipeline.Invoke(httpContext, contextResolver, requestPipeline, serviceConfiguration);
            this.httpContext = httpContext;

            return contextResolver.GetContext();
        }

        protected void SetupEnvironment(Action<IServiceCollection> servicePreprocessor = null)
        {
            var services = new ServiceCollection();
            var basePath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Startup)).Location);

            var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            hostingEnvironmentMock.Setup(m => m.EnvironmentName).Returns("Development");
            hostingEnvironmentMock.Setup(m => m.ContentRootPath).Returns(basePath);

            hostingEnvironmentMock.Setup(m => m.ContentRootFileProvider).Returns(new PhysicalFileProvider("/"));
            hostingEnvironmentMock.Setup(m => m.WebRootFileProvider).Returns(new PhysicalFileProvider("/"));

            services.AddSingleton<IWebHostEnvironment>(hostingEnvironmentMock.Object);

            startup = new Startup(hostingEnvironmentMock.Object);
            startup.ConfigureServices(services);

            this.serviceProvider = services.BuildServiceProvider();
            startup.Configure(new ApplicationBuilder(this.serviceProvider));
        }

        protected Task<string> GetResponseDataString(HttpResponse response)
        {
            using (var reader = new StreamReader(response.Body))
            {
                response.Body.Seek(0, SeekOrigin.Begin);
                var data = reader.ReadToEnd();
                return Task.FromResult(data);
            }
        }
    }
}
