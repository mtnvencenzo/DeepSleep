namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Pipeline;
    using DeepSleep.Tests.TestArtifacts;
    using FluentAssertions;
    using System.Reflection;
    using System.Threading;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpEndpointInvocationTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpEndpointInvocation(null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForNullInvocationContext()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = null
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForNullControllerMethod()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        ControllerMethod = null
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForVoidAndEmptyParameterEndpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        Controller = controller,
                        ControllerMethod = controller.GetType().GetMethod(nameof(controller.DefaultEndpoint))
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForTaskAndEmptyParameterEndpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        Controller = controller,
                        ControllerMethod = controller.GetType().GetMethod(nameof(controller.DefaultTaskEndpoint))
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForGenericTaskAndEmptyParameterEndpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        Controller = controller,
                        ControllerMethod = controller.GetType().GetMethod(nameof(controller.DefaultGenericTaskEndpoint))
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeOfType<int>();
            context.ResponseInfo.ResponseObject.Should().Be(100);
        }

        [Fact]
        public async void ReturnsTrueForApiResponseTaskAndEmptyParameterEndpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        Controller = controller,
                        ControllerMethod = controller.GetType().GetMethod(nameof(controller.DefaultGenericTaskWithFullApiResponseEndpoint))
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(200);
            context.ResponseInfo.ResponseObject.Should().BeOfType<int>();
            context.ResponseInfo.ResponseObject.Should().Be(100);
        }

        [Fact]
        public async void ReturnsTrueForApiResponseAndEmptyParameterEndpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        Controller = controller,
                        ControllerMethod = controller.GetType().GetMethod(nameof(controller.DefaultFullApiResponseEndpoint))
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeOfType<int>();
            context.ResponseInfo.ResponseObject.Should().Be(200);
        }

        [Fact]
        public async void ReturnsTrueForApiResponseAndUriAndBodyParameterNotAttributedEndpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        Controller = controller,
                        ControllerMethod = controller.GetType().GetMethod(nameof(controller.DefaultFullApiResponseEndpointWithUriParameterAndBodyParameterNotAttributed)),
                        UriModel = new StandardModel
                        {
                            IntProp = 202
                        },
                        BodyModel = new StandardNullableModel
                        {
                            IntProp = 300
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(200);
            context.ResponseInfo.ResponseObject.Should().BeOfType<int>();
            context.ResponseInfo.ResponseObject.Should().Be(300);
        }

        [Fact]
        public async void ReturnsTrueForApiResponseAndUriAndBodyParameterAttributedEndpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        Controller = controller,
                        ControllerMethod = controller.GetType().GetMethod(nameof(controller.DefaultFullApiResponseEndpointWithUriParameterAndBodyParameterAttributed)),
                        UriModel = new StandardModel
                        {
                            IntProp = 204
                        },
                        BodyModel = new StandardNullableModel
                        {
                            IntProp = 301
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(200);
            context.ResponseInfo.ResponseObject.Should().BeOfType<int>();
            context.ResponseInfo.ResponseObject.Should().Be(301);
        }

        [Fact]
        public async void ReturnsTrueForApiResponseAndUriAndBodyParameterAndExtraParametersEndpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        Controller = controller,
                        ControllerMethod = controller.GetType().GetMethod(nameof(controller.DefaultFullApiResponseEndpointWithUriParameterAndBodyParameterAndExtraParameters)),
                        UriModel = new StandardModel
                        {
                            IntProp = 204
                        },
                        BodyModel = new StandardNullableModel
                        {
                            IntProp = 301
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.StatusCode.Should().Be(200);
            context.ResponseInfo.ResponseObject.Should().BeOfType<int>();
            context.ResponseInfo.ResponseObject.Should().Be(301);
        }
    }
}
