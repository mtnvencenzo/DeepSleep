namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Pipeline;
    using DeepSleep.Tests.TestArtifacts;
    using FluentAssertions;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpEndpointInvocationTests
    {
        [Fact]
        public async Task endpoint_invocation___returns_false_for_cancelled_request()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpEndpointInvocation().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async Task endpoint_invocation___returns_true_for_null_invocation_context()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    InvocationContext = null
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async Task endpoint_invocation___returns_true_for_null_controller_method()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
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
                            methodInfo: null,
                            httpMethod: null)
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async Task endpoint_invocation___returns_true_for_void_and_empty_parameter_endpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        ControllerInstance = controller,
                        //ControllerMethod = controller.GetType().GetMethod(nameof(controller.DefaultEndpoint))
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async Task endpoint_invocation___returns_true_for_task_and_empty_parameter_endpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        ControllerInstance = controller,
                        //ControllerMethod = controller.GetType().GetMethod(nameof(controller.DefaultTaskEndpoint))
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async Task endpoint_invocation___returns_true_for_generic_task_and_empty_parameter_endpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        ControllerInstance = controller,
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: controller.GetType(),
                            methodInfo: controller.GetType().GetMethod(nameof(controller.DefaultGenericTaskEndpoint)),
                            httpMethod: null)
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeOfType<int>();
            context.Response.ResponseObject.Should().Be(100);
        }

        [Fact]
        public async Task endpoint_invocation___returns_true_for_api_response_task_and_empty_parameter_endpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        ControllerInstance = controller,
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: controller.GetType(),
                            methodInfo: controller.GetType().GetMethod(nameof(controller.DefaultGenericTaskWithFullApiResponseEndpoint)),
                            httpMethod: null)
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(200);
            context.Response.ResponseObject.Should().BeOfType<int>();
            context.Response.ResponseObject.Should().Be(100);
        }

        [Fact]
        public async Task endpoint_invocation___returns_true_for_api_response_and_empty_parameter_endpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        ControllerInstance = controller,
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: controller.GetType(),
                            methodInfo: controller.GetType().GetMethod(nameof(controller.DefaultFullApiResponseEndpoint)),
                            httpMethod: null)
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeOfType<int>();
            context.Response.ResponseObject.Should().Be(200);
        }

        [Fact]
        public async Task endpoint_invocation___returns_true_for_api_response_and_uri_and_body_parameter_not_attributed_endpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        ControllerInstance = controller,
                        UriModel = new StandardModel
                        {
                            IntProp = 202
                        },
                        BodyModel = new StandardNullableModel
                        {
                            IntProp = 300
                        }
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: controller.GetType(),
                            methodInfo: controller.GetType().GetMethod(nameof(controller.DefaultFullApiResponseEndpointWithUriParameterAndBodyParameterNotAttributed)),
                            httpMethod: null)
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(200);
            context.Response.ResponseObject.Should().BeOfType<int>();
            context.Response.ResponseObject.Should().Be(300);
        }

        [Fact]
        public async Task endpoint_invocation___returns_true_for_api_response_and_uri_and_body_parameter_attributed_endpoint()
        {
            var controller = new StandardController();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        ControllerInstance = controller,
                        UriModel = new StandardModel
                        {
                            IntProp = 204
                        },
                        BodyModel = new StandardNullableModel
                        {
                            IntProp = 301
                        }
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: controller.GetType(),
                            methodInfo: controller.GetType().GetMethod(nameof(controller.DefaultFullApiResponseEndpointWithUriParameterAndBodyParameterAttributed)),
                            httpMethod: null)
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(200);
            context.Response.ResponseObject.Should().BeOfType<int>();
            context.Response.ResponseObject.Should().Be(301);
        }

        [Fact]
        public async Task endpoint_invocation___returns_true_for_api_response_and_uri_and_body_parameter_and_extra_parameters_endpoint()
        {
            var controller = new StandardController();

            var uriModel = new StandardModel
            {
                IntProp = 204
            };

            var bodyModel = new StandardNullableModel
            {
                IntProp = 301
            };

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        ControllerInstance = controller,
                        UriModel = uriModel,
                        BodyModel = bodyModel
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: controller.GetType(),
                            methodInfo: controller.GetType().GetMethod(nameof(controller.DefaultFullApiResponseEndpointWithUriParameterAndBodyParameterAndExtraParameters)),
                            httpMethod: null)
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInvocation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.StatusCode.Should().Be(200);
            context.Response.ResponseObject.Should().BeOfType<int>();
            context.Response.ResponseObject.Should().Be(301);
        }

        [Fact]
        public void endpoint_invocation___model_lookup_tests()
        {
            var controller = new StandardController();

            var uriModel = new StandardModel
            {
                IntProp = 204
            };

            var bodyModel = new StandardNullableModel
            {
                IntProp = 301
            };

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    InvocationContext = new ApiInvocationContext
                    {
                        ControllerInstance = controller,
                        UriModel = uriModel,
                        BodyModel = bodyModel,
                    }
                }
            };


            // Test model lookup methods
            var models = context.Request.InvocationContext.Models();
            models.Should().NotBeNull();
            models.Should().HaveCount(2);
            models.Should().Contain(uriModel);
            models.Should().Contain(bodyModel);

            var foundUriModel = context.Request.InvocationContext.Models<StandardModel>().FirstOrDefault();
            foundUriModel.Should().NotBeNull();
            foundUriModel.Should().BeSameAs(uriModel);

            var foundBodyModel = context.Request.InvocationContext.Models<StandardNullableModel>().FirstOrDefault();
            foundBodyModel.Should().NotBeNull();
            foundBodyModel.Should().BeSameAs(bodyModel);

            var baseModels = context.Request.InvocationContext.Models<StandardModelBase>();
            baseModels.Should().NotBeNull();
            baseModels.Should().HaveCount(2);
        }
    }
}
