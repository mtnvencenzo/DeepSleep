namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Formatting;
    using DeepSleep.Formatting.Formatters;
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

            var processed = await context.ProcessHttpResponseBodyWriting(new DefaultApiRequestContextResolver(), null).ConfigureAwait(false);
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

            var processed = await context.ProcessHttpResponseBodyWriting(new DefaultApiRequestContextResolver(), null).ConfigureAwait(false);
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

            var processed = await context.ProcessHttpResponseBodyWriting(new DefaultApiRequestContextResolver(), null).ConfigureAwait(false);
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

            var processed = await context.ProcessHttpResponseBodyWriting(new DefaultApiRequestContextResolver(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(204);
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteForNullFormatter()
        {
            var mockFactory = SetupFormatterFactory(new IFormatStreamReaderWriter[] { });

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Response = new ApiResponseInfo
                {
                    ResponseObject = "test"
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(new DefaultApiRequestContextResolver(), mockFactory.Object).ConfigureAwait(false);
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

            var processed = await context.ProcessHttpResponseBodyWriting(new DefaultApiRequestContextResolver(), mockFactory.Object).ConfigureAwait(false);
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

            var contextResolver = new DefaultApiRequestContextResolver();
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

        [Theory]
        [InlineData("application/json", "application/json")]
        [InlineData("text/json", "text/json")]
        [InlineData("*/*", "application/json")]
        [InlineData(null, "application/json")]
        public async void ReturnsTrueAndDoesWritesUsingMatchedFormatter(string accept, string expectedContentType)
        {
            var formatter = SetupJsonFormatterMock(null, new string[] { "application/json", "text/json" });
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

            var processed = await context.ProcessHttpResponseBodyWriting(new DefaultApiRequestContextResolver(), mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(201);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(0);
            context.Response.ContentType.Should().NotBeNull();
            context.Response.ContentType.Should().Be(expectedContentType);
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteAndModifiedResponseTo304NotModifiedWhenIfMatchAndIfModifedSinceMatch()
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
                    IfModifiedSince = lastModifed
                }
            };

            var contextResolver = new DefaultApiRequestContextResolver();
            contextResolver.SetContext(context);

            context.Response.Headers.Add(new ApiHeader("ETag", etag));
            context.Response.Headers.Add(new ApiHeader("Last-Modified", lastModifed.ToString("r")));

            var processed = await context.ProcessHttpResponseBodyWriting(contextResolver, mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(304);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.ContentType.Should().BeNull();
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteAndModifiedResponseTo304NotModifiedWhenIfMatch()
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
                    IfMatch = etag
                }
            };

            var contextResolver = new DefaultApiRequestContextResolver();
            contextResolver.SetContext(context);

            context.Response.Headers.Add(new ApiHeader("ETag", etag));

            var processed = await context.ProcessHttpResponseBodyWriting(contextResolver, mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(304);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.ContentType.Should().BeNull();
            context.Response.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteAndModifiedResponseTo304NotModifiedWhenIfModifedSinceMatch()
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
                    IfModifiedSince = lastModifed
                }
            };

            var contextResolver = new DefaultApiRequestContextResolver();
            contextResolver.SetContext(context);

            context.Response.Headers.Add(new ApiHeader("Last-Modified", lastModifed.ToString("r")));

            var processed = await context.ProcessHttpResponseBodyWriting(contextResolver, mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(304);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.ContentType.Should().BeNull();
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

            var contextResolver = new DefaultApiRequestContextResolver();
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

            var contextResolver = new DefaultApiRequestContextResolver();
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

            var contextResolver = new DefaultApiRequestContextResolver();
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

            var contextResolver = new DefaultApiRequestContextResolver();
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

            var contextResolver = new DefaultApiRequestContextResolver();
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
        public async void ReturnsTrueAndDoesWritesUsingMatchedFormatterAndPrettyPrint()
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
                    Accept = "application/json",
                    PrettyPrint = true
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(new DefaultApiRequestContextResolver(), mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(201);
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("X-PrettyPrint");
            context.Response.Headers[0].Value.Should().Be("true");
            context.Response.ContentType.Should().NotBeNull();
            context.Response.ContentType.Should().Be("application/json");
            context.Response.ContentLength.Should().Be(0);
        }

        private Mock<HttpMediaTypeStreamReaderWriterFactory> SetupFormatterFactory(params IFormatStreamReaderWriter[] formatters)
        {
            var mockFactory = new Mock<HttpMediaTypeStreamReaderWriterFactory>(new object[] { null })
            {
                CallBase = true
            };

            mockFactory.Setup(m => m.GetFormatters())
                .Returns(new List<IFormatStreamReaderWriter>(formatters));

            return mockFactory;
        }

        private Mock<JsonHttpFormatter> SetupJsonFormatterMock(string[] readableTypes, string[] writeableTypes)
        {
            var mockFormatter = new Mock<JsonHttpFormatter>(new object[] { null })
            {
                CallBase = true
            };
            mockFormatter.Setup(m => m.ReadableMediaTypes).Returns(readableTypes);
            mockFormatter.Setup(m => m.WriteableMediaTypes).Returns(writeableTypes);
            return mockFormatter;
        }
    }
}
