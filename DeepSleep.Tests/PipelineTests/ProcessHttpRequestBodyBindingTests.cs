namespace DeepSleep.Tests.PipelineTests
{
    using DeepSleep.Configuration;
    using DeepSleep.Formatting;
    using DeepSleep.Formatting.Formatters;
    using DeepSleep.Pipeline;
    using FluentAssertions;
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

            var processed = await context.ProcessHttpRequestBodyBinding(null, null, null).ConfigureAwait(false);
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

            var processed = await context.ProcessHttpRequestBodyBinding(null, null, null).ConfigureAwait(false);
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

            var processed = await context.ProcessHttpRequestBodyBinding(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(411);
            context.ResponseInfo.ResponseObject.Body.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void ReturnsFalseAnd415StatusForMissingContentTypeAndHasContent(string contentType)
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

            var processed = await context.ProcessHttpRequestBodyBinding(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(415);
            context.ResponseInfo.ResponseObject.Body.Should().BeNull();
        }

        [Fact]
        public async void ReturnsFalseAnd411StatusForCOntentLengthSuppliedButNullInvocationContext()
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

            var processed = await context.ProcessHttpRequestBodyBinding(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(413);
            context.ResponseInfo.ResponseObject.Body.Should().BeNull();
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

            var processed = await context.ProcessHttpRequestBodyBinding(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(413);
            context.ResponseInfo.ResponseObject.Body.Should().BeNull();
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

            var processed = await context.ProcessHttpRequestBodyBinding(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(415);
            context.ResponseInfo.ResponseObject.Body.Should().BeNull();
        }

        [Fact]
        public async void ReturnsFalseAnd415StatusForNUnMatchedFormatter()
        {
            var formatterFactory = new HttpMediaTypeStreamWriterFactory();
            formatterFactory.Add(new XmlHttpFormatter(), new string[] { "application/xml" }, null);

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

            var processed = await context.ProcessHttpRequestBodyBinding(formatterFactory, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(415);
            context.ResponseInfo.ResponseObject.Body.Should().BeNull();
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

                var formatterFactory = new HttpMediaTypeStreamWriterFactory();
                formatterFactory.Add(new JsonHttpFormatter(), new string[] { "application/json" }, null);

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

                var processed = await context.ProcessHttpRequestBodyBinding(formatterFactory, null, null).ConfigureAwait(false);
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

                var formatterFactory = new HttpMediaTypeStreamWriterFactory();
                formatterFactory.Add(new JsonHttpFormatter(), new string[] { "application/json" }, null);

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

                var processed = await context.ProcessHttpRequestBodyBinding(formatterFactory, new DefaultApiResponseMessageConverter(), null).ConfigureAwait(false);
                processed.Should().BeFalse ();

                context.ResponseInfo.Should().NotBeNull();
                context.ResponseInfo.ResponseObject.Should().NotBeNull();
                context.ResponseInfo.ResponseObject.StatusCode.Should().Be(400);
                context.ResponseInfo.ResponseObject.Body.Should().BeNull();

                context.ProcessingInfo.Should().NotBeNull();
                context.ProcessingInfo.ExtendedMessages.Should().NotBeNull();
                context.ProcessingInfo.ExtendedMessages.Should().HaveCount(1);
                context.ProcessingInfo.ExtendedMessages[0].Code.Should().Be("400.000003");
                context.ProcessingInfo.ExtendedMessages[0].Message.Should().Be("Could not deserialize request.");
            }
        }
    }
}
