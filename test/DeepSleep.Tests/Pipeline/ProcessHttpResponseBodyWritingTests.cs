namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Media;
    using DeepSleep.Media.Serializers;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Moq;
    using System;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpResponseBodyWritingTests
    {
        [Fact]
        public async void ReturnsTrueForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true),
                Response = null
            };

            var processed = await context.ProcessHttpResponseBodyWriting(new ApiRequestContextResolver(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteForNullResponseObject()
        {
            var mockRequestInfo = new Mock<ApiRequestInfo>();
            mockRequestInfo.Setup(m => m.Accept).Throws(new Exception("test"));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    ResponseObject = null
                },
                Request = mockRequestInfo.Object
            };

            var processed = await context.ProcessHttpResponseBodyWriting(new ApiRequestContextResolver(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(204);
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteForResponseObjectBody()
        {
            var mockRequestInfo = new Mock<ApiRequestInfo>();
            mockRequestInfo.Setup(m => m.Accept).Throws(new Exception("test"));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    ResponseObject = null
                },
                Request = mockRequestInfo.Object
            };

            var processed = await context.ProcessHttpResponseBodyWriting(new ApiRequestContextResolver(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(204);
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteForNullFormatterFactory()
        {
            var mockRequestInfo = new Mock<ApiRequestInfo>();
            mockRequestInfo.Setup(m => m.Accept).Throws(new Exception("test"));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    ResponseObject = "test"
                },
                Request = mockRequestInfo.Object
            };

            var processed = await context.ProcessHttpResponseBodyWriting(new ApiRequestContextResolver(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(204);
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteForNullFormatter()
        {
            var mockFactory = SetupFormatterFactory(new IDeepSleepMediaSerializer[] { });

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    ResponseObject = "test"
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(new ApiRequestContextResolver(), mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(204);
            context.Response.ContentLength.Should().Be(0);
        }

        [Theory]
        [InlineData("application/xml")]
        [InlineData("text/json")]
        [InlineData("application/pdf")]
        [InlineData("text/plain")]
        public async void ReturnsTrueAndDoesNotWriteForNonMatchingFormatter(string accept)
        {
            var formatter = SetupJsonFormatterMock(null, new string[] { "application/json" });
            var mockFactory = SetupFormatterFactory(formatter.Object);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    StatusCode = 200,
                    ResponseObject = "test"
                },
                Request = new ApiRequestInfo
                {
                    Accept = accept
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(new ApiRequestContextResolver(), mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(204);
            context.Response.ContentLength.Should().Be(0);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void ReturnsTrueAndDoesWritesUsingDefaultFormatterWhenMissingRequestAccept(string accept)
        {
            var formatter = SetupJsonFormatterMock(null, new string[] { "application/json" });
            var mockFactory = SetupFormatterFactory(formatter.Object);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                Request = new ApiRequestInfo
                {
                    Accept = accept
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var processed = await context.ProcessHttpResponseBodyWriting(contextResolver, mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(201);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(0);
            context.Response.ContentType.Should().NotBeNull();
            context.Response.ContentType.Should().Be("application/json");
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesWithUnMatchedEtagAndIfModifiedSince()
        {
            var formatter = SetupJsonFormatterMock(null, new string[] { "application/json", "text/json" });
            var mockFactory = SetupFormatterFactory(formatter.Object);
            var etag = "TEST-IF-MATCH";
            DateTimeOffset lastModifed = DateTimeOffset.UtcNow;


            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                Request = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfMatch = etag + "_FAIL",
                    IfModifiedSince = lastModifed.AddSeconds(1)
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            context.Response.Headers.Add(new ApiHeader("ETag", etag));
            context.Response.Headers.Add(new ApiHeader("Last-Modified", lastModifed.ToString("r")));

            var processed = await context.ProcessHttpResponseBodyWriting(contextResolver, mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(201);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.ContentType.Should().NotBeNull();
            context.Response.ContentType.Should().Be("application/json");
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesWithUnMatchedEtagButMatchedIfModifiedSince()
        {
            var formatter = SetupJsonFormatterMock(null, new string[] { "application/json", "text/json" });
            var mockFactory = SetupFormatterFactory(formatter.Object);
            var etag = "TEST-IF-MATCH";
            DateTimeOffset lastModifed = DateTimeOffset.UtcNow;


            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                Request = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfMatch = etag + "_FAIL",
                    IfModifiedSince = lastModifed
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            context.Response.Headers.Add(new ApiHeader("ETag", etag));
            context.Response.Headers.Add(new ApiHeader("Last-Modified", lastModifed.ToString("r")));

            var processed = await context.ProcessHttpResponseBodyWriting(contextResolver, mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(201);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.ContentType.Should().NotBeNull();
            context.Response.ContentType.Should().Be("application/json");
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesWithMatchedEtagButUnMatchedIfModifiedSince()
        {
            var formatter = SetupJsonFormatterMock(null, new string[] { "application/json", "text/json" });
            var mockFactory = SetupFormatterFactory(formatter.Object);
            var etag = "TEST-IF-MATCH";
            DateTimeOffset lastModifed = DateTimeOffset.UtcNow;


            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                Request = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfMatch = etag,
                    IfModifiedSince = lastModifed.AddSeconds(1)
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            context.Response.Headers.Add(new ApiHeader("ETag", etag));
            context.Response.Headers.Add(new ApiHeader("Last-Modified", lastModifed.ToString("r")));

            var processed = await context.ProcessHttpResponseBodyWriting(contextResolver, mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(201);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.ContentType.Should().NotBeNull();
            context.Response.ContentType.Should().Be("application/json");
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesWithUnMatchedEtag()
        {
            var formatter = SetupJsonFormatterMock(null, new string[] { "application/json", "text/json" });
            var mockFactory = SetupFormatterFactory(formatter.Object);
            var etag = "TEST-IF-MATCH";


            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                Request = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfMatch = etag + "_FAIL"
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            context.Response.Headers.Add(new ApiHeader("ETag", etag));

            var processed = await context.ProcessHttpResponseBodyWriting(contextResolver, mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(201);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.ContentType.Should().NotBeNull();
            context.Response.ContentType.Should().Be("application/json");
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesWithUnMatchedIfModifiedSince()
        {
            var formatter = SetupJsonFormatterMock(null, new string[] { "application/json", "text/json" });
            var mockFactory = SetupFormatterFactory(formatter.Object);
            DateTimeOffset lastModifed = DateTimeOffset.UtcNow;


            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                Request = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfModifiedSince = lastModifed.AddSeconds(1)
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            context.Response.Headers.Add(new ApiHeader("Last-Modified", lastModifed.ToString("r")));

            var processed = await context.ProcessHttpResponseBodyWriting(contextResolver, mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(201);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.ContentType.Should().NotBeNull();
            context.Response.ContentType.Should().Be("application/json");
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesUsingMatchedFormatter()
        {
            var formatter = SetupJsonFormatterMock(null, new string[] { "application/json" });
            var mockFactory = SetupFormatterFactory(formatter.Object);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                Request = new ApiRequestInfo
                {
                    Accept = "application/json"
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(new ApiRequestContextResolver(), mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(201);
            context.Response.Headers.Should().NotBeNull();
            context.Response.ContentType.Should().NotBeNull();
            context.Response.ContentType.Should().Be("application/json");
            context.Response.ContentLength.Should().Be(0);
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
