namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Media;
    using DeepSleep.Media.Serializers;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpRequestAcceptTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var processed = await context.ProcessHttpRequestAccept(contextResolver, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRequestInfoIsNull()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = null
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var processed = await context.ProcessHttpRequestAccept(contextResolver, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsEmptyAcceptAllowWhenFormatterFactoryIsNull()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(TestController),
                            methodInfo: typeof(TestController).GetMethod(nameof(TestController.Get)),
                            httpMethod: "GET")
                    }
                },
                Request = new ApiRequestInfo()
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var processed = await context.ProcessHttpRequestAccept(contextResolver, null).ConfigureAwait(false);
            processed.Should().BeFalse();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(406);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("X-Allow-Accept");
            context.Response.Headers[0].Value.Should().Be(string.Empty);

        }

        [Fact]
        public async void ReturnsAllFormatterTypesAcceptAllowWhenFormatterNotFound()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(TestController),
                            methodInfo: typeof(TestController).GetMethod(nameof(TestController.Get)),
                            httpMethod: "GET")
                    }
                },
                Request = new ApiRequestInfo()
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var mockFormatterFactory = new Mock<IDeepSleepMediaSerializerFactory>();
            mockFormatterFactory
                .Setup(m => m.GetWriteableTypes(It.IsAny<Type>(), It.IsAny<IList<IDeepSleepMediaSerializer>>()))
                .Returns(new string[] { "application/json", "text/xml", "text/plain" });

            var processed = await context.ProcessHttpRequestAccept(contextResolver, mockFormatterFactory.Object).ConfigureAwait(false);
            processed.Should().BeFalse();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(406);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("X-Allow-Accept");
            context.Response.Headers[0].Value.Should().Be("application/json, text/xml, text/plain");

        }

        [Fact]
        public async void ReturnsTrueForFoundFormatter()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(TestController),
                            methodInfo: typeof(TestController).GetMethod(nameof(TestController.Get)),
                            httpMethod: "GET")
                    }
                },
                Request = new ApiRequestInfo()
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            string formatterType;
            var mockFormatter = new Mock<IDeepSleepMediaSerializer>();

            var mockFormatterFactory = new Mock<IDeepSleepMediaSerializerFactory>();
            mockFormatterFactory
                .Setup(m => m.GetAcceptableFormatter(It.IsAny<AcceptHeader>(), It.IsAny<Type>(), out formatterType, It.IsAny<IList<IDeepSleepMediaSerializer>>(), It.IsAny<IList<string>>()))
                .Returns(Task.FromResult(mockFormatter.Object));

            var processed = await context.ProcessHttpRequestAccept(contextResolver, mockFormatterFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(0);
        }

        [Theory]
        [InlineData("application/json")]
        [InlineData("application/*")]
        [InlineData("*/*")]
        public async void ReturnsTrueForFoundImplmentedFormatter(string requestAccept)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(TestController),
                            methodInfo: typeof(TestController).GetMethod(nameof(TestController.Get)),
                            httpMethod: "GET")
                    }
                },
                Request = new ApiRequestInfo
                {
                    Accept = new AcceptHeader(requestAccept)
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var formatter = SetupJsonFormatterMock(null, new string[] { "application/json" });
            var mockFactory = SetupFormatterFactory(formatter.Object);


            var processed = await context.ProcessHttpRequestAccept(contextResolver, mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(0);
        }

        private Mock<DeepSleepMediaSerializerWriterFactory> SetupFormatterFactory(params IDeepSleepMediaSerializer[] formatters)
        {
            var mockFactory = new Mock<DeepSleepMediaSerializerWriterFactory>(new object[] { null })
            {
                CallBase = true
            };

            mockFactory.Setup(m => m.GetFormatters())
                .Returns(new List<IDeepSleepMediaSerializer>(formatters));

            return mockFactory;
        }

        private Mock<DeepSleepJsonMediaSerializer> SetupJsonFormatterMock(string[] readableTypes, string[] writeableTypes)
        {
            var mockFormatter = new Mock<DeepSleepJsonMediaSerializer>(new object[] { null })
            {
                CallBase = true
            };
            mockFormatter.Setup(m => m.ReadableMediaTypes).Returns(readableTypes);
            mockFormatter.Setup(m => m.WriteableMediaTypes).Returns(writeableTypes);
            return mockFormatter;
        }
    }
}
