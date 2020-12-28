namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Formatting;
    using DeepSleep.Pipeline;
    using DeepSleep.Tests.Mocks;
    using FluentAssertions;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

            var processed = await context.ProcessHttpRequestRouting(new DefaultApiRoutingTable(), null, null).ConfigureAwait(false);
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

            var processed = await context.ProcessHttpRequestRouting(null, new DefaultRouteResolver(), null).ConfigureAwait(false);
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

            var processed = await context.ProcessHttpRequestRouting(new DefaultApiRoutingTable(), new DefaultRouteResolver(), null).ConfigureAwait(false);
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
            var routeResolver = new DefaultRouteResolver();

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
            context.Routing.Route.Location.Endpoint.Should().Be(nameof(MockController.Get));
            context.Routing.Route.Location.Controller.Should().Be(typeof(MockController));
            context.Routing.Route.Location.HttpMethod.Should().Be("GET");
            context.Routing.Template.Should().NotBeNull();
            context.Routing.Template.Variables.Should().NotBeNull();
            context.Routing.Template.Variables.Should().HaveCount(1);
            context.Routing.Template.Template.Should().Be("test/{id}/name");
            context.Routing.Template.Locations.Should().NotBeNull();
            context.Routing.Template.Locations.Should().HaveCount(1);
            context.Routing.Template.Locations[0].Controller.Should().Be(typeof(MockController));
            context.Routing.Template.Locations[0].Endpoint.Should().Be(nameof(MockController.Get));
            context.Routing.Template.Locations[0].HttpMethod.Should().Be("GET");
            context.Configuration.Should().NotBeNull();
        }

        [Theory]
        [InlineData("HEAD")]
        [InlineData("heaD")]
        public async void returns_head_endpoint_for_head_request(string head)
        {
            var routingTable = GetRoutingTable(null);

            routingTable = routingTable.AddRoute(
                template: "test/{id}/name",
                httpMethod: head,
                controller: typeof(MockController),
                endpoint: nameof(MockController.Head),
                config: null);

            var routeResolver = new DefaultRouteResolver();

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
            context.Routing.Route.Location.Endpoint.Should().Be(nameof(MockController.Head));
            context.Routing.Route.Location.Controller.Should().Be(typeof(MockController));
            context.Routing.Route.Location.HttpMethod.Should().Be("HEAD");
            context.Routing.Template.Should().NotBeNull();
            context.Routing.Template.Variables.Should().NotBeNull();
            context.Routing.Template.Variables.Should().HaveCount(1);
            context.Routing.Template.Template.Should().Be("test/{id}/name");
            context.Routing.Template.Locations.Should().NotBeNull();
            context.Routing.Template.Locations.Should().HaveCount(2);
            context.Routing.Template.Locations[0].Controller.Should().Be(typeof(MockController));
            context.Routing.Template.Locations[0].Endpoint.Should().Be(nameof(MockController.Get));
            context.Routing.Template.Locations[0].HttpMethod.Should().Be("GET");
            context.Routing.Template.Locations[1].Controller.Should().Be(typeof(MockController));
            context.Routing.Template.Locations[1].Endpoint.Should().Be(nameof(MockController.Head));
            context.Routing.Template.Locations[1].HttpMethod.Should().Be("HEAD");
            context.Configuration.Should().NotBeNull();
        }


        // Configuration Tests
        // ----------------------

        [Fact]
        public async void request_config___returns_system_default_when_default_and_endpoint_are_null()
        {
            IApiRequestConfiguration defaultConfig = null;
            IApiRequestConfiguration endpointConfig = null;

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                AllowAnonymous = def
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                AllowAnonymous = endpoint
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                EnableHeadForGetRequests = def
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                EnableHeadForGetRequests = endpoint
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                AllowAnonymous = true
            };

            DefaultApiRequestConfiguration endpointConfig = null;

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            DefaultApiRequestConfiguration defaultConfig = null;

            DefaultApiRequestConfiguration endpointConfig = new DefaultApiRequestConfiguration
            {
                AllowAnonymous = true
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                AllowRequestBodyWhenNoModelDefined = def
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                AllowRequestBodyWhenNoModelDefined = endpoint
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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

            context.Configuration.AllowRequestBodyWhenNoModelDefined.Should().Be(expected);
        }

        [Theory]
        [InlineData(false, null, null)]
        [InlineData(true, true, null)]
        [InlineData(true, true, true)]
        [InlineData(true, null, true)]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        public async void request_config___deprecated_returns_expected(bool expected, bool? def, bool? endpoint)
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                Deprecated = def
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                Deprecated = endpoint
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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

            context.Configuration.Deprecated.Should().Be(expected);
        }

        [Theory]
        [InlineData(true, null, null)]
        [InlineData(false, false, null)]
        [InlineData(false, false, false)]
        [InlineData(false, null, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, true)]
        public async void request_config___includeRequestIdHeaderInResponse_returns_expected(bool expected, bool? def, bool? endpoint)
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                IncludeRequestIdHeaderInResponse = def
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                IncludeRequestIdHeaderInResponse = endpoint
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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

            context.Configuration.IncludeRequestIdHeaderInResponse.Should().Be(expected);
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

            var defaultConfig = new DefaultApiRequestConfiguration
            {
                ReadWriteConfiguration = new ApiReadWriteConfiguration
                {
                    AcceptHeaderOverride = def != null
                        ? new AcceptHeader(def)
                        : null
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                ReadWriteConfiguration = new ApiReadWriteConfiguration
                {
                    AcceptHeaderOverride = endpoint != null
                        ? new AcceptHeader(endpoint)
                        : null
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                ReadWriteConfiguration = new ApiReadWriteConfiguration
                {
                    ReadableMediaTypes = def != null
                        ? def.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        : null
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                ReadWriteConfiguration = new ApiReadWriteConfiguration
                {
                    ReadableMediaTypes = endpoint != null
                        ? endpoint.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        : null
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                ReadWriteConfiguration = new ApiReadWriteConfiguration
                {
                    WriteableMediaTypes = def != null
                        ? def.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        : null
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                ReadWriteConfiguration = new ApiReadWriteConfiguration
                {
                    WriteableMediaTypes = endpoint != null
                        ? endpoint.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        : null
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            var defaultMockFormatter = new Mock<IFormatStreamReaderWriter>();
            defaultMockFormatter.Setup(f => f.SupportsRead).Returns(def ?? false);
            defaultMockFormatter.Setup(f => f.SupportsWrite).Returns(def ?? false);
            defaultMockFormatter.Setup(f => f.SupportsPrettyPrint).Returns(def ?? false);

            var endpointMockFormatter = new Mock<IFormatStreamReaderWriter>();
            endpointMockFormatter.Setup(f => f.SupportsRead).Returns(endpoint ?? false);
            endpointMockFormatter.Setup(f => f.SupportsWrite).Returns(endpoint ?? false);
            endpointMockFormatter.Setup(f => f.SupportsPrettyPrint).Returns(endpoint ?? false);

            var defaultConfig = new DefaultApiRequestConfiguration
            {
                ReadWriteConfiguration = new ApiReadWriteConfiguration()
            };

            if (def.HasValue)
            {
                defaultConfig.ReadWriteConfiguration.ReaderResolver = (args) => Task.FromResult(new FormatterReadOverrides(new[] { defaultMockFormatter.Object }));
            }

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                ReadWriteConfiguration = new ApiReadWriteConfiguration()
            };

            if (endpoint.HasValue)
            {
                endpointConfig.ReadWriteConfiguration.ReaderResolver = (args) => Task.FromResult(new FormatterReadOverrides(new[] { endpointMockFormatter.Object }));
            }

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
                overrides.Formatters[0].SupportsPrettyPrint.Should().Be(expected.Value);
            }
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(true, null, true)]
        [InlineData(true, true, null)]
        [InlineData(false, true, false)]
        public async void request_config___readWriteConfiguration_writerresolver_returns_expected(bool? expected, bool? def, bool? endpoint)
        {
            var defaultMockFormatter = new Mock<IFormatStreamReaderWriter>();
            defaultMockFormatter.Setup(f => f.SupportsRead).Returns(def ?? false);
            defaultMockFormatter.Setup(f => f.SupportsWrite).Returns(def ?? false);
            defaultMockFormatter.Setup(f => f.SupportsPrettyPrint).Returns(def ?? false);

            var endpointMockFormatter = new Mock<IFormatStreamReaderWriter>();
            endpointMockFormatter.Setup(f => f.SupportsRead).Returns(endpoint ?? false);
            endpointMockFormatter.Setup(f => f.SupportsWrite).Returns(endpoint ?? false);
            endpointMockFormatter.Setup(f => f.SupportsPrettyPrint).Returns(endpoint ?? false);

            var defaultConfig = new DefaultApiRequestConfiguration
            {
                ReadWriteConfiguration = new ApiReadWriteConfiguration()
            };

            if (def.HasValue)
            {
                defaultConfig.ReadWriteConfiguration.WriterResolver = (args) => Task.FromResult(new FormatterWriteOverrides(new[] { defaultMockFormatter.Object }));
            }

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                ReadWriteConfiguration = new ApiReadWriteConfiguration()
            };

            if (endpoint.HasValue)
            {
                endpointConfig.ReadWriteConfiguration.WriterResolver = (args) => Task.FromResult(new FormatterWriteOverrides(new[] { endpointMockFormatter.Object }));
            }

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
                overrides.Formatters[0].SupportsPrettyPrint.Should().Be(expected.Value);
            }
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("en-US", "en-US", null)]
        [InlineData("en-US", "en-GB", "en-US")]
        [InlineData("en-US", null, "en-US")]
        public async void request_config___fallbacklanguage_returns_expected(string expected, string def, string endpoint)
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                FallBackLanguage = def
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                FallBackLanguage = endpoint
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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

            context.Configuration.FallBackLanguage.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(4, 4, null)]
        [InlineData(5, 4, 5)]
        [InlineData(6, null, 6)]
        public async void request_config___maxrequestlength_returns_expected(long? expected, long? def, long? endpoint)
        {
            var hasSet = false;

            var defaultConfig = new DefaultApiRequestConfiguration
            {
                MaxRequestLength = def
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                MaxRequestLength = endpoint
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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

            context.Configuration.MaxRequestLength.Should().Be(expected);

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
            var endpointConfig = new DefaultApiRequestConfiguration
            {
                MaxRequestLength = 10
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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

            context.Configuration.MaxRequestLength.Should().Be(10);
        }

        [Fact]
        public async void request_config___maxrequestlength_does_not_fail_with_maxlengthsetter_exception()
        {
            var hasSet = false;

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                MaxRequestLength = 10
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = GetRequestInfo(),
                Configuration = null,
                ConfigureMaxRequestLength = (length) => { 
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

            context.Configuration.MaxRequestLength.Should().Be(10);
            hasSet.Should().Be(true);
        }

        [Theory]
        [InlineData(0, null, null)]
        [InlineData(4, 4, null)]
        [InlineData(5, 4, 5)]
        [InlineData(6, null, 6)]
        public async void request_config___maxrequesturiLength_returns_expected(int expected, int? def, int? endpoint)
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                MaxRequestUriLength = def
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                MaxRequestUriLength = endpoint
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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

            context.Configuration.MaxRequestUriLength.Should().Be(expected);
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
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                RequireContentLengthOnRequestBodyRequests = def
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                RequireContentLengthOnRequestBodyRequests = endpoint
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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

            context.Configuration.RequireContentLengthOnRequestBodyRequests.Should().Be(expected);
        }

        [Fact]
        public async void request_config___endpoint_authconfig_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                AuthorizationConfig = null
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                AuthorizationConfig = new ResourceAuthorizationConfiguration
                {
                    Policy = "TestPolicy"
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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

            context.Configuration.AuthorizationConfig.Should().NotBeNull();
            context.Configuration.AuthorizationConfig.Should().NotBeSameAs(defaultConfig.AuthorizationConfig);
            context.Configuration.AuthorizationConfig.Should().NotBeSameAs(endpointConfig.AuthorizationConfig);
            context.Configuration.AuthorizationConfig.Policy.Should().Be("TestPolicy");
        }

        [Fact]
        public async void request_config___endpoint_authconfig_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                AuthorizationConfig = new ResourceAuthorizationConfiguration
                {
                    Policy = "TestPolicyDefault"
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                AuthorizationConfig = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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

            context.Configuration.AuthorizationConfig.Should().NotBeNull();
            context.Configuration.AuthorizationConfig.Should().NotBeSameAs(defaultConfig.AuthorizationConfig);
            context.Configuration.AuthorizationConfig.Should().NotBeSameAs(endpointConfig.AuthorizationConfig);
            context.Configuration.AuthorizationConfig.Policy.Should().Be("TestPolicyDefault");
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("endpoint", "default", "endpoint")]
        [InlineData("default", "default", null)]
        [InlineData("endpoint", null, "endpoint")]
        public async void request_config___authconfig_policy_returns_expected(string expected, string def, string endpoint)
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                AuthorizationConfig = new ResourceAuthorizationConfiguration
                {
                    Policy = def
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                AuthorizationConfig = new ResourceAuthorizationConfiguration
                {
                    Policy = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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

            context.Configuration.AuthorizationConfig.Should().NotBeNull();
            context.Configuration.AuthorizationConfig.Should().NotBeSameAs(defaultConfig.AuthorizationConfig);
            context.Configuration.AuthorizationConfig.Should().NotBeSameAs(endpointConfig.AuthorizationConfig);
            context.Configuration.AuthorizationConfig.Policy.Should().Be(expected);
        }

        [Fact]
        public async void request_config___endpoint_supportedauthenticationschemes_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                SupportedAuthenticationSchemes = null
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                SupportedAuthenticationSchemes = new string[] { "test1", "test2" }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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

            context.Configuration.SupportedAuthenticationSchemes.Should().NotBeNull();
            context.Configuration.SupportedAuthenticationSchemes.Should().NotBeSameAs(defaultConfig.SupportedAuthenticationSchemes);
            context.Configuration.SupportedAuthenticationSchemes.Should().NotBeSameAs(endpointConfig.SupportedAuthenticationSchemes);
            context.Configuration.SupportedAuthenticationSchemes.Should().HaveCount(2);
            context.Configuration.SupportedAuthenticationSchemes[0].Should().Be("test1");
            context.Configuration.SupportedAuthenticationSchemes[1].Should().Be("test2");
        }

        [Fact]
        public async void request_config___endpoint_supportedauthenticationschemes_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                SupportedAuthenticationSchemes = new string[] { "test1", "test2" }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                SupportedAuthenticationSchemes = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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

            context.Configuration.SupportedAuthenticationSchemes.Should().NotBeNull();
            context.Configuration.SupportedAuthenticationSchemes.Should().NotBeSameAs(defaultConfig.SupportedAuthenticationSchemes);
            context.Configuration.SupportedAuthenticationSchemes.Should().NotBeSameAs(endpointConfig.SupportedAuthenticationSchemes);
            context.Configuration.SupportedAuthenticationSchemes.Should().HaveCount(2);
            context.Configuration.SupportedAuthenticationSchemes[0].Should().Be("test1");
            context.Configuration.SupportedAuthenticationSchemes[1].Should().Be("test2");
        }

        [Fact]
        public async void request_config___endpoint_supportedauthenticationschemes_notnull_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                SupportedAuthenticationSchemes = new string[] { "test1", "test2" }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                SupportedAuthenticationSchemes = new string[] { "test3", "test4" }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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

            context.Configuration.SupportedAuthenticationSchemes.Should().NotBeNull();
            context.Configuration.SupportedAuthenticationSchemes.Should().NotBeSameAs(defaultConfig.SupportedAuthenticationSchemes);
            context.Configuration.SupportedAuthenticationSchemes.Should().NotBeSameAs(endpointConfig.SupportedAuthenticationSchemes);
            context.Configuration.SupportedAuthenticationSchemes.Should().HaveCount(2);
            context.Configuration.SupportedAuthenticationSchemes[0].Should().Be("test3");
            context.Configuration.SupportedAuthenticationSchemes[1].Should().Be("test4");
        }

        [Fact]
        public async void request_config___endpoint_supportedlanguages_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                SupportedLanguages = null
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                SupportedLanguages = new string[] { "test1", "test2" }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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

            context.Configuration.SupportedLanguages.Should().NotBeNull();
            context.Configuration.SupportedLanguages.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.Configuration.SupportedLanguages.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.Configuration.SupportedLanguages.Should().HaveCount(2);
            context.Configuration.SupportedLanguages[0].Should().Be("test1");
            context.Configuration.SupportedLanguages[1].Should().Be("test2");
        }

        [Fact]
        public async void request_config___endpoint_supportedlanguages_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                SupportedLanguages = new string[] { "test1", "test2" }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                SupportedLanguages = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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

            context.Configuration.SupportedLanguages.Should().NotBeNull();
            context.Configuration.SupportedLanguages.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.Configuration.SupportedLanguages.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.Configuration.SupportedLanguages.Should().HaveCount(2);
            context.Configuration.SupportedLanguages[0].Should().Be("test1");
            context.Configuration.SupportedLanguages[1].Should().Be("test2");
        }

        [Fact]
        public async void request_config___endpoint_supportedlanguages_notnull_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                SupportedLanguages = new string[] { "test1", "test2" }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                SupportedLanguages = new string[] { "test3", "test4" }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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

            context.Configuration.SupportedLanguages.Should().NotBeNull();
            context.Configuration.SupportedLanguages.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.Configuration.SupportedLanguages.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.Configuration.SupportedLanguages.Should().HaveCount(2);
            context.Configuration.SupportedLanguages[0].Should().Be("test3");
            context.Configuration.SupportedLanguages[1].Should().Be("test4");
        }

        [Fact]
        public async void request_config___endpoint_cachedirective_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                CacheDirective = null
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                CacheDirective = new HttpCacheDirective
                {
                    Cacheability = HttpCacheType.Cacheable,
                    CacheLocation = HttpCacheLocation.Public,
                    ExpirationSeconds = 100
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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
            context.Configuration.CacheDirective.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.Configuration.CacheDirective.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.Configuration.CacheDirective.Cacheability.Should().Be(HttpCacheType.Cacheable);
            context.Configuration.CacheDirective.CacheLocation.Should().Be(HttpCacheLocation.Public);
            context.Configuration.CacheDirective.ExpirationSeconds.Should().Be(100);
        }

        [Fact]
        public async void request_config___endpoint_cachedirective_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                CacheDirective = new HttpCacheDirective
                {
                    Cacheability = HttpCacheType.Cacheable,
                    CacheLocation = HttpCacheLocation.Public,
                    ExpirationSeconds = 100
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                CacheDirective = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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
            context.Configuration.CacheDirective.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.Configuration.CacheDirective.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.Configuration.CacheDirective.Cacheability.Should().Be(HttpCacheType.Cacheable);
            context.Configuration.CacheDirective.CacheLocation.Should().Be(HttpCacheLocation.Public);
            context.Configuration.CacheDirective.ExpirationSeconds.Should().Be(100);
        }

        [Theory]
        [InlineData(HttpCacheType.NoCache, null, null)]
        [InlineData(HttpCacheType.Cacheable, HttpCacheType.NoCache, HttpCacheType.Cacheable)]
        [InlineData(HttpCacheType.Cacheable, HttpCacheType.Cacheable, null)]
        [InlineData(HttpCacheType.Cacheable, null, HttpCacheType.Cacheable)]
        public async void request_config___cachedirective_cacheability_returns_expected(HttpCacheType expected, HttpCacheType? def, HttpCacheType? endpoint)
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                CacheDirective = new HttpCacheDirective
                {
                    Cacheability = def
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                CacheDirective = new HttpCacheDirective
                {
                    Cacheability = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            context.Configuration.CacheDirective.Should().NotBeSameAs(defaultConfig.AuthorizationConfig);
            context.Configuration.CacheDirective.Should().NotBeSameAs(endpointConfig.AuthorizationConfig);
            context.Configuration.CacheDirective.Cacheability.Should().Be(expected);
        }

        [Theory]
        [InlineData(HttpCacheLocation.Private, null, null)]
        [InlineData(HttpCacheLocation.Public, HttpCacheLocation.Private, HttpCacheLocation.Public)]
        [InlineData(HttpCacheLocation.Public, HttpCacheLocation.Public, null)]
        [InlineData(HttpCacheLocation.Public, null, HttpCacheLocation.Public)]
        public async void request_config___cachedirective_cachelocation_returns_expected(HttpCacheLocation expected, HttpCacheLocation? def, HttpCacheLocation? endpoint)
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                CacheDirective = new HttpCacheDirective
                {
                    CacheLocation = def
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                CacheDirective = new HttpCacheDirective
                {
                    CacheLocation = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            context.Configuration.CacheDirective.Should().NotBeSameAs(defaultConfig.AuthorizationConfig);
            context.Configuration.CacheDirective.Should().NotBeSameAs(endpointConfig.AuthorizationConfig);
            context.Configuration.CacheDirective.CacheLocation.Should().Be(expected);
        }

        [Theory]
        [InlineData(-1, null, null)]
        [InlineData(100, -1, 100)]
        [InlineData(100, 100, null)]
        [InlineData(100, null, 100)]
        public async void request_config___cachedirective_expirationseconds_returns_expected(int expected, int? def, int? endpoint)
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                CacheDirective = new HttpCacheDirective
                {
                    ExpirationSeconds = def
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                CacheDirective = new HttpCacheDirective
                {
                    ExpirationSeconds = endpoint
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            context.Configuration.CacheDirective.Should().NotBeSameAs(defaultConfig.AuthorizationConfig);
            context.Configuration.CacheDirective.Should().NotBeSameAs(endpointConfig.AuthorizationConfig);
            context.Configuration.CacheDirective.ExpirationSeconds.Should().Be(expected);
        }

        [Fact]
        public async void request_config___endpoint_crossoriginconfig_null_default_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                CrossOriginConfig = null
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                CrossOriginConfig = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.Configuration.CrossOriginConfig.AllowCredentials.Should().Be(true);
            context.Configuration.CrossOriginConfig.MaxAgeSeconds.Should().Be(600);
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
        public async void request_config___endpoint_crossoriginconfig_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                CrossOriginConfig = null
            };

            var endpointConfig = new DefaultApiRequestConfiguration
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
            var routeResolver = new DefaultRouteResolver();
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
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
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
        public async void request_config___endpoint_crossoriginconfig_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
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

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                CrossOriginConfig = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
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
        public async void request_config___endpoint_crossoriginconfig__default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
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

            var endpointConfig = new DefaultApiRequestConfiguration
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
            var routeResolver = new DefaultRouteResolver();

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
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.Configuration.CrossOriginConfig.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
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
        public async void request_config___endpoint_headervalidationconfig_null_default_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                HeaderValidationConfig = null
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                HeaderValidationConfig = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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

            context.Configuration.HeaderValidationConfig.Should().NotBeNull();
            context.Configuration.HeaderValidationConfig.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.Configuration.HeaderValidationConfig.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.Configuration.HeaderValidationConfig.MaxHeaderLength.Should().Be(0);
        }

        [Fact]
        public async void request_config___endpoint_headervalidationconfig_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                HeaderValidationConfig = null
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                HeaderValidationConfig = new ApiHeaderValidationConfiguration
                {
                    MaxHeaderLength = 200
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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

            context.Configuration.HeaderValidationConfig.Should().NotBeNull();
            context.Configuration.HeaderValidationConfig.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.Configuration.HeaderValidationConfig.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.Configuration.HeaderValidationConfig.MaxHeaderLength.Should().Be(200);
        }

        [Fact]
        public async void request_config___endpoint_headervalidationconfig_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                HeaderValidationConfig = new ApiHeaderValidationConfiguration
                {
                    MaxHeaderLength = 200
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                HeaderValidationConfig = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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

            context.Configuration.HeaderValidationConfig.Should().NotBeNull();
            context.Configuration.HeaderValidationConfig.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.Configuration.HeaderValidationConfig.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.Configuration.HeaderValidationConfig.MaxHeaderLength.Should().Be(200);
        }

        [Fact]
        public async void request_config___endpoint_headervalidationconfig_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                HeaderValidationConfig = new ApiHeaderValidationConfiguration
                {
                    MaxHeaderLength = 100
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                HeaderValidationConfig = new ApiHeaderValidationConfiguration
                {
                    MaxHeaderLength = 200
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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

            context.Configuration.HeaderValidationConfig.Should().NotBeNull();
            context.Configuration.HeaderValidationConfig.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.Configuration.HeaderValidationConfig.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.Configuration.HeaderValidationConfig.MaxHeaderLength.Should().Be(200);
        }

        [Fact]
        public async void request_config___endpoint_validationerrorconfig_null_default_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                ValidationErrorConfiguration = null
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                ValidationErrorConfiguration = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.Configuration.ValidationErrorConfiguration.UriBindingError.Should().Be("400.000001|'{paramName}' Is in an incorrect format and could not be bound.");
            context.Configuration.ValidationErrorConfiguration.UriBindingValueError.Should().Be("400.000002|Uri type conversion for '{paramName}' with value '{paramValue}' could not be converted to type {paramType}.");
        }

        [Fact]
        public async void request_config___endpoint_validationerrorconfig_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                ValidationErrorConfiguration = null
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "UriBindingError - Override",
                    UriBindingValueError = "UriBindingValueError - Override"
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.Configuration.ValidationErrorConfiguration.UriBindingError.Should().Be("UriBindingError - Override");
            context.Configuration.ValidationErrorConfiguration.UriBindingValueError.Should().Be("UriBindingValueError - Override");
        }

        [Fact]
        public async void request_config___endpoint_validationerrorconfig_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "UriBindingError - Override",
                    UriBindingValueError = "UriBindingValueError - Override"
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                ValidationErrorConfiguration = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
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
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.Configuration.ValidationErrorConfiguration.UriBindingError.Should().Be("UriBindingError - Override");
            context.Configuration.ValidationErrorConfiguration.UriBindingValueError.Should().Be("UriBindingValueError - Override");
        }

        [Fact]
        public async void request_config___endpoint_validationerrorconfig_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "UriBindingError - Override",
                    UriBindingValueError = "UriBindingValueError - Override"
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "UriBindingError - Override - Endpoint",
                    UriBindingValueError = "UriBindingValueError - Override - Endpoint"
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.Configuration.ValidationErrorConfiguration.UriBindingError.Should().Be("UriBindingError - Override - Endpoint");
            context.Configuration.ValidationErrorConfiguration.UriBindingValueError.Should().Be("UriBindingValueError - Override - Endpoint");
        }

        [Fact]
        public async void request_config___endpoint_validationerrorconfig_default_notnull_endpoint_mixed_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "UriBindingError - Override",
                    UriBindingValueError = "UriBindingValueError - Override"
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "UriBindingError - Override - Endpoint",
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

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
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.Configuration.ValidationErrorConfiguration.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.Configuration.ValidationErrorConfiguration.UriBindingError.Should().Be("UriBindingError - Override - Endpoint");
            context.Configuration.ValidationErrorConfiguration.UriBindingValueError.Should().Be("UriBindingValueError - Override");
        }

        private void AssertConfiguration(IApiRequestConfiguration request, IApiRequestConfiguration endpoint, IApiRequestConfiguration def)
        {
            var system = ApiRequestContext.GetDefaultRequestConfiguration();

            request.AllowAnonymous.Should().Be(endpoint?.AllowAnonymous ?? def?.AllowAnonymous ?? system.AllowAnonymous);
            request.AllowRequestBodyWhenNoModelDefined.Should().Be(endpoint?.AllowRequestBodyWhenNoModelDefined ?? def?.AllowRequestBodyWhenNoModelDefined ?? system.AllowRequestBodyWhenNoModelDefined);
            request.Deprecated.Should().Be(endpoint?.Deprecated ?? def?.Deprecated ?? system.Deprecated);
            request.FallBackLanguage.Should().Be(endpoint?.FallBackLanguage ?? def?.FallBackLanguage ?? system.FallBackLanguage);
            request.MaxRequestLength.Should().Be(endpoint?.MaxRequestLength ?? def?.MaxRequestLength ?? system.MaxRequestLength);
            request.MaxRequestUriLength.Should().Be(endpoint?.MaxRequestUriLength ?? def?.MaxRequestUriLength ?? system.MaxRequestUriLength);
            request.RequireContentLengthOnRequestBodyRequests.Should().Be(endpoint?.RequireContentLengthOnRequestBodyRequests ?? def?.RequireContentLengthOnRequestBodyRequests ?? system.RequireContentLengthOnRequestBodyRequests);
            request.IncludeRequestIdHeaderInResponse.Should().Be(endpoint?.IncludeRequestIdHeaderInResponse ?? def?.IncludeRequestIdHeaderInResponse ?? system.IncludeRequestIdHeaderInResponse);

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
            // Authorization Configuration
            // -------------------
            request.AuthorizationConfig.Policy.Should().Be(endpoint?.AuthorizationConfig?.Policy ?? def?.AuthorizationConfig?.Policy ?? system.AuthorizationConfig.Policy);


            // -------------------
            // Cache Directive Configuration
            // -------------------
            request.CacheDirective.Cacheability.Should().Be(endpoint?.CacheDirective?.Cacheability ?? def?.CacheDirective?.Cacheability ?? system.CacheDirective.Cacheability);
            request.CacheDirective.CacheLocation.Should().Be(endpoint?.CacheDirective?.CacheLocation ?? def?.CacheDirective?.CacheLocation ?? system.CacheDirective.CacheLocation);
            request.CacheDirective.ExpirationSeconds.Should().Be(endpoint?.CacheDirective?.ExpirationSeconds ?? def?.CacheDirective?.ExpirationSeconds ?? system.CacheDirective.ExpirationSeconds);


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


            // ------------------------
            // Header Validation Configuration
            // ------------------------
            request.HeaderValidationConfig.MaxHeaderLength.Should().Be(endpoint?.HeaderValidationConfig?.MaxHeaderLength ?? def?.HeaderValidationConfig?.MaxHeaderLength ?? system.HeaderValidationConfig.MaxHeaderLength);


            // --------------------------------
            // Supported Authentication Schemes
            // --------------------------------
            request.SupportedAuthenticationSchemes.Count.Should().Be(endpoint?.SupportedAuthenticationSchemes?.Count ?? def?.SupportedAuthenticationSchemes?.Count ?? system.SupportedAuthenticationSchemes.Count);
            for (int i = 0; i < request.SupportedAuthenticationSchemes.Count; i++)
            {
                request.SupportedAuthenticationSchemes[i].Should().Be(endpoint?.SupportedAuthenticationSchemes?[i] ?? def?.SupportedAuthenticationSchemes?[i] ?? system.SupportedAuthenticationSchemes[i]);
            }


            // --------------------------------
            // Supported Languages
            // --------------------------------
            request.SupportedLanguages.Count.Should().Be(endpoint?.SupportedLanguages?.Count ?? def?.SupportedLanguages?.Count ?? system.SupportedLanguages.Count);
            for (int i = 0; i < request.SupportedLanguages.Count; i++)
            {
                request.SupportedLanguages[i].Should().Be(endpoint?.SupportedLanguages?[i] ?? def?.SupportedLanguages?[i] ?? system.SupportedLanguages[i]);
            }

            // ------------------------------
            // Validation Error Configuration
            // ------------------------------
            request.ValidationErrorConfiguration.UriBindingValueError.Should().Be(endpoint?.ValidationErrorConfiguration?.UriBindingValueError ?? def?.ValidationErrorConfiguration?.UriBindingValueError ?? system.ValidationErrorConfiguration.UriBindingValueError);
            request.ValidationErrorConfiguration.UriBindingError.Should().Be(endpoint?.ValidationErrorConfiguration?.UriBindingError ?? def?.ValidationErrorConfiguration?.UriBindingError ?? system.ValidationErrorConfiguration.UriBindingError);


            // making sure references are not the same
            request.Should().NotBeSameAs(system);
            request.ApiErrorResponseProvider(null).Should().NotBeSameAs(system.ApiErrorResponseProvider(null));
            request.AuthorizationConfig.Should().NotBeSameAs(system.AuthorizationConfig);
            request.CacheDirective.Should().NotBeSameAs(system.CacheDirective);
            request.CrossOriginConfig.Should().NotBeSameAs(system.CrossOriginConfig);
            request.HeaderValidationConfig.Should().NotBeSameAs(system.HeaderValidationConfig);
            request.SupportedAuthenticationSchemes.Should().NotBeSameAs(system.SupportedAuthenticationSchemes);
            request.SupportedLanguages.Should().NotBeSameAs(system.SupportedLanguages);
            request.ReadWriteConfiguration.Should().NotBeSameAs(system.ReadWriteConfiguration);
            request.ValidationErrorConfiguration.Should().NotBeSameAs(system.ValidationErrorConfiguration);
        }

        private ApiRequestInfo GetRequestInfo(string method = "GET")
        {
            return new ApiRequestInfo
            {
                Method = method,
                Path = "test/1/name"
            };
        }

        private IApiRoutingTable GetRoutingTable(IApiRequestConfiguration routeConfig)
        {
            var routingTable = new DefaultApiRoutingTable();

            return routingTable.AddRoute(
                template: "test/{id}/name",
                httpMethod: "GET",
                controller: typeof(MockController),
                endpoint: nameof(MockController.Get),
                config: routeConfig);
        }
    }
}
