namespace DeepSleep.Tests.PipelineTests
{
    using DeepSleep.Formatting;
    using DeepSleep.Formatting.Formatters;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Moq;
    using System;
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

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(204);
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
                    ResponseObject = new ApiResponse
                    {
                        Body = null,
                        StatusCode = 200
                    }
                },
                RequestInfo = mockRequestInfo.Object
            };

            var processed = await context.ProcessHttpResponseBodyWriting(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(204);
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
                    ResponseObject = new ApiResponse
                    {
                        Body = "test",
                        StatusCode = 200
                    }
                },
                RequestInfo = mockRequestInfo.Object
            };

            var processed = await context.ProcessHttpResponseBodyWriting(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(204);
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Fact]
        public async void ReturnsTrueAndDoesNotWriteForNullFormatter()
        {
            var formatterFactory = new HttpMediaTypeStreamWriterFactory();

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    ResponseObject = new ApiResponse
                    {
                        Body = "test",
                        StatusCode = 200
                    }
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(formatterFactory).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(204);
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Theory]
        [InlineData("application/xml")]
        [InlineData("text/json")]
        [InlineData("application/pdf")]
        [InlineData("text/plain")]
        public async void ReturnsTrueAndDoesNotWriteForNonMatchingFormatter(string accept)
        {
            var formatterFactory = new HttpMediaTypeStreamWriterFactory();
            formatterFactory.Add(new JsonHttpFormatter(), new string[] { "application/json" }, null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    ResponseObject = new ApiResponse
                    {
                        Body = "test",
                        StatusCode = 200
                    }
                },
                RequestInfo = new ApiRequestInfo
                {
                    Accept = accept
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(formatterFactory).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(204);
            context.ResponseInfo.ContentLength.Should().Be(0);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void ReturnsTrueAndDoesWritesUsingDefaultFormatterWhenMissingRequestAccept(string accept)
        {
            var formatterFactory = new HttpMediaTypeStreamWriterFactory();
            formatterFactory.Add(new JsonHttpFormatter(), new string[] { "application/json" }, null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    ResponseObject = new ApiResponse
                    {
                        Body = "test",
                        StatusCode = 201
                    }
                },
                RequestInfo = new ApiRequestInfo
                {
                    Accept = accept
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(formatterFactory).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(201);
            context.ResponseInfo.RawResponseObject.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(0);
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be("application/json");
            context.ResponseInfo.ContentLength.Should().Be(9);
            context.ResponseInfo.RawResponseObject.Length.Should().Be(9);
        }

        [Theory]
        [InlineData("application/json", "application/json")]
        [InlineData("text/json", "text/json")]
        [InlineData("*/*", "application/json")]
        [InlineData(null, "application/json")]
        public async void ReturnsTrueAndDoesWritesUsingMatchedFormatter(string accept, string expectedContentType)
        {
            var formatterFactory = new HttpMediaTypeStreamWriterFactory();
            formatterFactory.Add(new JsonHttpFormatter(), new string[] { "application/json", "text/json" }, null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    ResponseObject = new ApiResponse
                    {
                        Body = "test",
                        StatusCode = 201
                    }
                },
                RequestInfo = new ApiRequestInfo
                {
                    Accept = accept
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(formatterFactory).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(201);
            context.ResponseInfo.RawResponseObject.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(0);
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be(expectedContentType);
            context.ResponseInfo.ContentLength.Should().Be(9);
            context.ResponseInfo.RawResponseObject.Length.Should().Be(9);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesUsingMatchedFormatterAndPrettyPrint()
        {
            var formatterFactory = new HttpMediaTypeStreamWriterFactory();
            formatterFactory.Add(new JsonHttpFormatter(), new string[] { "application/json" }, null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    ResponseObject = new ApiResponse
                    {
                        Body = "test",
                        StatusCode = 201
                    }
                },
                RequestInfo = new ApiRequestInfo
                {
                    Accept = "application/json",
                    PrettyPrint = true
                }
            };

            var processed = await context.ProcessHttpResponseBodyWriting(formatterFactory).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(201);
            context.ResponseInfo.RawResponseObject.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("X-PrettyPrint");
            context.ResponseInfo.Headers[0].Value.Should().Be("true");
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be("application/json");
            context.ResponseInfo.ContentLength.Should().Be(9);
            context.ResponseInfo.RawResponseObject.Length.Should().Be(9);
        }

        [Fact]
        public async void ReturnsTrueAndDoesWritesUsingMatchedFormatterAndNoPrettyPrintWhenOverriden()
        {
            var formatterFactory = new HttpMediaTypeStreamWriterFactory();
            formatterFactory.Add(new JsonHttpFormatter(), new string[] { "application/json" }, null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ResponseInfo = new ApiResponseInfo
                {
                    ResponseObject = new ApiResponse
                    {
                        Body = "test",
                        StatusCode = 201
                    }
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

            var processed = await context.ProcessHttpResponseBodyWriting(formatterFactory).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(201);
            context.ResponseInfo.RawResponseObject.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("X-PrettyPrint");
            context.ResponseInfo.Headers[0].Value.Should().Be("false");
            context.ResponseInfo.ContentType.Should().NotBeNull();
            context.ResponseInfo.ContentType.Should().Be("application/json");
            context.ResponseInfo.ContentLength.Should().Be(9);
            context.ResponseInfo.RawResponseObject.Length.Should().Be(9);
        }
    }
}
