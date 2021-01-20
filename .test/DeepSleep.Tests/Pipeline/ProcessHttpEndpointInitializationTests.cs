namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Pipeline;
    using DeepSleep.Tests.TestArtifacts;
    using FluentAssertions;
    using Moq;
    using System;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpEndpointInitializationTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ThrowsForNullRouteInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = null
            };


            try
            {
                var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be("Routing item's controller type is null");
                return;
            }

            Assert.False(true);
        }

        [Fact]
        public async void ThrowsForNullRoutingItem()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = null
                }
            };


            try
            {
                var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be("Routing item's controller type is null");
                return;
            }

            Assert.False(true);
        }

        [Fact]
        public async void ThrowsForNullEndpointLocation()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = null
                    }
                }
            };


            try
            {
                var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be("Routing item's controller type is null");
                return;
            }

            Assert.False(true);
        }

        [Fact]
        public async void ThrowsForNullController()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(null, null, null)
                    }
                }
            };


            try
            {
                var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be("Routing item's controller type is null");
                return;
            }

            Assert.False(true);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void ThrowsForNullEndpoint(string endpoint)
        {
            var controllerType = typeof(StandardController);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: controllerType,
                            endpoint: endpoint,
                            httpMethod: null)
                    }
                }
            };


            try
            {
                var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be("Routing item's endpoint name is null");
                return;
            }

            Assert.False(true);
        }

        [Fact]
        public async void ReturnsTrueAndRetrivesControllerFromServiceProvider()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(InjectionController),
                            endpoint: nameof(InjectionController.DefaultEndpoint),
                            httpMethod: null)
                    }
                },
                RequestServices = mockServiceProvider.Object
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Request.InvocationContext.ControllerInstance.Should().NotBeNull();
            context.Request.InvocationContext.ControllerInstance.Should().BeOfType<InjectionController>();
            context.Request.InvocationContext.UriModel.Should().BeNull();
            context.Request.InvocationContext.BodyModel.Should().BeNull();
            context.Routing.Route.Location.UriParameterType?.Should().BeNull();
            context.Routing.Route.Location.BodyParameterType.Should().BeNull();
            context.Routing.Route.Location.MethodInfo.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(context.Routing.Route.Location.Endpoint);
        }

        [Fact]
        public async void ReturnsTrueAndRetrivesActivatesControlleWhenServiceProviderIsNull()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(InjectionController),
                            endpoint: nameof(InjectionController.DefaultEndpoint),
                            httpMethod: null)
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Request.InvocationContext.ControllerInstance.Should().NotBeNull();
            context.Request.InvocationContext.ControllerInstance.Should().BeOfType<InjectionController>();
            context.Request.InvocationContext.UriModel.Should().BeNull();
            context.Request.InvocationContext.BodyModel.Should().BeNull();
            context.Routing.Route.Location.BodyParameterType.Should().BeNull();
            context.Routing.Route.Location.UriParameterType.Should().BeNull();
            context.Routing.Route.Location.MethodInfo.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(context.Routing.Route.Location.Endpoint);
        }

        [Fact]
        public async void ReturnsTrueAndRetrivesActivatesControlleWhenServiceProviderDoesntContainController()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(InjectionController),
                            endpoint: nameof(InjectionController.DefaultEndpoint),
                            httpMethod: null)
                    }
                },
                RequestServices = mockServiceProvider.Object
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Request.InvocationContext.ControllerInstance.Should().NotBeNull();
            context.Request.InvocationContext.ControllerInstance.Should().BeOfType<InjectionController>();
            context.Request.InvocationContext.UriModel.Should().BeNull();
            context.Request.InvocationContext.BodyModel.Should().BeNull();
            context.Routing.Route.Location.BodyParameterType.Should().BeNull();
            context.Routing.Route.Location.UriParameterType.Should().BeNull();
            context.Routing.Route.Location.MethodInfo.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(context.Routing.Route.Location.Endpoint);
        }

        [Fact]
        public async void ReturnsTrueAndRetrivesControllerEndpointForInternalMethod()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(InjectionController),
                            endpoint: nameof(InjectionController.DefaultEndpointInternal),
                            httpMethod: null)
                    }
                },
                RequestServices = mockServiceProvider.Object
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Request.InvocationContext.ControllerInstance.Should().NotBeNull();
            context.Request.InvocationContext.ControllerInstance.Should().BeOfType<InjectionController>();
            context.Request.InvocationContext.UriModel.Should().BeNull();
            context.Request.InvocationContext.BodyModel.Should().BeNull();
            context.Routing.Route.Location.BodyParameterType.Should().BeNull();
            context.Routing.Route.Location.UriParameterType.Should().BeNull();
            context.Routing.Route.Location.MethodInfo.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(context.Routing.Route.Location.Endpoint);
        }

        [Fact]
        public async void ReturnsTrueAndRetrivesControllerEndpointForPrivateMethod()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(InjectionController),
                            endpoint: "DefaultEndpointPrivate",
                            httpMethod: null)
                    }
                },
                RequestServices = mockServiceProvider.Object
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Request.InvocationContext.ControllerInstance.Should().NotBeNull();
            context.Request.InvocationContext.ControllerInstance.Should().BeOfType<InjectionController>();
            context.Request.InvocationContext.UriModel.Should().BeNull();
            context.Request.InvocationContext.BodyModel.Should().BeNull();
            context.Routing.Route.Location.BodyParameterType.Should().BeNull();
            context.Routing.Route.Location.UriParameterType.Should().BeNull();
            context.Routing.Route.Location.MethodInfo.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(context.Routing.Route.Location.Endpoint);
        }

        [Fact]
        public async void ReturnsTrueAndRetrivesControllerEndpointForProtectedMethod()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(InjectionController),
                            endpoint: "DefaultEndpointProtected",
                            httpMethod: null)
                    }
                },
                RequestServices = mockServiceProvider.Object
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Request.InvocationContext.ControllerInstance.Should().NotBeNull();
            context.Request.InvocationContext.ControllerInstance.Should().BeOfType<InjectionController>();
            context.Request.InvocationContext.UriModel.Should().BeNull();
            context.Request.InvocationContext.BodyModel.Should().BeNull();
            context.Routing.Route.Location.BodyParameterType.Should().BeNull();
            context.Routing.Route.Location.UriParameterType.Should().BeNull();
            context.Routing.Route.Location.MethodInfo.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(context.Routing.Route.Location.Endpoint);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("get")]
        [InlineData("post")]
        [InlineData("Post")]
        [InlineData("PUT")]
        [InlineData("pUt")]
        [InlineData("head")]
        [InlineData("HEAD")]
        [InlineData("TRACE")]
        [InlineData("paTch")]
        [InlineData("PATCH")]
        [InlineData("patch")]
        [InlineData("options")]
        public async void ReturnsTrueAndRetrivesUriModelForOnlyUriParameter(string method)
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(InjectionController),
                            endpoint: nameof(InjectionController.DefaultEndpointWithUri),
                            httpMethod: method)
                    }
                },
                RequestServices = mockServiceProvider.Object
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Request.InvocationContext.ControllerInstance.Should().NotBeNull();
            context.Request.InvocationContext.ControllerInstance.Should().BeOfType<InjectionController>();
            context.Request.InvocationContext.UriModel.Should().BeNull();
            context.Request.InvocationContext.BodyModel.Should().BeNull();
            context.Routing.Route.Location.BodyParameterType.Should().BeNull();
            context.Routing.Route.Location.UriParameterType.Should().Be(typeof(StandardModel));
            context.Routing.Route.Location.MethodInfo.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(context.Routing.Route.Location.Endpoint);
        }

        [Theory]
        [InlineData("PATch")]
        [InlineData("PATCH")]
        [InlineData("post")]
        [InlineData("Post")]
        [InlineData("PUT")]
        [InlineData("pUt")]
        public async void ReturnsTrueAndRetrivesUriModelForOnlyBodyParameter(string method)
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(InjectionController),
                            endpoint: nameof(InjectionController.DefaultEndpointWithBody),
                            httpMethod: method)
                    }
                },
                RequestServices = mockServiceProvider.Object
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Request.InvocationContext.ControllerInstance.Should().NotBeNull();
            context.Request.InvocationContext.ControllerInstance.Should().BeOfType<InjectionController>();
            context.Request.InvocationContext.UriModel.Should().BeNull();
            context.Request.InvocationContext.BodyModel.Should().BeNull();
            context.Routing.Route.Location.BodyParameterType.Should().Be(typeof(StandardModel));
            context.Routing.Route.Location.UriParameterType.Should().BeNull();
            context.Routing.Route.Location.MethodInfo.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(context.Routing.Route.Location.Endpoint);
        }

        [Theory]
        [InlineData("post")]
        [InlineData("Post")]
        [InlineData("PUT")]
        [InlineData("pUt")]
        [InlineData("PATCH")]
        [InlineData("patch")]
        public async void ReturnsTrueAndRetrivesModelsForUriAndBodyParameter(string method)
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(InjectionController),
                            endpoint: nameof(InjectionController.DefaultEndpointWithUriAndBody),
                            httpMethod: method)
                    }
                },
                RequestServices = mockServiceProvider.Object
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Request.InvocationContext.ControllerInstance.Should().NotBeNull();
            context.Request.InvocationContext.ControllerInstance.Should().BeOfType<InjectionController>();
            context.Request.InvocationContext.UriModel.Should().BeNull();
            context.Request.InvocationContext.BodyModel.Should().BeNull();
            context.Routing.Route.Location.BodyParameterType.Should().Be(typeof(StandardNullableModel));
            context.Routing.Route.Location.UriParameterType.Should().Be(typeof(StandardModel));
            context.Routing.Route.Location.MethodInfo.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(context.Routing.Route.Location.Endpoint);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("get")]
        [InlineData("head")]
        [InlineData("HEAD")]
        [InlineData("TRACE")]
        [InlineData("options")]
        public async void ReturnsTrueAndRetrivesUriModelForUriAndBodyParameterAndNoneBodyMethod(string method)
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(InjectionController),
                            endpoint: nameof(InjectionController.DefaultEndpointWithUriAndBody),
                            httpMethod: method)
                    }
                },
                RequestServices = mockServiceProvider.Object
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Request.InvocationContext.ControllerInstance.Should().NotBeNull();
            context.Request.InvocationContext.ControllerInstance.Should().BeOfType<InjectionController>();
            context.Request.InvocationContext.UriModel.Should().BeNull();
            context.Request.InvocationContext.BodyModel.Should().BeNull();
            context.Routing.Route.Location.BodyParameterType.Should().BeNull();
            context.Routing.Route.Location.UriParameterType.Should().Be(typeof(StandardModel));
            context.Routing.Route.Location.MethodInfo.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(context.Routing.Route.Location.Endpoint);
        }

        [Theory]
        [InlineData("post")]
        [InlineData("Post")]
        [InlineData("PUT")]
        [InlineData("pUt")]
        [InlineData("PATCH")]
        [InlineData("patch")]
        public async void ReturnsTrueAndRetrivesModelsForUriAndBodyParameterWhenEndpointHasExtraParametersBefore(string method)
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(InjectionController),
                            endpoint: nameof(InjectionController.DefaultEndpointWithUriAndBodyAndOthersBefore),
                            httpMethod: method)
                    }
                },
                RequestServices = mockServiceProvider.Object
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Request.InvocationContext.ControllerInstance.Should().NotBeNull();
            context.Request.InvocationContext.ControllerInstance.Should().BeOfType<InjectionController>();
            context.Request.InvocationContext.UriModel.Should().BeNull();
            context.Request.InvocationContext.BodyModel.Should().BeNull();

            context.Routing.Route.Location.BodyParameterType.Should().Be(typeof(StandardNullableModel));
            context.Routing.Route.Location.UriParameterType.Should().Be(typeof(StandardModel));
            context.Routing.Route.Location.MethodInfo.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(context.Routing.Route.Location.Endpoint);
        }

        [Theory]
        [InlineData("post")]
        [InlineData("Post")]
        [InlineData("PUT")]
        [InlineData("pUt")]
        [InlineData("PATCH")]
        [InlineData("patch")]
        public async void ReturnsTrueAndRetrivesModelsForUriAndBodyParameterWhenEndpointHasExtraParametersAfter(string method)
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController());

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Route = new ApiRoutingItem
                    {
                        Location = new ApiEndpointLocation(
                            controller: typeof(InjectionController),
                            endpoint: nameof(InjectionController.DefaultEndpointWithUriAndBodyAndOthersAfter),
                            httpMethod: method)
                    }
                },
                RequestServices = mockServiceProvider.Object
            };

            var processed = await context.ProcessHttpEndpointInitialization().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Request.InvocationContext.ControllerInstance.Should().NotBeNull();
            context.Request.InvocationContext.ControllerInstance.Should().BeOfType<InjectionController>();
            context.Request.InvocationContext.UriModel.Should().BeNull();
            context.Request.InvocationContext.BodyModel.Should().BeNull();

            context.Routing.Route.Location.BodyParameterType.Should().Be(typeof(StandardNullableModel));
            context.Routing.Route.Location.UriParameterType.Should().Be(typeof(StandardModel));
            context.Routing.Route.Location.MethodInfo.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(context.Routing.Route.Location.Endpoint);
        }
    }
}
