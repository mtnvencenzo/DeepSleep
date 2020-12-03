namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Formatting;
    using DeepSleep.Formatting.Formatters;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Moq;
    using System.Collections.Generic;
    using System.IO;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpRequestBodyBindingTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true),
                RequestInfo = null
            };

            var processed = await context.ProcessHttpRequestBodyBinding(null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("get")]
        [InlineData("head")]
        [InlineData("trace")]
        [InlineData("options")]
        [InlineData("delete")]
        public async void ReturnsTrueForNonBodyBoundRequestMehod(string method)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Method = method
                }
            };

            var processed = await context.ProcessHttpRequestBodyBinding(null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("PUT")]
        [InlineData("paTch")]
        [InlineData("put")]
        public async void ReturnsFalseAnd411StatusForMissingContentType(string method)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Method = method,
                    ContentLength = null
                }
            };

            var processed = await context.ProcessHttpRequestBodyBinding(null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(411);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void ReturnsFalseAnd422StatusForMissingContentTypeAndHasContent(string contentType)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Method = "PUT",
                    ContentLength = 1,
                    ContentType = contentType
                }
            };

            var processed = await context.ProcessHttpRequestBodyBinding(null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(422);
        }

        [Fact]
        public async void ReturnsFalseAnd411StatusForContentLengthSuppliedButNullInvocationContext()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Method = "PATCH",
                    ContentLength = 1,
                    ContentType = "application/json",
                    InvocationContext = null
                },
                
            };

            var processed = await context.ProcessHttpRequestBodyBinding(null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(413);
        }

        [Fact]
        public async void ReturnsFalseAnd411StatusForContentLengthSuppliedButNullInvocationContextBodyModelType()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Method = "PATCH",
                    ContentLength = 1,
                    ContentType = "application/json",
                    InvocationContext = new ApiInvocationContext
                    {
                        BodyModelType = null
                    }
                },

            };

            var processed = await context.ProcessHttpRequestBodyBinding(null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(413);
        }

        [Fact]
        public async void ReturnsFalseAnd415StatusForNullFormatterFactory()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Method = "PATCH",
                    ContentLength = 1,
                    ContentType = "application/json",
                    InvocationContext = new ApiInvocationContext
                    {
                        BodyModelType = typeof(string)
                    }
                }
            };

            var processed = await context.ProcessHttpRequestBodyBinding(null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(415);
        }

        [Fact]
        public async void ReturnsFalseAnd415StatusForNUnMatchedFormatter()
        {
            var formatter = SetupXmlFormatterMock(new string[] { "application/xml" }, null);
            var mockFactory = SetupFormatterFactory(formatter.Object);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Method = "PATCH",
                    ContentLength = 1,
                    ContentType = "application/json",
                    InvocationContext = new ApiInvocationContext
                    {
                        BodyModelType = typeof(string)
                    }
                }
            };

            var processed = await context.ProcessHttpRequestBodyBinding(mockFactory.Object, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(415);
        }

        [Fact]
        public async void ReturnsTrueFormSuccessfullBodyModelBinding()
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write("{ \"Name\": \"MyHeader\", \"Value\": \"MyValue\" }");
                await writer.FlushAsync().ConfigureAwait(false);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var formatter = SetupJsonFormatterMock(new string[] { "application/json" }, null);
                var mockFactory = SetupFormatterFactory(formatter.Object);

                var context = new ApiRequestContext
                {
                    RequestAborted = new System.Threading.CancellationToken(false),
                    RequestInfo = new ApiRequestInfo
                    {
                        Method = "PATCH",
                        ContentLength = 1,
                        ContentType = "application/json",
                        InvocationContext = new ApiInvocationContext
                        {
                            BodyModelType = typeof(ApiHeader)
                        },
                        Body = memoryStream
                    }
                };

                var processed = await context.ProcessHttpRequestBodyBinding(mockFactory.Object, null).ConfigureAwait(false);
                processed.Should().BeTrue();

                context.ResponseInfo.Should().NotBeNull();
                context.ResponseInfo.ResponseObject.Should().BeNull();
                context.RequestInfo.InvocationContext.BodyModel.Should().NotBeNull();
                context.RequestInfo.InvocationContext.BodyModel.Should().BeOfType<ApiHeader>();

                ((ApiHeader)context.RequestInfo.InvocationContext.BodyModel).Name.Should().Be("MyHeader");
                ((ApiHeader)context.RequestInfo.InvocationContext.BodyModel).Value.Should().Be("MyValue");
            }
        }

        [Fact]
        public async void ReturnsFalseForFailedBodyBinding()
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write("<test></test>");
                await writer.FlushAsync().ConfigureAwait(false);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var formatter = SetupJsonFormatterMock(new string[] { "application/json" }, null);
                var mockFactory = SetupFormatterFactory(formatter.Object);

                var context = new ApiRequestContext
                {
                    RequestAborted = new System.Threading.CancellationToken(false),
                    RequestInfo = new ApiRequestInfo
                    {
                        Method = "PATCH",
                        ContentLength = 1,
                        ContentType = "application/json",
                        InvocationContext = new ApiInvocationContext
                        {
                            BodyModelType = typeof(ApiHeader)
                        },
                        Body = memoryStream
                    }
                };

                var processed = await context.ProcessHttpRequestBodyBinding(mockFactory.Object, new DefaultApiResponseMessageConverter()).ConfigureAwait(false);
                processed.Should().BeFalse ();

                context.ResponseInfo.Should().NotBeNull();
                context.ResponseInfo.ResponseObject.Should().BeNull();
                context.ResponseInfo.StatusCode.Should().Be(400);

                context.ProcessingInfo.Should().NotBeNull();
                context.ProcessingInfo.ExtendedMessages.Should().NotBeNull();
                context.ProcessingInfo.ExtendedMessages.Should().HaveCount(1);
                context.ProcessingInfo.ExtendedMessages[0].Code.Should().Be("400.000003");
                context.ProcessingInfo.ExtendedMessages[0].Message.Should().Be("Could not deserialize request.");
            }
        }

        private Mock<HttpMediaTypeStreamWriterFactory> SetupFormatterFactory(params IFormatStreamReaderWriter[] formatters)
        {
            var mockFactory = new Mock<HttpMediaTypeStreamWriterFactory>(new object[] { null })
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

        private Mock<XmlHttpFormatter> SetupXmlFormatterMock(string[] contentTypes, string[] charsets)
        {
            var mockFormatter = new Mock<XmlHttpFormatter>()
            {
                CallBase = true
            };
            mockFormatter.Setup(m => m.SuuportedContentTypes).Returns(contentTypes);
            mockFormatter.Setup(m => m.SuuportedCharsets).Returns(charsets);
            return mockFormatter;
        }
    }
}
