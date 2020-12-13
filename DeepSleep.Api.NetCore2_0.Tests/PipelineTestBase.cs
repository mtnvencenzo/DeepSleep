namespace DeepSleep.Api.NetCore.Tests
{
    using DeepSleep;
    using DeepSleep.Configuration;
    using DeepSleep.NetCore;
    using FluentAssertions;
    using global::Api.DeepSleep.NetCore2_0;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web;
    using System.Xml;
    using System.Xml.Serialization;

    public abstract class PipelineTestBase
    {
        protected readonly string host = "ut-test.com";
        protected readonly string applicationJson = "application/json";
        protected readonly string textJson = "text/json";
        protected readonly string applicationWwwFormUrlEncoded = "application/x-www-form-urlencoded";
        protected readonly string applicationXml = "application/xml";
        protected readonly string textXml = "text/xml";
        protected readonly string applicationJsonPatch = "application/json-patch+json";
        protected readonly string cacheControlNoCache = "no-cache, no-store, must-revalidate, max-age=0";
        protected readonly string authSchemeToken = "Token";
        protected readonly string authSchemeBearer = "Bearer";
        protected readonly string staticToken = "T0RrMlJqWXpNVFF0UmtReFF5MDBRamN5TFVJeE5qZ3RPVGxGTlRBek5URXdNVUkz";
        private Startup startup;

        protected void SetupEnvironment(Action<IServiceCollection> servicePreprocessor)
        {
            var services = new ServiceCollection();
            var basePath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Startup)).Location);

            var hostingEnvironmentMock = new Mock<IHostingEnvironment>();
            hostingEnvironmentMock.Setup(m => m.EnvironmentName).Returns("Development");
            hostingEnvironmentMock.Setup(m => m.ContentRootPath).Returns(basePath);

            startup = new Startup(hostingEnvironmentMock.Object)
            {
                ServicePreprocessor = servicePreprocessor
            };

            startup.ConfigureServices(services);
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

        protected void AssertResponse(
            ApiRequestContext apiContext,
            HttpResponse response,
            int expectedHttpStatus = 200,
            bool shouldHaveResponse = true,
            string expectedContentType = null,
            ApiValidationState expectedValidationState = ApiValidationState.Succeeded,
            Dictionary<string, string> extendedHeaders = null,
            bool shouldBeCancelledReuqest = false,
            string expectedProtocol = "HTTP/1.1",
            bool? expectedAuthenticationResult = null)
        {
            apiContext.Should().NotBeNull();
            response.Should().NotBeNull();

            // Check for extended messages and exceptions on expected
            // success responses.  This helps in debuging failing tests
            if (expectedHttpStatus >= 200 && expectedHttpStatus < 300)
            {
                apiContext.ProcessingInfo.Exceptions.ForEach(ex =>
                {
                    ex.ToString().Should().BeNull();
                });

                apiContext.ErrorMessages.ForEach(m =>
                {
                    m.Should().BeNull();
                });
            }

            if (shouldHaveResponse)
            {
                apiContext.ResponseInfo.ResponseObject.Should().NotBeNull();
            }
            else
            {
                apiContext.ResponseInfo.ResponseObject.Should().BeNull();
            }

            apiContext.ResponseInfo.StatusCode.Should().Be(expectedHttpStatus);
            apiContext.ValidationState().Should().Be(expectedValidationState);
            apiContext.PathBase.Should().Be(apiContext.RequestInfo.Path);
            response.StatusCode.Should().Be(expectedHttpStatus);

            var expectedHeaderCount = (response.StatusCode >= 500) 
                ? 3 + (extendedHeaders?.Count ?? 0) + (shouldHaveResponse ? 0 : -1)
                : 5 + (extendedHeaders?.Count ?? 0) + (shouldHaveResponse ? 0 : -1);

            // Check for headers
            apiContext.ResponseInfo.Headers.Should().HaveCount(expectedHeaderCount);
            response.Headers.Should().HaveCount(expectedHeaderCount);

            response.Headers.Should().ContainKey("Date");
            response.Headers["Date"].Should().Equal(apiContext.ResponseInfo.Date?.ToString("r"));
            apiContext.ResponseInfo.Headers.HasHeader("Date").Should().BeTrue();
            apiContext.ResponseInfo.Headers.GetHeader("Date").Value.Should().Be(apiContext.ResponseInfo.Date?.ToString("r"));

            response.Headers.Should().ContainKey("Content-Length");
            response.Headers["Content-Length"].Should().Equal(response.Body.Length.ToString());
            apiContext.ResponseInfo.Headers.HasHeader("Content-Length").Should().BeTrue();
            apiContext.ResponseInfo.Headers.GetHeader("Content-Length").Value.Should().Be(response.Body.Length.ToString());
            apiContext.ResponseInfo.ContentLength.Should().Be(response.Body.Length);

            if (response.StatusCode < 500)
            {
                response.Headers.Should().ContainKey("Cache-Control");
                response.Headers["Cache-Control"].Should().Equal(this.cacheControlNoCache);
                apiContext.ResponseInfo.Headers.HasHeader("Cache-Control").Should().BeTrue();
                apiContext.ResponseInfo.Headers.GetHeader("Cache-Control").Value.Should().Be(this.cacheControlNoCache);

                response.Headers.Should().ContainKey("Expires");
                response.Headers["Expires"].Should().Equal(apiContext.ResponseInfo.Date?.AddYears(-1).ToString("r"));
                apiContext.ResponseInfo.Headers.HasHeader("Expires").Should().BeTrue();
                apiContext.ResponseInfo.Headers.GetHeader("Expires").Value.Should().Be(apiContext.ResponseInfo.Date?.AddYears(-1).ToString("r"));
            }

            if (shouldHaveResponse)
            {
                response.Headers.Should().ContainKey("Content-Type");
                response.Headers["Content-Type"].Should().Equal(expectedContentType ?? this.applicationJson);
                apiContext.ResponseInfo.Headers.HasHeader("Content-Type").Should().BeTrue();
                apiContext.ResponseInfo.Headers.GetHeader("Content-Type").Value.Should().Be(expectedContentType ?? this.applicationJson);
                apiContext.ResponseInfo.ContentType.Should().Be(expectedContentType);
            }

            if (extendedHeaders != null)
            {
                foreach (var header in extendedHeaders)
                {
                    response.Headers.Should().ContainKey(header.Key);
                    response.Headers[header.Key].Should().Equal(header.Value);

                    apiContext.ResponseInfo.Headers.HasHeader(header.Key).Should().BeTrue();
                    apiContext.ResponseInfo.Headers.GetHeader(header.Key).Value.Should().Be(header.Value);
                }
            }

            // -------------------------------------
            // Misicalleanous Api Context Validation
            apiContext.ProcessingInfo.UTCRequestDuration.Should().NotBeNull();
            apiContext.ProcessingInfo.UTCRequestDuration.Duration.Should().Be(
                (int)(apiContext.ProcessingInfo.UTCRequestDuration.EndDate - apiContext.ProcessingInfo.UTCRequestDuration.StartDate).TotalMilliseconds);

            apiContext.RequestAborted.IsCancellationRequested.Should().Be(shouldBeCancelledReuqest);
            apiContext.RequestInfo.Protocol.Should().Be(expectedProtocol);
            apiContext.RequestInfo.RequestIdentifier.Should().NotBeNull();

            if (response.StatusCode < 500)
            {
                apiContext.ResponseInfo.ResponseWriter.Should().NotBeNull();
                apiContext.ResponseInfo.ResponseWriterOptions.Should().NotBeNull();

                if (apiContext.RequestInfo.PrettyPrint && apiContext.ResponseInfo.ResponseWriter.SupportsPrettyPrint)
                {
                    apiContext.ResponseInfo.ResponseWriterOptions.PrettyPrint.Should().BeTrue();
                }
                else
                {
                    apiContext.ResponseInfo.ResponseWriterOptions.PrettyPrint.Should().BeFalse();
                }
            }

            if (expectedAuthenticationResult.HasValue)
            {
                apiContext.RequestInfo.ClientAuthenticationInfo.Should().NotBeNull();
                apiContext.RequestInfo.ClientAuthenticationInfo.AuthResult.Should().NotBeNull();
                apiContext.RequestInfo.ClientAuthenticationInfo.AuthResult.IsAuthenticated.Should().Be(expectedAuthenticationResult.Value);
            }
        }

        protected Task<T> GetResponseData<T>(HttpResponse response)
        {
            var contentType = response.Headers["Content-Type"][0] ?? string.Empty;

            if (contentType == this.applicationJson || contentType == this.textJson)
            {
                using var reader = new StreamReader(response.Body);
                response.Body.Seek(0, SeekOrigin.Begin);
                var data = reader.ReadToEnd();
                var obj = JsonConvert.DeserializeObject<T>(data);
                return Task.FromResult(obj);
            }
            else if (contentType == this.applicationXml || contentType == this.textXml)
            {
                object obj;
                var serializer = new XmlSerializer(typeof(T));
                var settings = new XmlReaderSettings
                {
                    CloseInput = false,
                    ConformanceLevel = ConformanceLevel.Fragment,
                    IgnoreComments = true,
                    ValidationType = ValidationType.None
                };

                response.Body.Seek(0, SeekOrigin.Begin);

                using XmlReader reader = XmlReader.Create(response.Body, settings);
                obj = serializer.Deserialize(reader);

                return obj != null
                    ? Task.FromResult((T)obj)
                    : Task.FromResult(default(T));
            }

            throw new Exception($"Content-Type {contentType} is not supported.");
        }

        protected static string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value);
        }
    }
}
