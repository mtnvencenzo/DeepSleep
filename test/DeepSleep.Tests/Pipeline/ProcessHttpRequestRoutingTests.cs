﻿namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Auth;
    using DeepSleep.Configuration;
    using DeepSleep.Discovery;
    using DeepSleep.Media;
    using DeepSleep.Pipeline;
    using DeepSleep.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpRequestRoutingTests
    {
        [Fact]
        public async void returns_false_for_cancelled_request()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestRouting(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void returns_false_for_null_resolver()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false)
            };

            var processed = await context.ProcessHttpRequestRouting(new ApiRoutingTable(), null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().BeNull();
            context.Routing.Template.Should().BeNull();
        }

        [Fact]
        public async void returns_false_for_null_routing_table()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false)
            };

            var processed = await context.ProcessHttpRequestRouting(null, new ApiRouteResolver(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().BeNull();
            context.Routing.Template.Should().BeNull();
        }

        [Fact]
        public async void returns_false__null_route_and_template_for_empty_route_table()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false)
            };

            var processed = await context.ProcessHttpRequestRouting(new ApiRoutingTable(), new ApiRouteResolver(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().BeNull();
            context.Routing.Template.Should().BeNull();
        }

        // HEAD Match Tests
        // ----------------------

        [Theory]
        [InlineData("HEAD")]
        [InlineData("heaD")]
        public async void returns_get_endpoint_for_head_request(string head)
        {
            var routingTable = GetRoutingTable(null);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(method: head),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Route.HttpMethod.Should().Be("GET");
            context.Routing.Route.RouteVariables.Should().NotBeNull();
            context.Routing.Route.RouteVariables.Should().HaveCount(1);
            context.Routing.Route.RouteVariables.Should().ContainKey("id");
            context.Routing.Route.RouteVariables["id"].Should().Be("1");
            context.Routing.Route.Location.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(nameof(MockController.Get));
            context.Routing.Route.Location.Controller.Should().Be(typeof(MockController));
            context.Routing.Route.Location.HttpMethod.Should().Be("GET");
            context.Routing.Template.Should().NotBeNull();
            context.Routing.Template.Variables.Should().NotBeNull();
            context.Routing.Template.Variables.Should().HaveCount(1);
            context.Routing.Template.Template.Should().Be("test/{id}/name");
            context.Routing.Template.Locations.Should().NotBeNull();
            context.Routing.Template.Locations.Should().HaveCount(1);
            context.Routing.Template.Locations[0].Controller.Should().Be(typeof(MockController));
            context.Routing.Template.Locations[0].MethodInfo.Name.Should().Be(nameof(MockController.Get));
            context.Routing.Template.Locations[0].HttpMethod.Should().Be("GET");
            context.Configuration.Should().NotBeNull();
        }

        [Theory]
        [InlineData("HEAD")]
        [InlineData("heaD")]
        public async void returns_head_endpoint_for_head_request(string head)
        {
            var routingTable = GetRoutingTable(null);

            routingTable = routingTable.AddRoute(new DeepSleepRouteRegistration(
                template: "test/{id}/name",
                httpMethods: new[] { head },
                controller: typeof(MockController),
                endpoint: nameof(MockController.Head),
                config: null));

            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(method: head),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Route.HttpMethod.Should().Be("HEAD");
            context.Routing.Route.RouteVariables.Should().NotBeNull();
            context.Routing.Route.RouteVariables.Should().HaveCount(1);
            context.Routing.Route.RouteVariables.Should().ContainKey("id");
            context.Routing.Route.RouteVariables["id"].Should().Be("1");
            context.Routing.Route.Location.Should().NotBeNull();
            context.Routing.Route.Location.MethodInfo.Name.Should().Be(nameof(MockController.Head));
            context.Routing.Route.Location.Controller.Should().Be(typeof(MockController));
            context.Routing.Route.Location.HttpMethod.Should().Be("HEAD");
            context.Routing.Template.Should().NotBeNull();
            context.Routing.Template.Variables.Should().NotBeNull();
            context.Routing.Template.Variables.Should().HaveCount(1);
            context.Routing.Template.Template.Should().Be("test/{id}/name");
            context.Routing.Template.Locations.Should().NotBeNull();
            context.Routing.Template.Locations.Should().HaveCount(2);
            context.Routing.Template.Locations[0].Controller.Should().Be(typeof(MockController));
            context.Routing.Template.Locations[0].MethodInfo.Name.Should().Be(nameof(MockController.Get));
            context.Routing.Template.Locations[0].HttpMethod.Should().Be("GET");
            context.Routing.Template.Locations[1].Controller.Should().Be(typeof(MockController));
            context.Routing.Template.Locations[1].MethodInfo.Name.Should().Be(nameof(MockController.Head));
            context.Routing.Template.Locations[1].HttpMethod.Should().Be("HEAD");
            context.Configuration.Should().NotBeNull();
        }


        // Configuration Tests
        // ----------------------

        [Fact]
        public async void request_config___returns_system_default_when_default_and_endpoint_are_null()
        {
            IDeepSleepRequestConfiguration defaultConfig = null;
            IDeepSleepRequestConfiguration endpointConfig = null;

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);
        }

        [Theory]
        [InlineData(false, null, null)]
        [InlineData(true, true, null)]
        [InlineData(true, true, true)]
        [InlineData(true, null, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        public async void request_config___allowanoymous_returns_expected(bool expected, bool? def, bool? endpoint)
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                AllowAnonymous = def
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                AllowAnonymous = endpoint
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.AllowAnonymous.Should().Be(expected);
        }

        [Theory]
        [InlineData(true, null, null)]
        [InlineData(false, false, null)]
        [InlineData(false, false, false)]
        [InlineData(false, null, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, true)]
        public async void request_config___enableHeadForGetRequests_returns_expected(bool expected, bool? def, bool? endpoint)
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                EnableHeadForGetRequests = def
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                EnableHeadForGetRequests = endpoint
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.EnableHeadForGetRequests.Should().Be(expected);
        }

        [Fact]
        public async void request_config___allowanoymous_endpoint_null_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                AllowAnonymous = true
            };

            DeepSleepRequestConfiguration endpointConfig = null;

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.AllowAnonymous.Should().Be(true);
        }

        [Fact]
        public async void request_config___allowanoymous_endpoint_notnull_default_null_returns_expected()
        {
            DeepSleepRequestConfiguration defaultConfig = null;

            DeepSleepRequestConfiguration endpointConfig = new DeepSleepRequestConfiguration
            {
                AllowAnonymous = true
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.AllowAnonymous.Should().Be(true);
        }

        [Theory]
        [InlineData(false, null, null)]
        [InlineData(true, true, null)]
        [InlineData(true, true, true)]
        [InlineData(true, null, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        public async void request_config___allowrequestbodywhennomodeldefined_returns_expected(bool expected, bool? def, bool? endpoint)
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    AllowRequestBodyWhenNoModelDefined = def
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    AllowRequestBodyWhenNoModelDefined = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.RequestValidation.AllowRequestBodyWhenNoModelDefined.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("application/json", "application/json", null)]
        [InlineData("application/json", "application/json", "application/json")]
        [InlineData("application/json", null, "application/json")]
        [InlineData("application/json", "text/json", "application/json")]
        [InlineData("text/json", "application/json", "text/json")]
        public async void request_config___readWriteConfiguration_acceptheaderoverride_returns_expected(string expected, string def, string endpoint)
        {
            AcceptHeader expectedHeader = expected != null
                ? new AcceptHeader(expected)
                : null;

            var defaultConfig = new DeepSleepRequestConfiguration
            {
                ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                {
                    AcceptHeaderOverride = def != null
                        ? new AcceptHeader(def)
                        : null
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                {
                    AcceptHeaderOverride = endpoint != null
                        ? new AcceptHeader(endpoint)
                        : null
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.ReadWriteConfiguration.AcceptHeaderOverride.Should().Be(expectedHeader);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("application/json,text/json", "application/json,text/json", null)]
        [InlineData("application/json,text/json", "application/xml", "application/json,text/json")]
        [InlineData("application/json,text/json,text/plain", null, "application/json,text/json,text/plain")]
        [InlineData("application/json", "text/json", "application/json")]
        [InlineData("text/json", "application/json", "text/json")]
        public async void request_config___readWriteConfiguration_readblemediatypes_returns_expected(string expected, string def, string endpoint)
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                {
                    ReadableMediaTypes = def != null
                        ? def.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        : null
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                {
                    ReadableMediaTypes = endpoint != null
                        ? endpoint.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        : null
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            var requestTypes = context.Configuration.ReadWriteConfiguration.ReadableMediaTypes != null
                ? string.Join(",", context.Configuration.ReadWriteConfiguration.ReadableMediaTypes)
                : null;

            requestTypes.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("application/json,text/json", "application/json,text/json", null)]
        [InlineData("application/json,text/json", "application/xml", "application/json,text/json")]
        [InlineData("application/json,text/json,text/plain", null, "application/json,text/json,text/plain")]
        [InlineData("application/json", "text/json", "application/json")]
        [InlineData("text/json", "application/json", "text/json")]
        public async void request_config___readWriteConfiguration_writeablemediatypes_returns_expected(string expected, string def, string endpoint)
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                {
                    WriteableMediaTypes = def != null
                        ? def.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        : null
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                {
                    WriteableMediaTypes = endpoint != null
                        ? endpoint.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        : null
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            var requestTypes = context.Configuration.ReadWriteConfiguration.WriteableMediaTypes != null
                ? string.Join(",", context.Configuration.ReadWriteConfiguration.WriteableMediaTypes)
                : null;

            requestTypes.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(true, null, true)]
        [InlineData(true, true, null)]
        [InlineData(false, true, false)]
        public async void request_config___readWriteConfiguration_readerresolver_returns_expected(bool? expected, bool? def, bool? endpoint)
        {
            var defaultMockFormatter = new Mock<IDeepSleepMediaSerializer>();
            defaultMockFormatter.Setup(f => f.SupportsRead).Returns(def ?? false);
            defaultMockFormatter.Setup(f => f.SupportsWrite).Returns(def ?? false);

            var endpointMockFormatter = new Mock<IDeepSleepMediaSerializer>();
            endpointMockFormatter.Setup(f => f.SupportsRead).Returns(endpoint ?? false);
            endpointMockFormatter.Setup(f => f.SupportsWrite).Returns(endpoint ?? false);

            var defaultConfig = new DeepSleepRequestConfiguration
            {
                ReadWriteConfiguration = new ApiMediaSerializerConfiguration()
            };

            if (def.HasValue)
            {
                defaultConfig.ReadWriteConfiguration.ReaderResolver = (args) => Task.FromResult(new MediaSerializerReadOverrides(new[] { defaultMockFormatter.Object }));
            }

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                ReadWriteConfiguration = new ApiMediaSerializerConfiguration()
            };

            if (endpoint.HasValue)
            {
                endpointConfig.ReadWriteConfiguration.ReaderResolver = (args) => Task.FromResult(new MediaSerializerReadOverrides(new[] { endpointMockFormatter.Object }));
            }

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            if (!expected.HasValue)
            {
                context.Configuration.ReadWriteConfiguration.ReaderResolver.Should().BeNull();
            }
            else
            {
                context.Configuration.ReadWriteConfiguration.ReaderResolver.Should().NotBeNull();
                var overrides = await context.Configuration.ReadWriteConfiguration.ReaderResolver(null);
                overrides.Should().NotBeNull();
                overrides.Formatters.Should().NotBeNull();
                overrides.Formatters.Should().HaveCount(1);
                overrides.Formatters[0].SupportsRead.Should().Be(expected.Value);
                overrides.Formatters[0].SupportsWrite.Should().Be(expected.Value);
            }
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(true, null, true)]
        [InlineData(true, true, null)]
        [InlineData(false, true, false)]
        public async void request_config___readWriteConfiguration_writerresolver_returns_expected(bool? expected, bool? def, bool? endpoint)
        {
            var defaultMockFormatter = new Mock<IDeepSleepMediaSerializer>();
            defaultMockFormatter.Setup(f => f.SupportsRead).Returns(def ?? false);
            defaultMockFormatter.Setup(f => f.SupportsWrite).Returns(def ?? false);

            var endpointMockFormatter = new Mock<IDeepSleepMediaSerializer>();
            endpointMockFormatter.Setup(f => f.SupportsRead).Returns(endpoint ?? false);
            endpointMockFormatter.Setup(f => f.SupportsWrite).Returns(endpoint ?? false);

            var defaultConfig = new DeepSleepRequestConfiguration
            {
                ReadWriteConfiguration = new ApiMediaSerializerConfiguration()
            };

            if (def.HasValue)
            {
                defaultConfig.ReadWriteConfiguration.WriterResolver = (args) => Task.FromResult(new MediaSerializerWriteOverrides(new[] { defaultMockFormatter.Object }));
            }

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                ReadWriteConfiguration = new ApiMediaSerializerConfiguration()
            };

            if (endpoint.HasValue)
            {
                endpointConfig.ReadWriteConfiguration.WriterResolver = (args) => Task.FromResult(new MediaSerializerWriteOverrides(new[] { endpointMockFormatter.Object }));
            }

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            if (!expected.HasValue)
            {
                context.Configuration.ReadWriteConfiguration.WriterResolver.Should().BeNull();
            }
            else
            {
                context.Configuration.ReadWriteConfiguration.WriterResolver.Should().NotBeNull();
                var overrides = await context.Configuration.ReadWriteConfiguration.WriterResolver(null);
                overrides.Should().NotBeNull();
                overrides.Formatters.Should().NotBeNull();
                overrides.Formatters.Should().HaveCount(1);
                overrides.Formatters[0].SupportsRead.Should().Be(expected.Value);
                overrides.Formatters[0].SupportsWrite.Should().Be(expected.Value);
            }
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("en-US", "en-US", null)]
        [InlineData("en-US", "en-GB", "en-US")]
        [InlineData("en-US", null, "en-US")]
        public async void request_config___fallbacklanguage_returns_expected(string expected, string def, string endpoint)
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                LanguageSupport = new ApiLanguageSupportConfiguration
                {
                    FallBackLanguage = def
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                LanguageSupport = new ApiLanguageSupportConfiguration
                {
                    FallBackLanguage = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.LanguageSupport.FallBackLanguage.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(4, 4, null)]
        [InlineData(5, 4, 5)]
        [InlineData(6, null, 6)]
        public async void request_config___maxrequestlength_returns_expected(long? expected, long? def, long? endpoint)
        {
            var hasSet = false;

            var defaultConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    MaxRequestLength = def
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    MaxRequestLength = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null,
                ConfigureMaxRequestLength = (length) => { hasSet = true; }
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.RequestValidation.MaxRequestLength.Should().Be(expected);

            if (expected != null)
            {
                hasSet.Should().Be(true);
            }
            else
            {
                hasSet.Should().Be(false);
            }
        }

        [Fact]
        public async void request_config___maxrequestlength_does_not_fail_with_null_maxlengthsetter()
        {
            var endpointConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    MaxRequestLength = 10
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null,
                ConfigureMaxRequestLength = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, null);

            context.Configuration.RequestValidation.MaxRequestLength.Should().Be(10);
        }

        [Fact]
        public async void request_config___maxrequestlength_does_not_fail_with_maxlengthsetter_exception()
        {
            var hasSet = false;

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    MaxRequestLength = 10
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null,
                ConfigureMaxRequestLength = (length) =>
                {
                    hasSet = true;
                    throw new Exception("should not have been called");
                }
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, null);

            context.Configuration.RequestValidation.MaxRequestLength.Should().Be(10);
            hasSet.Should().Be(true);
        }

        [Theory]
        [InlineData(0, null, null)]
        [InlineData(4, 4, null)]
        [InlineData(5, 4, 5)]
        [InlineData(6, null, 6)]
        public async void request_config___maxrequesturiLength_returns_expected(int expected, int? def, int? endpoint)
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    MaxRequestUriLength = def
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    MaxRequestUriLength = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.RequestValidation.MaxRequestUriLength.Should().Be(expected);
        }


        [Theory]
        [InlineData(true, null, null)]
        [InlineData(false, false, null)]
        [InlineData(false, false, false)]
        [InlineData(false, null, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, true)]
        public async void request_config___requirecontentlengthonrequestbodyrequests_returns_expected(bool expected, bool? def, bool? endpoint)
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    RequireContentLengthOnRequestBodyRequests = def
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    RequireContentLengthOnRequestBodyRequests = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.RequestValidation.RequireContentLengthOnRequestBodyRequests.Should().Be(expected);
        }

        [Fact]
        public async void request_config___endpoint_supportedlanguages_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                LanguageSupport = new ApiLanguageSupportConfiguration
                {
                    SupportedLanguages = null
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                LanguageSupport = new ApiLanguageSupportConfiguration
                {
                    SupportedLanguages = new string[] { "test1", "test2" }
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.LanguageSupport.SupportedLanguages.Should().NotBeNull();
            context.Configuration.LanguageSupport.SupportedLanguages.Should().NotBeSameAs(defaultConfig.LanguageSupport.SupportedLanguages);
            context.Configuration.LanguageSupport.SupportedLanguages.Should().NotBeSameAs(endpointConfig.LanguageSupport.SupportedLanguages);
            context.Configuration.LanguageSupport.SupportedLanguages.Should().HaveCount(2);
            context.Configuration.LanguageSupport.SupportedLanguages[0].Should().Be("test1");
            context.Configuration.LanguageSupport.SupportedLanguages[1].Should().Be("test2");
        }

        [Fact]
        public async void request_config___endpoint_supportedlanguages_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                LanguageSupport = new ApiLanguageSupportConfiguration
                {
                    SupportedLanguages = new string[] { "test1", "test2" }
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                LanguageSupport = new ApiLanguageSupportConfiguration
                {
                    SupportedLanguages = null
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.LanguageSupport.SupportedLanguages.Should().NotBeNull();
            context.Configuration.LanguageSupport.SupportedLanguages.Should().NotBeSameAs(defaultConfig.LanguageSupport.SupportedLanguages);
            context.Configuration.LanguageSupport.SupportedLanguages.Should().NotBeSameAs(endpointConfig.LanguageSupport.SupportedLanguages);
            context.Configuration.LanguageSupport.SupportedLanguages.Should().HaveCount(2);
            context.Configuration.LanguageSupport.SupportedLanguages[0].Should().Be("test1");
            context.Configuration.LanguageSupport.SupportedLanguages[1].Should().Be("test2");
        }

        [Fact]
        public async void request_config___endpoint_supportedlanguages_notnull_default_notnull_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                LanguageSupport = new ApiLanguageSupportConfiguration
                {
                    SupportedLanguages = new string[] { "test1", "test2" }
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                LanguageSupport = new ApiLanguageSupportConfiguration
                {
                    SupportedLanguages = new string[] { "test3", "test4" }
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.LanguageSupport.SupportedLanguages.Should().NotBeNull();
            context.Configuration.LanguageSupport.SupportedLanguages.Should().NotBeSameAs(defaultConfig.LanguageSupport.SupportedLanguages);
            context.Configuration.LanguageSupport.SupportedLanguages.Should().NotBeSameAs(endpointConfig.LanguageSupport.SupportedLanguages);
            context.Configuration.LanguageSupport.SupportedLanguages.Should().HaveCount(2);
            context.Configuration.LanguageSupport.SupportedLanguages[0].Should().Be("test3");
            context.Configuration.LanguageSupport.SupportedLanguages[1].Should().Be("test4");
        }

        [Fact]
        public async void request_config___endpoint_cachedirective_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                CacheDirective = null
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                CacheDirective = new ApiCacheDirectiveConfiguration
                {
                    Cacheability = HttpCacheType.Cacheable,
                    CacheLocation = HttpCacheLocation.Public,
                    ExpirationSeconds = 100,
                    VaryHeaderValue = "Test"
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.CacheDirective.Should().NotBeNull();
            context.Configuration.CacheDirective.Should().NotBeSameAs(defaultConfig.CacheDirective);
            context.Configuration.CacheDirective.Should().NotBeSameAs(endpointConfig.CacheDirective);
            context.Configuration.CacheDirective.Cacheability.Should().Be(HttpCacheType.Cacheable);
            context.Configuration.CacheDirective.CacheLocation.Should().Be(HttpCacheLocation.Public);
            context.Configuration.CacheDirective.ExpirationSeconds.Should().Be(100);
            context.Configuration.CacheDirective.VaryHeaderValue.Should().Be("Test");
        }

        [Fact]
        public async void request_config___endpoint_cachedirective_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                CacheDirective = new ApiCacheDirectiveConfiguration
                {
                    Cacheability = HttpCacheType.Cacheable,
                    CacheLocation = HttpCacheLocation.Public,
                    ExpirationSeconds = 100,
                    VaryHeaderValue = "Test"
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                CacheDirective = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.CacheDirective.Should().NotBeNull();
            context.Configuration.CacheDirective.Should().NotBeSameAs(defaultConfig.CacheDirective);
            context.Configuration.CacheDirective.Should().NotBeSameAs(endpointConfig.CacheDirective);
            context.Configuration.CacheDirective.Cacheability.Should().Be(HttpCacheType.Cacheable);
            context.Configuration.CacheDirective.CacheLocation.Should().Be(HttpCacheLocation.Public);
            context.Configuration.CacheDirective.ExpirationSeconds.Should().Be(100);
            context.Configuration.CacheDirective.VaryHeaderValue.Should().Be("Test");
        }

        [Theory]
        [InlineData(HttpCacheType.NoCache, null, null)]
        [InlineData(HttpCacheType.Cacheable, HttpCacheType.NoCache, HttpCacheType.Cacheable)]
        [InlineData(HttpCacheType.Cacheable, HttpCacheType.Cacheable, null)]
        [InlineData(HttpCacheType.Cacheable, null, HttpCacheType.Cacheable)]
        public async void request_config___cachedirective_cacheability_returns_expected(HttpCacheType expected, HttpCacheType? def, HttpCacheType? endpoint)
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                CacheDirective = new ApiCacheDirectiveConfiguration
                {
                    Cacheability = def
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                CacheDirective = new ApiCacheDirectiveConfiguration
                {
                    Cacheability = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.CacheDirective.Should().NotBeNull();
            context.Configuration.CacheDirective.Should().NotBeSameAs(defaultConfig.CacheDirective);
            context.Configuration.CacheDirective.Should().NotBeSameAs(endpointConfig.CacheDirective);
            context.Configuration.CacheDirective.Cacheability.Should().Be(expected);
        }

        [Theory]
        [InlineData(HttpCacheLocation.Private, null, null)]
        [InlineData(HttpCacheLocation.Public, HttpCacheLocation.Private, HttpCacheLocation.Public)]
        [InlineData(HttpCacheLocation.Public, HttpCacheLocation.Public, null)]
        [InlineData(HttpCacheLocation.Public, null, HttpCacheLocation.Public)]
        public async void request_config___cachedirective_cachelocation_returns_expected(HttpCacheLocation expected, HttpCacheLocation? def, HttpCacheLocation? endpoint)
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                CacheDirective = new ApiCacheDirectiveConfiguration
                {
                    CacheLocation = def
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                CacheDirective = new ApiCacheDirectiveConfiguration
                {
                    CacheLocation = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.CacheDirective.Should().NotBeNull();
            context.Configuration.CacheDirective.Should().NotBeSameAs(defaultConfig.CacheDirective);
            context.Configuration.CacheDirective.Should().NotBeSameAs(endpointConfig.CacheDirective);
            context.Configuration.CacheDirective.CacheLocation.Should().Be(expected);
        }

        [Theory]
        [InlineData(-1, null, null)]
        [InlineData(100, -1, 100)]
        [InlineData(100, 100, null)]
        [InlineData(100, null, 100)]
        public async void request_config___cachedirective_expirationseconds_returns_expected(int expected, int? def, int? endpoint)
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                CacheDirective = new ApiCacheDirectiveConfiguration
                {
                    ExpirationSeconds = def
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                CacheDirective = new ApiCacheDirectiveConfiguration
                {
                    ExpirationSeconds = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.CacheDirective.Should().NotBeNull();
            context.Configuration.CacheDirective.Should().NotBeSameAs(defaultConfig.CacheDirective);
            context.Configuration.CacheDirective.Should().NotBeSameAs(endpointConfig.CacheDirective);
            context.Configuration.CacheDirective.ExpirationSeconds.Should().Be(expected);
        }

        [Theory]
        [InlineData("Accept, Accept-Encoding, Accept-Language", null, null)]
        [InlineData("Test", "Other", "Test")]
        [InlineData("Test", "Test", null)]
        [InlineData("Test", null, "Test")]
        public async void request_config___cachedirective_varyheadervalue_returns_expected(string expected, string def, string endpoint)
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                CacheDirective = new ApiCacheDirectiveConfiguration
                {
                    VaryHeaderValue = def
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                CacheDirective = new ApiCacheDirectiveConfiguration
                {
                    VaryHeaderValue = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.CacheDirective.Should().NotBeNull();
            context.Configuration.CacheDirective.Should().NotBeSameAs(defaultConfig.CacheDirective);
            context.Configuration.CacheDirective.Should().NotBeSameAs(endpointConfig.CacheDirective);
            context.Configuration.CacheDirective.VaryHeaderValue.Should().Be(expected);
        }

        [Fact]
        public async void request_config___crossoriginconfig_null_default_null_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                CrossOriginConfig = null
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                CrossOriginConfig = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.CrossOriginConfig.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(defaultConfig.CrossOriginConfig);
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(endpointConfig.CrossOriginConfig);
            context.Configuration.CrossOriginConfig.AllowCredentials.Should().Be(true);
            context.Configuration.CrossOriginConfig.MaxAgeSeconds.Should().Be(0);
            context.Configuration.CrossOriginConfig.AllowedHeaders.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.AllowedHeaders.Should().HaveCount(1);
            context.Configuration.CrossOriginConfig.AllowedHeaders[0].Should().Be("*");
            context.Configuration.CrossOriginConfig.AllowedOrigins.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.AllowedOrigins.Should().HaveCount(1);
            context.Configuration.CrossOriginConfig.AllowedOrigins[0].Should().Be("*");
            context.Configuration.CrossOriginConfig.ExposeHeaders.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.ExposeHeaders.Should().HaveCount(0);
        }

        [Fact]
        public async void request_config___crossoriginconfig_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                CrossOriginConfig = null
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                CrossOriginConfig = new ApiCrossOriginConfiguration
                {
                    AllowCredentials = false,
                    AllowedHeaders = new string[] { "Header1", "Header2" },
                    AllowedOrigins = new string[] { "http://origin1.ut", "http://origin12.ut" },
                    ExposeHeaders = new string[] { "ExHeader1", "ExHeader2" },
                    MaxAgeSeconds = 100
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.CrossOriginConfig.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(defaultConfig.CrossOriginConfig);
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(endpointConfig.CrossOriginConfig);
            context.Configuration.CrossOriginConfig.AllowCredentials.Should().Be(false);
            context.Configuration.CrossOriginConfig.MaxAgeSeconds.Should().Be(100);
            context.Configuration.CrossOriginConfig.AllowedHeaders.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.AllowedHeaders.Should().HaveCount(2);
            context.Configuration.CrossOriginConfig.AllowedHeaders[0].Should().Be("Header1");
            context.Configuration.CrossOriginConfig.AllowedHeaders[1].Should().Be("Header2");
            context.Configuration.CrossOriginConfig.AllowedOrigins.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.AllowedOrigins.Should().HaveCount(2);
            context.Configuration.CrossOriginConfig.AllowedOrigins[0].Should().Be("http://origin1.ut");
            context.Configuration.CrossOriginConfig.AllowedOrigins[1].Should().Be("http://origin12.ut");
            context.Configuration.CrossOriginConfig.ExposeHeaders.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.ExposeHeaders.Should().HaveCount(2);
            context.Configuration.CrossOriginConfig.ExposeHeaders[0].Should().Be("ExHeader1");
            context.Configuration.CrossOriginConfig.ExposeHeaders[1].Should().Be("ExHeader2");
        }

        [Fact]
        public async void request_config___crossoriginconfig_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                CrossOriginConfig = new ApiCrossOriginConfiguration
                {
                    AllowCredentials = false,
                    AllowedHeaders = new string[] { "Header1", "Header2" },
                    AllowedOrigins = new string[] { "http://origin1.ut", "http://origin12.ut" },
                    ExposeHeaders = new string[] { "ExHeader1", "ExHeader2" },
                    MaxAgeSeconds = 150
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                CrossOriginConfig = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.CrossOriginConfig.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(defaultConfig.CrossOriginConfig);
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(endpointConfig.CrossOriginConfig);
            context.Configuration.CrossOriginConfig.AllowCredentials.Should().Be(false);
            context.Configuration.CrossOriginConfig.MaxAgeSeconds.Should().Be(150);
            context.Configuration.CrossOriginConfig.AllowedHeaders.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.AllowedHeaders.Should().HaveCount(2);
            context.Configuration.CrossOriginConfig.AllowedHeaders[0].Should().Be("Header1");
            context.Configuration.CrossOriginConfig.AllowedHeaders[1].Should().Be("Header2");
            context.Configuration.CrossOriginConfig.AllowedOrigins.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.AllowedOrigins.Should().HaveCount(2);
            context.Configuration.CrossOriginConfig.AllowedOrigins[0].Should().Be("http://origin1.ut");
            context.Configuration.CrossOriginConfig.AllowedOrigins[1].Should().Be("http://origin12.ut");
            context.Configuration.CrossOriginConfig.ExposeHeaders.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.ExposeHeaders.Should().HaveCount(2);
            context.Configuration.CrossOriginConfig.ExposeHeaders[0].Should().Be("ExHeader1");
            context.Configuration.CrossOriginConfig.ExposeHeaders[1].Should().Be("ExHeader2");
        }

        [Fact]
        public async void request_config___crossoriginconfig__default_notnull_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                CrossOriginConfig = new ApiCrossOriginConfiguration
                {
                    AllowCredentials = true,
                    AllowedHeaders = new string[] { "D:Header1", "D:Header2" },
                    AllowedOrigins = new string[] { "http://dorigin1.ut", "http://dorigin12.ut" },
                    ExposeHeaders = new string[] { "D:ExHeader1", "D:ExHeader2" },
                    MaxAgeSeconds = 200
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                CrossOriginConfig = new ApiCrossOriginConfiguration
                {
                    AllowCredentials = false,
                    AllowedHeaders = new string[] { "E:Header1", "E:Header2" },
                    AllowedOrigins = new string[] { "http://eorigin1.ut", "http://eorigin12.ut" },
                    ExposeHeaders = new string[] { "E:ExHeader1", "E:ExHeader2" },
                    MaxAgeSeconds = 100
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.CrossOriginConfig.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(defaultConfig.CrossOriginConfig);
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(endpointConfig.CrossOriginConfig);
            context.Configuration.CrossOriginConfig.AllowCredentials.Should().Be(false);
            context.Configuration.CrossOriginConfig.MaxAgeSeconds.Should().Be(100);
            context.Configuration.CrossOriginConfig.AllowedHeaders.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.AllowedHeaders.Should().HaveCount(2);
            context.Configuration.CrossOriginConfig.AllowedHeaders[0].Should().Be("E:Header1");
            context.Configuration.CrossOriginConfig.AllowedHeaders[1].Should().Be("E:Header2");
            context.Configuration.CrossOriginConfig.AllowedOrigins.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.AllowedOrigins.Should().HaveCount(2);
            context.Configuration.CrossOriginConfig.AllowedOrigins[0].Should().Be("http://eorigin1.ut");
            context.Configuration.CrossOriginConfig.AllowedOrigins[1].Should().Be("http://eorigin12.ut");
            context.Configuration.CrossOriginConfig.ExposeHeaders.Should().NotBeNull();
            context.Configuration.CrossOriginConfig.ExposeHeaders.Should().HaveCount(2);
            context.Configuration.CrossOriginConfig.ExposeHeaders[0].Should().Be("E:ExHeader1");
            context.Configuration.CrossOriginConfig.ExposeHeaders[1].Should().Be("E:ExHeader2");
        }

        [Fact]
        public async void request_config___headervalidationconfig_null_default_null_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.RequestValidation.MaxHeaderLength.Should().Be(0);
        }

        [Fact]
        public async void request_config___headervalidationconfig_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    MaxHeaderLength = 200
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.RequestValidation.MaxHeaderLength.Should().Be(200);
        }

        [Fact]
        public async void request_config___headervalidationconfig_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    MaxHeaderLength = 200
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.RequestValidation.MaxHeaderLength.Should().Be(200);
        }

        [Fact]
        public async void request_config___headervalidationconfig_default_notnull_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    MaxHeaderLength = 100
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    MaxHeaderLength = 200
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.RequestValidation.MaxHeaderLength.Should().Be(200);
        }

        [Fact]
        public async void request_config___validationerrorconfig_null_default_null_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                ValidationErrorConfiguration = null
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                ValidationErrorConfiguration = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.ValidationErrorConfiguration.Should().NotBeNull();
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(defaultConfig.ValidationErrorConfiguration);
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(endpointConfig.ValidationErrorConfiguration);
            context.Configuration.ValidationErrorConfiguration.UriBindingError.Should().Be("'{paramName}' is in an incorrect format and could not be bound.");
            context.Configuration.ValidationErrorConfiguration.UriBindingValueError.Should().Be("Uri type conversion for '{paramName}' with value '{paramValue}' could not be converted to type {paramType}.");
            context.Configuration.ValidationErrorConfiguration.RequestDeserializationError.Should().Be("The request body could not be deserialized.");
            context.Configuration.ValidationErrorConfiguration.HttpStatusMode.Should().Be(ValidationHttpStatusMode.StrictHttpSpecification);
        }

        [Fact]
        public async void request_config___validationerrorconfig_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                ValidationErrorConfiguration = null
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "UriBindingError - Override",
                    UriBindingValueError = "UriBindingValueError - Override"
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.ValidationErrorConfiguration.Should().NotBeNull();
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(defaultConfig.ValidationErrorConfiguration);
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(endpointConfig.ValidationErrorConfiguration);
            context.Configuration.ValidationErrorConfiguration.UriBindingError.Should().Be("UriBindingError - Override");
            context.Configuration.ValidationErrorConfiguration.UriBindingValueError.Should().Be("UriBindingValueError - Override");
        }

        [Fact]
        public async void request_config___validationerrorconfig_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "UriBindingError - Override",
                    UriBindingValueError = "UriBindingValueError - Override"
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                ValidationErrorConfiguration = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.ValidationErrorConfiguration.Should().NotBeNull();
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(defaultConfig.ValidationErrorConfiguration);
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(endpointConfig.ValidationErrorConfiguration);
            context.Configuration.ValidationErrorConfiguration.UriBindingError.Should().Be("UriBindingError - Override");
            context.Configuration.ValidationErrorConfiguration.UriBindingValueError.Should().Be("UriBindingValueError - Override");
        }

        [Fact]
        public async void request_config___validationerrorconfig_default_notnull_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "UriBindingError - Override",
                    UriBindingValueError = "UriBindingValueError - Override"
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "UriBindingError - Override - Endpoint",
                    UriBindingValueError = "UriBindingValueError - Override - Endpoint"
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.ValidationErrorConfiguration.Should().NotBeNull();
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(defaultConfig.ValidationErrorConfiguration);
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(endpointConfig.ValidationErrorConfiguration);
            context.Configuration.ValidationErrorConfiguration.UriBindingError.Should().Be("UriBindingError - Override - Endpoint");
            context.Configuration.ValidationErrorConfiguration.UriBindingValueError.Should().Be("UriBindingValueError - Override - Endpoint");
        }

        [Fact]
        public async void request_config___validationerrorconfig_default_notnull_endpoint_mixed_null_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "UriBindingError - Override",
                    UriBindingValueError = "UriBindingValueError - Override"
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "UriBindingError - Override - Endpoint",
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.ValidationErrorConfiguration.Should().NotBeNull();
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(defaultConfig.ValidationErrorConfiguration);
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(endpointConfig.ValidationErrorConfiguration);
            context.Configuration.ValidationErrorConfiguration.UriBindingError.Should().Be("UriBindingError - Override - Endpoint");
            context.Configuration.ValidationErrorConfiguration.UriBindingValueError.Should().Be("UriBindingValueError - Override");
        }

        [Fact]
        public async void request_config___pipelinecomponents_default_notnull_returns_endpoint()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                PipelineComponents = new List<IRequestPipelineComponent>
                {
                    new RequestPipelineComponent<CustomRequestPipelineComponent>(PipelinePlacement.AfterEndpointInvocation, 0)
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                PipelineComponents = new List<IRequestPipelineComponent>
                {
                    new RequestPipelineComponent<CustomRequestPipelineComponent>(PipelinePlacement.AfterEndpointInvocation, 0),
                    new RequestPipelineComponent<CustomRequestPipelineComponent>(PipelinePlacement.AfterEndpointInvocation, 1),
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.PipelineComponents.Should().NotBeNull();
            context.Configuration.PipelineComponents.Should().NotBeSameAs(defaultConfig.PipelineComponents);
            context.Configuration.PipelineComponents.Should().NotBeSameAs(endpointConfig.PipelineComponents);
            context.Configuration.PipelineComponents.Should().HaveCount(2);
        }

        [Fact]
        public async void request_config___pipelinecomponents_default_notnull_returns_default()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                PipelineComponents = new List<IRequestPipelineComponent>
                {
                    new RequestPipelineComponent<CustomRequestPipelineComponent>(PipelinePlacement.AfterEndpointInvocation, 0)
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.PipelineComponents.Should().NotBeNull();
            context.Configuration.PipelineComponents.Should().NotBeSameAs(defaultConfig.PipelineComponents);
            context.Configuration.PipelineComponents.Should().NotBeSameAs(endpointConfig.PipelineComponents);
            context.Configuration.PipelineComponents.Should().HaveCount(1);
        }

        [Fact]
        public async void request_config___pipelinecomponents_default_null_returns_endpoint()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                PipelineComponents = new List<IRequestPipelineComponent>
                {
                    new RequestPipelineComponent<CustomRequestPipelineComponent>(PipelinePlacement.AfterEndpointInvocation, 0)
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.PipelineComponents.Should().NotBeNull();
            context.Configuration.PipelineComponents.Should().NotBeSameAs(defaultConfig.PipelineComponents);
            context.Configuration.PipelineComponents.Should().NotBeSameAs(endpointConfig.PipelineComponents);
            context.Configuration.PipelineComponents.Should().HaveCount(1);
        }

        [Fact]
        public async void request_config___pipelinecomponents_default_null_endpoint_null_returns_system()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.PipelineComponents.Should().NotBeNull();
            context.Configuration.PipelineComponents.Should().NotBeSameAs(defaultConfig.PipelineComponents);
            context.Configuration.PipelineComponents.Should().NotBeSameAs(endpointConfig.PipelineComponents);
            context.Configuration.PipelineComponents.Should().HaveCount(0);
        }

        [Fact]
        public async void request_config___validators_default_notnull_returns_endpoint()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                Validators = new List<IEndpointValidatorComponent>
                {
                    new EndpointValidatorComponent<CustomRequestPipelineModelValidator>()
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                Validators = new List<IEndpointValidatorComponent>
                {
                    new EndpointValidatorComponent<CustomRequestPipelineModelValidator>(),
                    new EndpointValidatorComponent<CustomRequestPipelineModelValidator>(),
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.Validators.Should().NotBeNull();
            context.Configuration.Validators.Should().NotBeSameAs(defaultConfig.Validators);
            context.Configuration.Validators.Should().NotBeSameAs(endpointConfig.Validators);
            context.Configuration.Validators.Should().HaveCount(2);
        }

        [Fact]
        public async void request_config___validators_default_notnull_returns_default()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                Validators = new List<IEndpointValidatorComponent>
                {
                    new EndpointValidatorComponent<CustomRequestPipelineModelValidator>()
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.Validators.Should().NotBeNull();
            context.Configuration.Validators.Should().NotBeSameAs(defaultConfig.Validators);
            context.Configuration.Validators.Should().NotBeSameAs(endpointConfig.Validators);
            context.Configuration.Validators.Should().HaveCount(1);
        }

        [Fact]
        public async void request_config___validators_default_null_returns_endpoint()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                Validators = new List<IEndpointValidatorComponent>
                {
                    new EndpointValidatorComponent<CustomRequestPipelineModelValidator>()
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.Validators.Should().NotBeNull();
            context.Configuration.Validators.Should().NotBeSameAs(defaultConfig.Validators);
            context.Configuration.Validators.Should().NotBeSameAs(endpointConfig.Validators);
            context.Configuration.Validators.Should().HaveCount(1);
        }

        [Fact]
        public async void request_config___validators_default_null_endpoint_null_returns_system()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.Validators.Should().NotBeNull();
            context.Configuration.Validators.Should().NotBeSameAs(defaultConfig.Validators);
            context.Configuration.Validators.Should().NotBeSameAs(endpointConfig.Validators);
            context.Configuration.Validators.Should().HaveCount(0);
        }

        [Fact]
        public async void request_config___authorizationproviders_endpoint_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                AuthorizationProviders = null
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                AuthorizationProviders = new List<IAuthorizationComponent>
                {
                    new ApiAuthorizationComponent<DefaultAuthorizationProvider>()
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.AuthorizationProviders.Should().NotBeNull();
            context.Configuration.AuthorizationProviders.Should().NotBeSameAs(defaultConfig.AuthorizationProviders);
            context.Configuration.AuthorizationProviders.Should().NotBeSameAs(endpointConfig.AuthorizationProviders);
            context.Configuration.AuthorizationProviders.Should().HaveCount(1);
            context.Configuration.AuthorizationProviders[0].Should().BeOfType<ApiAuthorizationComponent<DefaultAuthorizationProvider>>();
        }

        [Fact]
        public async void request_config___authorizationproviders_endpoint_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                AuthorizationProviders = new List<IAuthorizationComponent>
                {
                    new ApiAuthorizationComponent<DefaultAuthorizationProvider>()
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                AuthorizationProviders = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.AuthorizationProviders.Should().NotBeNull();
            context.Configuration.AuthorizationProviders.Should().NotBeSameAs(defaultConfig.AuthorizationProviders);
            context.Configuration.AuthorizationProviders.Should().NotBeSameAs(endpointConfig.AuthorizationProviders);
            context.Configuration.AuthorizationProviders.Should().HaveCount(1);
            context.Configuration.AuthorizationProviders[0].Should().BeOfType<ApiAuthorizationComponent<DefaultAuthorizationProvider>>();
        }

        [Fact]
        public async void request_config___authorizationproviders_endpoint_and_default_returns_expected()
        {
            var defaultConfig = new DeepSleepRequestConfiguration
            {
                AuthorizationProviders = new List<IAuthorizationComponent>
                {
                    new ApiAuthorizationComponent<DefaultAuthorizationProvider>()
                }
            };

            var endpointConfig = new DeepSleepRequestConfiguration
            {
                AuthorizationProviders = new List<IAuthorizationComponent>
                {
                    new ApiAuthorizationComponent<Default2AuthorizationProvider>()
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new ApiRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Routing.Should().NotBeNull();
            context.Routing.Route.Should().NotBeNull();
            context.Routing.Template.Should().NotBeNull();
            context.Configuration.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.Configuration, endpointConfig, defaultConfig);

            context.Configuration.AuthorizationProviders.Should().NotBeNull();
            context.Configuration.AuthorizationProviders.Should().NotBeSameAs(defaultConfig.AuthorizationProviders);
            context.Configuration.AuthorizationProviders.Should().NotBeSameAs(endpointConfig.AuthorizationProviders);
            context.Configuration.AuthorizationProviders.Should().HaveCount(1);
            context.Configuration.AuthorizationProviders[0].Should().BeOfType<ApiAuthorizationComponent<Default2AuthorizationProvider>>();
        }

        private void AssertConfiguration(IDeepSleepRequestConfiguration request, IDeepSleepRequestConfiguration endpoint, IDeepSleepRequestConfiguration def)
        {
            var system = ApiRequestContext.GetDefaultRequestConfiguration();

            request.AllowAnonymous.Should().Be(endpoint?.AllowAnonymous ?? def?.AllowAnonymous ?? system.AllowAnonymous);

            // -------------------------
            // Authorization Components
            // -------------------------
            request.AuthorizationProviders.Count.Should().Be(endpoint?.AuthorizationProviders?.Count ?? def?.AuthorizationProviders?.Count ?? system.AuthorizationProviders.Count);
            for (int i = 0; i < request.AuthorizationProviders.Count; i++)
            {
                request.AuthorizationProviders[i].Should().Be(endpoint?.AuthorizationProviders?[i] ?? def?.AuthorizationProviders?[i] ?? system.AuthorizationProviders[i]);
            }

            // -------------------------
            // Authentication Components
            // -------------------------
            request.AuthenticationProviders.Count.Should().Be(endpoint?.AuthenticationProviders?.Count ?? def?.AuthenticationProviders?.Count ?? system.AuthenticationProviders.Count);
            for (int i = 0; i < request.AuthenticationProviders.Count; i++)
            {
                request.AuthenticationProviders[i].Should().Be(endpoint?.AuthenticationProviders?[i] ?? def?.AuthenticationProviders?[i] ?? system.AuthenticationProviders[i]);
            }

            // ----------------------
            // Validator Configuration
            // ----------------------
            request.Validators.Count.Should().Be(endpoint?.Validators?.Count ?? def?.Validators?.Count ?? system.Validators.Count);
            for (int i = 0; i < request.Validators.Count; i++)
            {
                request.Validators[i].Should().Be(endpoint?.Validators?[i] ?? def?.Validators?[i] ?? system.Validators[i]);
            }

            // ----------------------
            // Pipeline Configuration
            // ----------------------
            request.PipelineComponents.Count.Should().Be(endpoint?.PipelineComponents?.Count ?? def?.PipelineComponents?.Count ?? system.PipelineComponents.Count);
            for (int i = 0; i < request.PipelineComponents.Count; i++)
            {
                request.PipelineComponents[i].Should().Be(endpoint?.PipelineComponents?[i] ?? def?.PipelineComponents?[i] ?? system.PipelineComponents[i]);
            }

            // -------------------
            // Language Support Configuration
            // -------------------
            request.LanguageSupport.FallBackLanguage.Should().Be(endpoint?.LanguageSupport?.FallBackLanguage ?? def?.LanguageSupport?.FallBackLanguage ?? system.LanguageSupport.FallBackLanguage);
            request.LanguageSupport.UseAcceptedLanguageAsThreadCulture.Should().Be(endpoint?.LanguageSupport?.UseAcceptedLanguageAsThreadCulture ?? def?.LanguageSupport?.UseAcceptedLanguageAsThreadCulture ?? system.LanguageSupport.UseAcceptedLanguageAsThreadCulture);
            request.LanguageSupport.UseAcceptedLanguageAsThreadUICulture.Should().Be(endpoint?.LanguageSupport?.UseAcceptedLanguageAsThreadUICulture ?? def?.LanguageSupport?.UseAcceptedLanguageAsThreadUICulture ?? system.LanguageSupport.UseAcceptedLanguageAsThreadUICulture);

            request.LanguageSupport.SupportedLanguages.Count.Should().Be(endpoint?.LanguageSupport?.SupportedLanguages?.Count ?? def?.LanguageSupport?.SupportedLanguages?.Count ?? system.LanguageSupport.SupportedLanguages.Count);
            for (int i = 0; i < request.LanguageSupport.SupportedLanguages.Count; i++)
            {
                request.LanguageSupport.SupportedLanguages[i].Should().Be(endpoint?.LanguageSupport?.SupportedLanguages?[i] ?? def?.LanguageSupport?.SupportedLanguages?[i] ?? system.LanguageSupport.SupportedLanguages[i]);
            }


            // -------------------
            // Request Validation Configuration
            // -------------------
            request.RequestValidation.MaxRequestLength.Should().Be(endpoint?.RequestValidation?.MaxRequestLength ?? def?.RequestValidation?.MaxRequestLength ?? system.RequestValidation.MaxRequestLength);
            request.RequestValidation.MaxRequestUriLength.Should().Be(endpoint?.RequestValidation?.MaxRequestUriLength ?? def?.RequestValidation?.MaxRequestUriLength ?? system.RequestValidation.MaxRequestUriLength);
            request.RequestValidation.MaxHeaderLength.Should().Be(endpoint?.RequestValidation?.MaxHeaderLength ?? def?.RequestValidation?.MaxHeaderLength ?? system.RequestValidation.MaxHeaderLength);
            request.RequestValidation.RequireContentLengthOnRequestBodyRequests.Should().Be(endpoint?.RequestValidation?.RequireContentLengthOnRequestBodyRequests ?? def?.RequestValidation?.RequireContentLengthOnRequestBodyRequests ?? system.RequestValidation.RequireContentLengthOnRequestBodyRequests);
            request.RequestValidation.AllowRequestBodyWhenNoModelDefined.Should().Be(endpoint?.RequestValidation?.AllowRequestBodyWhenNoModelDefined ?? def?.RequestValidation?.AllowRequestBodyWhenNoModelDefined ?? system.RequestValidation.AllowRequestBodyWhenNoModelDefined);


            // ------------------------
            // Read Write Configuration
            // ------------------------
            if (endpoint?.ReadWriteConfiguration?.ReaderResolver != null)
            {
                request.ReadWriteConfiguration.ReaderResolver.Should().Be(endpoint.ReadWriteConfiguration.ReaderResolver);
            }
            else if (def?.ReadWriteConfiguration?.ReaderResolver != null)
            {
                request.ReadWriteConfiguration.ReaderResolver.Should().Be(def.ReadWriteConfiguration.ReaderResolver);
            }
            else
            {
                request.ReadWriteConfiguration.ReaderResolver.Should().BeNull();
            }


            if (endpoint?.ReadWriteConfiguration?.WriterResolver != null)
            {
                request.ReadWriteConfiguration.WriterResolver.Should().Be(endpoint.ReadWriteConfiguration.WriterResolver);
            }
            else if (def?.ReadWriteConfiguration?.WriterResolver != null)
            {
                request.ReadWriteConfiguration.WriterResolver.Should().Be(def.ReadWriteConfiguration.WriterResolver);
            }
            else
            {
                request.ReadWriteConfiguration.WriterResolver.Should().BeNull();
            }

            request.ReadWriteConfiguration.AcceptHeaderOverride.Should().Be(endpoint?.ReadWriteConfiguration?.AcceptHeaderOverride
                ?? def?.ReadWriteConfiguration?.AcceptHeaderOverride
                ?? system.ReadWriteConfiguration?.AcceptHeaderOverride);

            request.ReadWriteConfiguration.ReadableMediaTypes?.Count.Should().Be(endpoint?.ReadWriteConfiguration?.ReadableMediaTypes?.Count
                ?? def?.ReadWriteConfiguration?.ReadableMediaTypes?.Count
                ?? system.ReadWriteConfiguration?.ReadableMediaTypes?.Count);
            for (int i = 0; i < (request.ReadWriteConfiguration?.ReadableMediaTypes ?? new List<string>()).Count; i++)
            {
                request.ReadWriteConfiguration.ReadableMediaTypes[i].Should().Be(endpoint?.ReadWriteConfiguration?.ReadableMediaTypes?[i] ?? def?.ReadWriteConfiguration?.ReadableMediaTypes?[i] ?? system.ReadWriteConfiguration.ReadableMediaTypes[i]);
            }

            request.ReadWriteConfiguration.WriteableMediaTypes?.Count.Should().Be(endpoint?.ReadWriteConfiguration?.WriteableMediaTypes?.Count
                ?? def?.ReadWriteConfiguration?.WriteableMediaTypes?.Count
                ?? system.ReadWriteConfiguration?.WriteableMediaTypes?.Count);
            for (int i = 0; i < (request.ReadWriteConfiguration?.WriteableMediaTypes ?? new List<string>()).Count; i++)
            {
                request.ReadWriteConfiguration.WriteableMediaTypes[i].Should().Be(endpoint?.ReadWriteConfiguration?.WriteableMediaTypes?[i] ?? def?.ReadWriteConfiguration?.WriteableMediaTypes?[i] ?? system.ReadWriteConfiguration.WriteableMediaTypes[i]);
            }


            // -------------------
            // Cache Directive Configuration
            // -------------------
            request.CacheDirective.Cacheability.Should().Be(endpoint?.CacheDirective?.Cacheability ?? def?.CacheDirective?.Cacheability ?? system.CacheDirective.Cacheability);
            request.CacheDirective.CacheLocation.Should().Be(endpoint?.CacheDirective?.CacheLocation ?? def?.CacheDirective?.CacheLocation ?? system.CacheDirective.CacheLocation);
            request.CacheDirective.ExpirationSeconds.Should().Be(endpoint?.CacheDirective?.ExpirationSeconds ?? def?.CacheDirective?.ExpirationSeconds ?? system.CacheDirective.ExpirationSeconds);
            request.CacheDirective.VaryHeaderValue.Should().Be(endpoint?.CacheDirective?.VaryHeaderValue ?? def?.CacheDirective?.VaryHeaderValue ?? system.CacheDirective.VaryHeaderValue);


            // -------------------
            // Cross Origin Configuration
            // -------------------
            request.CrossOriginConfig.AllowCredentials.Should().Be(endpoint?.CrossOriginConfig?.AllowCredentials ?? def?.CrossOriginConfig?.AllowCredentials ?? system.CrossOriginConfig.AllowCredentials);
            request.CrossOriginConfig.MaxAgeSeconds.Should().Be(endpoint?.CrossOriginConfig?.MaxAgeSeconds ?? def?.CrossOriginConfig?.MaxAgeSeconds ?? system.CrossOriginConfig.MaxAgeSeconds);


            request.CrossOriginConfig.AllowedHeaders.Count.Should().Be(endpoint?.CrossOriginConfig?.AllowedHeaders?.Count ?? def?.CrossOriginConfig?.AllowedHeaders?.Count ?? system.CrossOriginConfig.AllowedHeaders?.Count);
            for (int i = 0; i < request.CrossOriginConfig.AllowedHeaders.Count; i++)
            {
                request.CrossOriginConfig.AllowedHeaders[i].Should().Be(endpoint?.CrossOriginConfig?.AllowedHeaders?[i] ?? def?.CrossOriginConfig?.AllowedHeaders?[i] ?? system.CrossOriginConfig.AllowedHeaders[i]);
            }

            request.CrossOriginConfig.AllowedOrigins.Count.Should().Be(endpoint?.CrossOriginConfig?.AllowedOrigins?.Count ?? def?.CrossOriginConfig?.AllowedOrigins?.Count ?? system.CrossOriginConfig.AllowedOrigins?.Count);
            for (int i = 0; i < request.CrossOriginConfig.AllowedOrigins.Count; i++)
            {
                request.CrossOriginConfig.AllowedOrigins[i].Should().Be(endpoint?.CrossOriginConfig?.AllowedOrigins?[i] ?? def?.CrossOriginConfig?.AllowedOrigins?[i] ?? system.CrossOriginConfig.AllowedOrigins[i]);
            }

            request.CrossOriginConfig.ExposeHeaders.Count.Should().Be(endpoint?.CrossOriginConfig?.ExposeHeaders?.Count ?? def?.CrossOriginConfig?.ExposeHeaders?.Count ?? system.CrossOriginConfig.ExposeHeaders?.Count);
            for (int i = 0; i < request.CrossOriginConfig.ExposeHeaders.Count; i++)
            {
                request.CrossOriginConfig.ExposeHeaders[i].Should().Be(endpoint?.CrossOriginConfig?.ExposeHeaders?[i] ?? def?.CrossOriginConfig?.ExposeHeaders?[i] ?? system.CrossOriginConfig.ExposeHeaders[i]);
            }


            // ------------------------------
            // Validation Error Configuration
            // ------------------------------
            request.ValidationErrorConfiguration.UriBindingValueError.Should().Be(endpoint?.ValidationErrorConfiguration?.UriBindingValueError ?? def?.ValidationErrorConfiguration?.UriBindingValueError ?? system.ValidationErrorConfiguration.UriBindingValueError);
            request.ValidationErrorConfiguration.UriBindingError.Should().Be(endpoint?.ValidationErrorConfiguration?.UriBindingError ?? def?.ValidationErrorConfiguration?.UriBindingError ?? system.ValidationErrorConfiguration.UriBindingError);
            request.ValidationErrorConfiguration.RequestDeserializationError.Should().Be(endpoint?.ValidationErrorConfiguration?.RequestDeserializationError ?? def?.ValidationErrorConfiguration?.RequestDeserializationError ?? system.ValidationErrorConfiguration.RequestDeserializationError);
            request.ValidationErrorConfiguration.HttpStatusMode.Should().Be(endpoint?.ValidationErrorConfiguration?.HttpStatusMode ?? def?.ValidationErrorConfiguration?.HttpStatusMode ?? system.ValidationErrorConfiguration.HttpStatusMode);


            // making sure references are not the same
            request.Should().NotBeSameAs(system);
            request.ApiErrorResponseProvider(null).Should().NotBeSameAs(system.ApiErrorResponseProvider(null));
            request.AuthorizationProviders.Should().NotBeSameAs(system.AuthorizationProviders);
            request.CacheDirective.Should().NotBeSameAs(system.CacheDirective);
            request.CrossOriginConfig.Should().NotBeSameAs(system.CrossOriginConfig);
            request.LanguageSupport.Should().NotBeSameAs(system.LanguageSupport);
            request.ReadWriteConfiguration.Should().NotBeSameAs(system.ReadWriteConfiguration);
            request.ValidationErrorConfiguration.Should().NotBeSameAs(system.ValidationErrorConfiguration);
            request.Validators.Should().NotBeSameAs(system.Validators);
            request.RequestValidation.Should().NotBeSameAs(system.RequestValidation);
            request.AuthenticationProviders.Should().NotBeSameAs(system.AuthenticationProviders);
            request.PipelineComponents.Should().NotBeSameAs(system.PipelineComponents);
        }

        private ApiRequestInfo GetRequestInfo(string method = "GET")
        {
            return new ApiRequestInfo
            {
                Method = method,
                Path = "test/1/name"
            };
        }

        private IApiRoutingTable GetRoutingTable(IDeepSleepRequestConfiguration routeConfig)
        {
            var routingTable = new ApiRoutingTable();

            return routingTable.AddRoute(new DeepSleepRouteRegistration(
                template: "test/{id}/name",
                httpMethods: new[] { "GET" },
                controller: typeof(MockController),
                endpoint: nameof(MockController.Get),
                config: routeConfig));
        }
    }
}
