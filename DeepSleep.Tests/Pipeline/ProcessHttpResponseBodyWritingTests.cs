namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Formatting;
    using DeepSleep.Formatting.Formatters;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                ResponseInfo = null
            };

            var processed = await context.ProcessHttpResponseBodyWriting(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteForNullResponseObject()
        {
            var mockRequestInfo = new Mock<ApiRequestInfo>();
            mockRequestInfo.Setup(m => m.Accept).Throws(new Exception("test"));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    ResponseObject = null
                },
                RequestInfo = mockRequestInfo.Object
            };

            var processed = await context.ProcessHttpResponseBodyWriting(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(204);
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteForResponseObjectBody()
        {
            var mockRequestInfo = new Mock<ApiRequestInfo>();
            mockRequestInfo.Setup(m => m.Accept).Throws(new Exception("test"));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    ResponseObject = null
                },
                RequestInfo = mockRequestInfo.Object
            };

            var processed = await context.ProcessHttpResponseBodyWriting(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(204);
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteForNullFormatterFactory()
        {
            var mockRequestInfo = new Mock<ApiRequestInfo>();
            mockRequestInfo.Setup(m => m.Accept).Throws(new Exception("test"));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    ResponseObject = "test"
                },
                RequestInfo = mockRequestInfo.Object
            };

            var processed = await context.ProcessHttpResponseBodyWriting(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(204);
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteForNullFormatter()
        {
            var mockFactory = SetupFormatterFactory(new IFormatStreamReaderWriter[] { });

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    ResponseObject = "test"
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(204);
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Theory]
        [InlineData("application/xml")]
        [InlineData("text/json")]
        [InlineData("application/pdf")]
        [InlineData("text/plain")]
        public async void ReturnsTrueAndDoesNotWriteForNonMatchingFormatter(string accept)
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 200,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Accept = accept
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(204);
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void ReturnsTrueAndDoesWritesUsingDefaultFormatterWhenMissingRequestAccept(string accept)
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Accept = accept
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(201);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(0);
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be("application/json");
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Theory]
        [InlineData("application/json", "application/json")]
        [InlineData("text/json", "text/json")]
        [InlineData("*/*", "application/json")]
        [InlineData(null, "application/json")]
        public async void ReturnsTrueAndDoesWritesUsingMatchedFormatter(string accept, string expectedContentType)
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json", "text/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Accept = accept
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(201);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(0);
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be(expectedContentType);
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteAndModifiedResponseTo304NotModifiedWhenIfMatchAndIfModifedSinceMatch()
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json", "text/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);
            var etag = "TEST-IF-MATCH";
            DateTimeOffset lastModifed = DateTimeOffset.UtcNow;

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfMatch = etag,
                    IfModifiedSince = lastModifed
                }
            };

            context.ResponseInfo.Headers.Add(new ApiHeader("ETag", etag));
            context.ResponseInfo.Headers.Add(new ApiHeader("Last-Modified", lastModifed.ToString("r")));

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(304);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.ContentType.Should().BeNull();
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteAndModifiedResponseTo304NotModifiedWhenIfMatch()
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json", "text/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);
            var etag = "TEST-IF-MATCH";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfMatch = etag
                }
            };

            context.ResponseInfo.Headers.Add(new ApiHeader("ETag", etag));

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(304);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.ContentType.Should().BeNull();
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteAndModifiedResponseTo304NotModifiedWhenIfModifedSinceMatch()
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json", "text/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);
            DateTimeOffset lastModifed = DateTimeOffset.UtcNow;

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfModifiedSince = lastModifed
                }
            };

            context.ResponseInfo.Headers.Add(new ApiHeader("Last-Modified", lastModifed.ToString("r")));

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(304);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.ContentType.Should().BeNull();
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesWithUnMatchedEtagAndIfModifiedSince()
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json", "text/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);
            var etag = "TEST-IF-MATCH";
            DateTimeOffset lastModifed = DateTimeOffset.UtcNow;


            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfMatch = etag + "_FAIL",
                    IfModifiedSince = lastModifed.AddSeconds(1)
                }
            };

            context.ResponseInfo.Headers.Add(new ApiHeader("ETag", etag));
            context.ResponseInfo.Headers.Add(new ApiHeader("Last-Modified", lastModifed.ToString("r")));

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(201);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be("application/json");
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesWithUnMatchedEtagButMatchedIfModifiedSince()
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json", "text/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);
            var etag = "TEST-IF-MATCH";
            DateTimeOffset lastModifed = DateTimeOffset.UtcNow;


            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfMatch = etag + "_FAIL",
                    IfModifiedSince = lastModifed
                }
            };

            context.ResponseInfo.Headers.Add(new ApiHeader("ETag", etag));
            context.ResponseInfo.Headers.Add(new ApiHeader("Last-Modified", lastModifed.ToString("r")));

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(201);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be("application/json");
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesWithMatchedEtagButUnMatchedIfModifiedSince()
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json", "text/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);
            var etag = "TEST-IF-MATCH";
            DateTimeOffset lastModifed = DateTimeOffset.UtcNow;


            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfMatch = etag,
                    IfModifiedSince = lastModifed.AddSeconds(1)
                }
            };

            context.ResponseInfo.Headers.Add(new ApiHeader("ETag", etag));
            context.ResponseInfo.Headers.Add(new ApiHeader("Last-Modified", lastModifed.ToString("r")));

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(201);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be("application/json");
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesWithUnMatchedEtag()
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json", "text/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);
            var etag = "TEST-IF-MATCH";


            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfMatch = etag + "_FAIL"
                }
            };

            context.ResponseInfo.Headers.Add(new ApiHeader("ETag", etag));

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(201);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be("application/json");
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesWithUnMatchedIfModifiedSince()
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json", "text/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);
            DateTimeOffset lastModifed = DateTimeOffset.UtcNow;


            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Method = "GET",
                    Accept = "application/json",
                    IfModifiedSince = lastModifed.AddSeconds(1)
                }
            };

            context.ResponseInfo.Headers.Add(new ApiHeader("Last-Modified", lastModifed.ToString("r")));

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(201);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be("application/json");
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesUsingMatchedFormatterAndPrettyPrint()
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Accept = "application/json",
                    PrettyPrint = true
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(201);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("X-PrettyPrint");
            context.ResponseInfo.Headers[0].Value.Should().Be("true");
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be("application/json");
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesUsingMatchedFormatterAndNoPrettyPrintWhenOverriden()
        {
            var formatter = SetupJsonFormatterMock(new string[] { "application/json" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 201,
                    ResponseObject = "test"
                },
                RequestInfo = new ApiRequestInfo
                {
                    Accept = "application/json",
                    PrettyPrint = true
                },
                ProcessingInfo = new ApiProcessingInfo
                {
                    OverridingFormatOptions = new FormatterOptions
                    {
                        PrettyPrint = false
                    }
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(201);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("X-PrettyPrint");
            context.ResponseInfo.Headers[0].Value.Should().Be("false");
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be("application/json");
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        private Mock<HttpMediaTypeStreamWriterFactory> SetupFormatterFactory(params IFormatStreamReaderWriter[] formatters)
        {
            var mockFactory = new Mock<HttpMediaTypeStreamWriterFactory>(new object[] { null, null })
            {
                CallBase = true
            };

            mockFactory.Setup(m => m.GetFormatters())
                .Returns(new List<IFormatStreamReaderWriter>(formatters));

            return mockFactory;
        }

        private Mock<JsonHttpFormatter> SetupJsonFormatterMock(string[] contentTypes, string[] charsets)
        {
            var mockFormatter = new Mock<JsonHttpFormatter>(new object[] { null })
            {
                CallBase = true
            };
            mockFormatter.Setup(m => m.SuuportedContentTypes).Returns(contentTypes);
            mockFormatter.Setup(m => m.SuuportedCharsets).Returns(charsets);
            return mockFormatter;
        }
    }
}
