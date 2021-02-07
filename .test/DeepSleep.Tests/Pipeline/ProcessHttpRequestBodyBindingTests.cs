namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Media;
    using DeepSleep.Media.Serializers;
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
        public async void body_binding___returns_false_for_cancelled_request()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true),
                Request = null
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var processed = await context.ProcessHttpRequestBodyBinding(contextResolver, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("get")]
        [InlineData("head")]
        [InlineData("trace")]
        [InlineData("options")]
        [InlineData("delete")]
        public async void body_binding___returns_true_for_non_bodybound_request_method(string method)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = method
                }
            };

            var processed = await context.ProcessHttpRequestBodyBinding(new ApiRequestContextResolver(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("PUT")]
        [InlineData("paTch")]
        [InlineData("put")]
        public async void body_binding___returns_false_and_411_status_for_missing_contenttype(string method)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = method,
                    ContentLength = null
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var processed = await context.ProcessHttpRequestBodyBinding(contextResolver, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(411);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void body_binding___returns_false_and_415_status_for_missing_contenttype_and_has_content(string contentType)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "PUT",
                    ContentLength = 1,
                    ContentType = contentType
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var processed = await context.ProcessHttpRequestBodyBinding(contextResolver, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(415);
        }

        [Fact]
        public async void body_binding___returns_false_and_411_status_for_contentlength_supplied_but_null_invocationcontext()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "PATCH",
                    ContentLength = 1,
                    ContentType = "application/json",
                    InvocationContext = null
                },
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var processed = await context.ProcessHttpRequestBodyBinding(contextResolver, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(413);
        }

        [Fact]
        public async void body_binding___returns_false_and_411_status_for_contentlength_supplied_but_null_invocationcontext_bodymodeltype()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "PATCH",
                    ContentLength = 1,
                    ContentType = "application/json",
                    InvocationContext = new ApiInvocationContext
                    {
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: null,
                            httpMethod: null,
                            methodInfo: null,
                            uriParameterType: null,
                            bodyParameterType: null,
                            simpleParameters: null,
                            methodReturnType: null)
                    }
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var processed = await context.ProcessHttpRequestBodyBinding(contextResolver, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(413);
        }

        [Fact]
        public async void body_binding___returns_false_and_415_status_for_null_formatterfactory()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "PATCH",
                    ContentLength = 1,
                    ContentType = "application/json",
                    InvocationContext = new ApiInvocationContext
                    {
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: null,
                            httpMethod: null,
                            methodInfo: null,
                            uriParameterType: null,
                            bodyParameterType: typeof(string),
                            simpleParameters: null,
                            methodReturnType: null)
                    }
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var processed = await context.ProcessHttpRequestBodyBinding(contextResolver, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(415);
        }

        [Fact]
        public async void body_binding___returns_false_and_415_status_for_unmatched_formatter()
        {
            var formatter = SetupXmlFormatterMock(null, new string[] { "application/xml" });
            var mockFactory = SetupFormatterFactory(formatter.Object);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "PATCH",
                    ContentLength = 1,
                    ContentType = "application/json",
                    InvocationContext = new ApiInvocationContext
                    {
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: null,
                            httpMethod: null,
                            methodInfo: null,
                            uriParameterType: null,
                            bodyParameterType: typeof(string),
                            simpleParameters: null,
                            methodReturnType: null)
                    }
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var processed = await context.ProcessHttpRequestBodyBinding(contextResolver, mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(415);
        }

        [Fact]
        public async void body_binding___returns_false_and_415_status_for_non_writable_formatter()
        {
            var xmlformatter = SetupXmlFormatterMock(null, null);
            var jsonformatter = SetupJsonFormatterMock(null, null);
            var mockFactory = SetupFormatterFactory(xmlformatter.Object, jsonformatter.Object);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "POST",
                    ContentLength = 1,
                    ContentType = "application/json",
                    InvocationContext = new ApiInvocationContext
                    {
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: null,
                            httpMethod: null,
                            methodInfo: null,
                            uriParameterType: null,
                            bodyParameterType: typeof(string),
                            simpleParameters: null,
                            methodReturnType: null)
                    }
                }
            };

            var contextResolver = new ApiRequestContextResolver();
            contextResolver.SetContext(context);

            var processed = await context.ProcessHttpRequestBodyBinding(contextResolver, mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(415);
        }

        [Fact]
        public async void body_binding___returns_false_and_413_status_for_maxrequestlength_too_long()
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
                    Request = new ApiRequestInfo
                    {
                        Method = "PATCH",
                        ContentLength = 10,
                        ContentType = "application/json",
                        InvocationContext = new ApiInvocationContext
                        {
                            //BodyModelType = typeof(ApiHeader)
                        },
                        Body = memoryStream
                    },
                    Configuration = new DeepSleepRequestConfiguration
                    {
                        RequestValidation = new ApiRequestValidationConfiguration
                        {
                            MaxRequestLength = 1
                        }
                    },
                    Routing = new ApiRoutingInfo
                    {
                        Route = new ApiRoutingItem
                        {
                            Location = new ApiEndpointLocation(
                            controller: null,
                            httpMethod: null,
                            methodInfo: null,
                            uriParameterType: null,
                            bodyParameterType: null,
                            simpleParameters: null,
                            methodReturnType: null)
                        }
                    }
                };

                var contextResolver = new ApiRequestContextResolver();
                contextResolver.SetContext(context);

                var processed = await context.ProcessHttpRequestBodyBinding(contextResolver, mockFactory.Object).ConfigureAwait(false);
                processed.Should().BeFalse();

                context.Response.Should().NotBeNull();
                context.Response.ResponseObject.Should().BeNull();
                context.Response.StatusCode.Should().Be(413);
            }
        }

        [Fact]
        public async void body_binding___returns_true_for_successfull_bodymodel_binding()
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
                    Request = new ApiRequestInfo
                    {
                        Method = "PATCH",
                        ContentLength = 1,
                        ContentType = "application/json",
                        InvocationContext = new ApiInvocationContext
                        {
                        },
                        Body = memoryStream
                    },
                    Routing = new ApiRoutingInfo
                    {
                        Route = new ApiRoutingItem
                        {
                            Location = new ApiEndpointLocation(
                                controller: null,
                                httpMethod: null,
                                methodInfo: null,
                                uriParameterType: null,
                                bodyParameterType: typeof(ApiHeader),
                                simpleParameters: null,
                                methodReturnType: null)
                        }
                    }
                };

                var contextResolver = new ApiRequestContextResolver();
                contextResolver.SetContext(context);

                var processed = await context.ProcessHttpRequestBodyBinding(contextResolver, mockFactory.Object).ConfigureAwait(false);
                processed.Should().BeTrue();

                context.Response.Should().NotBeNull();
                context.Response.ResponseObject.Should().BeNull();
                context.Request.InvocationContext.BodyModel.Should().NotBeNull();
                context.Request.InvocationContext.BodyModel.Should().BeOfType<ApiHeader>();

                ((ApiHeader)context.Request.InvocationContext.BodyModel).Name.Should().Be("MyHeader");
                ((ApiHeader)context.Request.InvocationContext.BodyModel).Value.Should().Be("MyValue");
            }
        }

        [Fact]
        public async void body_binding___returns_false_for_failed_bodymodel_binding()
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
                    Request = new ApiRequestInfo
                    {
                        Method = "PATCH",
                        ContentLength = 1,
                        ContentType = "application/json",
                        InvocationContext = new ApiInvocationContext
                        {
                        },
                        Body = memoryStream
                    },
                    Configuration = new DeepSleepRequestConfiguration
                    {
                        ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                        {
                            HttpStatusMode = ValidationHttpStatusMode.CommonHttpSpecificationWithCustomDeserializationStatus
                        }
                    },
                    Routing = new ApiRoutingInfo
                    {
                        Route = new ApiRoutingItem
                        {
                            Location = new ApiEndpointLocation(
                            controller: null,
                            httpMethod: null,
                            methodInfo: null,
                            uriParameterType: null,
                            bodyParameterType: typeof(ApiHeader),
                            simpleParameters: null,
                            methodReturnType: null)
                        }
                    }
                };

                var contextResolver = new ApiRequestContextResolver();
                contextResolver.SetContext(context);

                var processed = await context.ProcessHttpRequestBodyBinding(contextResolver, mockFactory.Object).ConfigureAwait(false);
                processed.Should().BeFalse();

                context.Response.Should().NotBeNull();
                context.Response.ResponseObject.Should().BeNull();
                context.Response.StatusCode.Should().Be(450);

                context.Validation.Should().NotBeNull();
                context.Validation.Errors.Should().NotBeNull();
                context.Validation.Errors.Should().HaveCount(0);
            }
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

        private Mock<DeepSleepXmlMediaSerializer> SetupXmlFormatterMock(string[] readableTypes, string[] writeableTypes)
        {
            var mockFormatter = new Mock<DeepSleepXmlMediaSerializer>(new object[] { null })
            {
                CallBase = true
            };
            mockFormatter.Setup(m => m.ReadableMediaTypes).Returns(readableTypes);
            mockFormatter.Setup(m => m.WriteableMediaTypes).Returns(writeableTypes);
            return mockFormatter;
        }
    }
}
