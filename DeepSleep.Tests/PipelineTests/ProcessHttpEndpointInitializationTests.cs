namespace DeepSleep.Tests.PipelineTests
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

            var processed = await context.ProcessHttpEndpointInitialization(null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ThrowsForNullRouteInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = null
            };


            try
            {
                var processed = await context.ProcessHttpEndpointInitialization(null, null).ConfigureAwait(false);
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
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = null
                }
            };


            try
            {
                var processed = await context.ProcessHttpEndpointInitialization(null, null).ConfigureAwait(false);
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
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = null
                    }
                }
            };


            try
            {
                var processed = await context.ProcessHttpEndpointInitialization(null, null).ConfigureAwait(false);
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
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = null
                        }
                    }
                }
            };


            try
            {
                var processed = await context.ProcessHttpEndpointInitialization(null, null).ConfigureAwait(false);
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
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = controllerType,
                            Endpoint = endpoint
                        }
                    }
                }
            };


            try
            {
                var processed = await context.ProcessHttpEndpointInitialization(null, null).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be("Routing item's endpoint name is null");
                return;
            }

            Assert.False(true);
        }

        [Fact]
        public async void ThrowsForMissingEndpointControllerMethod()
        {
            var controllerType = typeof(StandardController);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = controllerType,
                            Endpoint = "MissingEndpoint"
                        }
                    }
                }
            };


            try
            {
                var processed = await context.ProcessHttpEndpointInitialization(null, null).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be($"Routing items endpoint method 'MissingEndpoint' does not exist on controller 'StandardController'.");
                return;
            }

            Assert.False(true);
        }

        [Fact]
        public async void ThrowsForMissingEndpointControllerMethodCaseSensitivity()
        {
            var controllerType = typeof(StandardController);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = controllerType,
                            Endpoint = nameof(StandardController.DefaultEndpoint).ToLower()
                        }
                    }
                }
            };


            try
            {
                var processed = await context.ProcessHttpEndpointInitialization(null, null).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be($"Routing items endpoint method 'defaultendpoint' does not exist on controller 'StandardController'.");
                return;
            }

            Assert.False(true);
        }

        [Fact]
        public async void ReturnsTrueAndRetrivesControllerFromServiceProvider()
        {
            var logger = new ListLogger();
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController(logger));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = typeof(InjectionController),
                            Endpoint = nameof(InjectionController.DefaultEndpoint)
                        }
                    }
                }
            };
            
            var processed = await context.ProcessHttpEndpointInitialization(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.RequestInfo.InvocationContext.Controller.Should().NotBeNull();
            context.RequestInfo.InvocationContext.Controller.Should().BeOfType<InjectionController>();
            context.RequestInfo.InvocationContext.UriModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModel.Should().BeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Should().NotBeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Name.Should().Be(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint);

            var controller = context.RequestInfo.InvocationContext.Controller as InjectionController;
            controller.Logger.Should().NotBeNull();
            controller.Logger.Should().Be(logger);
        }

        [Fact]
        public async void ReturnsTrueAndRetrivesActivatesControlleWhenServiceProviderIsNull()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = typeof(InjectionController),
                            Endpoint = nameof(InjectionController.DefaultEndpoint)
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInitialization(null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.RequestInfo.InvocationContext.Controller.Should().NotBeNull();
            context.RequestInfo.InvocationContext.Controller.Should().BeOfType<InjectionController>();
            context.RequestInfo.InvocationContext.UriModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModel.Should().BeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Should().NotBeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Name.Should().Be(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint);

            var controller = context.RequestInfo.InvocationContext.Controller as InjectionController;
            controller.Logger.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueAndRetrivesActivatesControlleWhenServiceProviderDoesntContainController()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = typeof(InjectionController),
                            Endpoint = nameof(InjectionController.DefaultEndpoint)
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInitialization(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.RequestInfo.InvocationContext.Controller.Should().NotBeNull();
            context.RequestInfo.InvocationContext.Controller.Should().BeOfType<InjectionController>();
            context.RequestInfo.InvocationContext.UriModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModel.Should().BeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Should().NotBeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Name.Should().Be(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint);

            var controller = context.RequestInfo.InvocationContext.Controller as InjectionController;
            controller.Logger.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueAndRetrivesControllerEndpointForInternalMethod()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = typeof(InjectionController),
                            Endpoint = nameof(InjectionController.DefaultEndpointInternal)
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInitialization(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.RequestInfo.InvocationContext.Controller.Should().NotBeNull();
            context.RequestInfo.InvocationContext.Controller.Should().BeOfType<InjectionController>();
            context.RequestInfo.InvocationContext.UriModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModel.Should().BeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Should().NotBeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Name.Should().Be(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint);

            var controller = context.RequestInfo.InvocationContext.Controller as InjectionController;
            controller.Logger.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueAndRetrivesControllerEndpointForPrivateMethod()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = typeof(InjectionController),
                            Endpoint = "DefaultEndpointPrivate"
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInitialization(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.RequestInfo.InvocationContext.Controller.Should().NotBeNull();
            context.RequestInfo.InvocationContext.Controller.Should().BeOfType<InjectionController>();
            context.RequestInfo.InvocationContext.UriModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModel.Should().BeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Should().NotBeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Name.Should().Be(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint);

            var controller = context.RequestInfo.InvocationContext.Controller as InjectionController;
            controller.Logger.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueAndRetrivesControllerEndpointForProtectedMethod()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(null);

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = typeof(InjectionController),
                            Endpoint = "DefaultEndpointProtected"
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInitialization(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.RequestInfo.InvocationContext.Controller.Should().NotBeNull();
            context.RequestInfo.InvocationContext.Controller.Should().BeOfType<InjectionController>();
            context.RequestInfo.InvocationContext.UriModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModel.Should().BeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Should().NotBeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Name.Should().Be(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint);

            var controller = context.RequestInfo.InvocationContext.Controller as InjectionController;
            controller.Logger.Should().BeNull();
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
            var logger = new ListLogger();
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController(logger));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = typeof(InjectionController),
                            Endpoint = nameof(InjectionController.DefaultEndpointWithUri),
                            HttpMethod = method
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInitialization(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.RequestInfo.InvocationContext.Controller.Should().NotBeNull();
            context.RequestInfo.InvocationContext.Controller.Should().BeOfType<InjectionController>();
            context.RequestInfo.InvocationContext.UriModelType.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModelType.Should().Be(typeof(StandardModel));
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModel.Should().BeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Should().NotBeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Name.Should().Be(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint);

            var controller = context.RequestInfo.InvocationContext.Controller as InjectionController;
            controller.Logger.Should().NotBeNull();
            controller.Logger.Should().Be(logger);
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
            var logger = new ListLogger();
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController(logger));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = typeof(InjectionController),
                            Endpoint = nameof(InjectionController.DefaultEndpointWithBody),
                            HttpMethod = method
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInitialization(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.RequestInfo.InvocationContext.Controller.Should().NotBeNull();
            context.RequestInfo.InvocationContext.Controller.Should().BeOfType<InjectionController>();
            context.RequestInfo.InvocationContext.UriModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().NotBeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().Be(typeof(StandardModel));
            context.RequestInfo.InvocationContext.BodyModel.Should().BeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Should().NotBeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Name.Should().Be(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint);

            var controller = context.RequestInfo.InvocationContext.Controller as InjectionController;
            controller.Logger.Should().NotBeNull();
            controller.Logger.Should().Be(logger);
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
            var logger = new ListLogger();
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController(logger));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = typeof(InjectionController),
                            Endpoint = nameof(InjectionController.DefaultEndpointWithUriAndBody),
                            HttpMethod = method
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInitialization(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.RequestInfo.InvocationContext.Controller.Should().NotBeNull();
            context.RequestInfo.InvocationContext.Controller.Should().BeOfType<InjectionController>();
            context.RequestInfo.InvocationContext.UriModelType.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModelType.Should().Be(typeof(StandardModel));
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().Be(typeof(StandardNullableModel));
            context.RequestInfo.InvocationContext.BodyModel.Should().BeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Should().NotBeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Name.Should().Be(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint);

            var controller = context.RequestInfo.InvocationContext.Controller as InjectionController;
            controller.Logger.Should().NotBeNull();
            controller.Logger.Should().Be(logger);
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
            var logger = new ListLogger();
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController(logger));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = typeof(InjectionController),
                            Endpoint = nameof(InjectionController.DefaultEndpointWithUriAndBody),
                            HttpMethod = method
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInitialization(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.RequestInfo.InvocationContext.Controller.Should().NotBeNull();
            context.RequestInfo.InvocationContext.Controller.Should().BeOfType<InjectionController>();
            context.RequestInfo.InvocationContext.UriModelType.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModelType.Should().Be(typeof(StandardModel));
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModel.Should().BeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Should().NotBeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Name.Should().Be(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint);

            var controller = context.RequestInfo.InvocationContext.Controller as InjectionController;
            controller.Logger.Should().NotBeNull();
            controller.Logger.Should().Be(logger);
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
            var logger = new ListLogger();
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController(logger));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = typeof(InjectionController),
                            Endpoint = nameof(InjectionController.DefaultEndpointWithUriAndBodyAndOthersBefore),
                            HttpMethod = method
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInitialization(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.RequestInfo.InvocationContext.Controller.Should().NotBeNull();
            context.RequestInfo.InvocationContext.Controller.Should().BeOfType<InjectionController>();
            context.RequestInfo.InvocationContext.UriModelType.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModelType.Should().Be(typeof(StandardModel));
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().Be(typeof(StandardNullableModel));
            context.RequestInfo.InvocationContext.BodyModel.Should().BeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Should().NotBeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Name.Should().Be(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint);

            var controller = context.RequestInfo.InvocationContext.Controller as InjectionController;
            controller.Logger.Should().NotBeNull();
            controller.Logger.Should().Be(logger);
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
            var logger = new ListLogger();
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.IsAny<Type>())).Returns(new InjectionController(logger));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        EndpointLocation = new ApiEndpointLocation
                        {
                            Controller = typeof(InjectionController),
                            Endpoint = nameof(InjectionController.DefaultEndpointWithUriAndBodyAndOthersAfter),
                            HttpMethod = method
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpEndpointInitialization(mockServiceProvider.Object, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.RequestInfo.InvocationContext.Controller.Should().NotBeNull();
            context.RequestInfo.InvocationContext.Controller.Should().BeOfType<InjectionController>();
            context.RequestInfo.InvocationContext.UriModelType.Should().NotBeNull();
            context.RequestInfo.InvocationContext.UriModelType.Should().Be(typeof(StandardModel));
            context.RequestInfo.InvocationContext.UriModel.Should().BeNull();
            context.RequestInfo.InvocationContext.BodyModelType.Should().Be(typeof(StandardNullableModel));
            context.RequestInfo.InvocationContext.BodyModel.Should().BeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Should().NotBeNull();
            context.RequestInfo.InvocationContext.ControllerMethod.Name.Should().Be(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint);

            var controller = context.RequestInfo.InvocationContext.Controller as InjectionController;
            controller.Logger.Should().NotBeNull();
            controller.Logger.Should().Be(logger);
        }
    }
}
