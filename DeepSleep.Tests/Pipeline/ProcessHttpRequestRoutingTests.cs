namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using DeepSleep.Tests.Mocks;
    using FluentAssertions;
    using System;
    using System.Threading;
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

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
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

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().BeNull();
            context.RouteInfo.TemplateInfo.Should().BeNull();
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

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().BeNull();
            context.RouteInfo.TemplateInfo.Should().BeNull();
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

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().BeNull();
            context.RouteInfo.TemplateInfo.Should().BeNull();
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
                RequestInfo = GetRequestInfo(method: head),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.RoutingItem.HttpMethod.Should().Be("GET");
            context.RouteInfo.RoutingItem.RouteVariables.Should().NotBeNull();
            context.RouteInfo.RoutingItem.RouteVariables.Should().HaveCount(1);
            context.RouteInfo.RoutingItem.RouteVariables.Should().ContainKey("id");
            context.RouteInfo.RoutingItem.RouteVariables["id"].Should().Be("1");
            context.RouteInfo.RoutingItem.VariablesList.Should().NotBeNull();
            context.RouteInfo.RoutingItem.VariablesList.Should().HaveCount(1);
            context.RouteInfo.RoutingItem.EndpointLocation.Should().NotBeNull();
            context.RouteInfo.RoutingItem.EndpointLocation.Endpoint.Should().Be(nameof(MockController.Get));
            context.RouteInfo.RoutingItem.EndpointLocation.Controller.Should().Be(typeof(MockController));
            context.RouteInfo.RoutingItem.EndpointLocation.HttpMethod.Should().Be("GET");
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.VariablesList.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.VariablesList.Should().HaveCount(1);
            context.RouteInfo.TemplateInfo.Template.Should().Be("test/{id}/name");
            context.RouteInfo.TemplateInfo.EndpointLocations.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.EndpointLocations.Should().HaveCount(1);
            context.RouteInfo.TemplateInfo.EndpointLocations[0].Controller.Should().Be(typeof(MockController));
            context.RouteInfo.TemplateInfo.EndpointLocations[0].Endpoint.Should().Be(nameof(MockController.Get));
            context.RouteInfo.TemplateInfo.EndpointLocations[0].HttpMethod.Should().Be("GET");
            context.RequestConfig.Should().NotBeNull();
        }

        [Theory]
        [InlineData("HEAD")]
        [InlineData("heaD")]
        public async void returns_head_endpoint_for_head_request(string head)
        {
            var routingTable = GetRoutingTable(null);

            routingTable = routingTable.AddRoute(
                name: "HEAD_test/{id}/name",
                template: "test/{id}/name",
                httpMethod: head,
                controller: typeof(MockController),
                endpoint: nameof(MockController.Head),
                config: null);

            var routeResolver = new DefaultRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = GetRequestInfo(method: head),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.RoutingItem.HttpMethod.Should().Be("HEAD");
            context.RouteInfo.RoutingItem.RouteVariables.Should().NotBeNull();
            context.RouteInfo.RoutingItem.RouteVariables.Should().HaveCount(1);
            context.RouteInfo.RoutingItem.RouteVariables.Should().ContainKey("id");
            context.RouteInfo.RoutingItem.RouteVariables["id"].Should().Be("1");
            context.RouteInfo.RoutingItem.VariablesList.Should().NotBeNull();
            context.RouteInfo.RoutingItem.VariablesList.Should().HaveCount(1);
            context.RouteInfo.RoutingItem.EndpointLocation.Should().NotBeNull();
            context.RouteInfo.RoutingItem.EndpointLocation.Endpoint.Should().Be(nameof(MockController.Head));
            context.RouteInfo.RoutingItem.EndpointLocation.Controller.Should().Be(typeof(MockController));
            context.RouteInfo.RoutingItem.EndpointLocation.HttpMethod.Should().Be("HEAD");
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.VariablesList.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.VariablesList.Should().HaveCount(1);
            context.RouteInfo.TemplateInfo.Template.Should().Be("test/{id}/name");
            context.RouteInfo.TemplateInfo.EndpointLocations.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.EndpointLocations.Should().HaveCount(2);
            context.RouteInfo.TemplateInfo.EndpointLocations[0].Controller.Should().Be(typeof(MockController));
            context.RouteInfo.TemplateInfo.EndpointLocations[0].Endpoint.Should().Be(nameof(MockController.Get));
            context.RouteInfo.TemplateInfo.EndpointLocations[0].HttpMethod.Should().Be("GET");
            context.RouteInfo.TemplateInfo.EndpointLocations[1].Controller.Should().Be(typeof(MockController));
            context.RouteInfo.TemplateInfo.EndpointLocations[1].Endpoint.Should().Be(nameof(MockController.Head));
            context.RouteInfo.TemplateInfo.EndpointLocations[1].HttpMethod.Should().Be("HEAD");
            context.RequestConfig.Should().NotBeNull();
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.AllowAnonymous.Should().Be(expected);
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.AllowAnonymous.Should().Be(true);
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.AllowAnonymous.Should().Be(true);
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.AllowRequestBodyWhenNoModelDefined.Should().Be(expected);
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.Deprecated.Should().Be(expected);
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.FallBackLanguage.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, null, null)]
        [InlineData(4, 4, null)]
        [InlineData(5, 4, 5)]
        [InlineData(6, null, 6)]
        public async void request_config___maxrequestlength_returns_expected(int expected, int? def, int? endpoint)
        {
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.MaxRequestLength.Should().Be(expected);
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.MaxRequestUriLength.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, null, null)]
        [InlineData(4, 4, null)]
        [InlineData(5, 4, 5)]
        [InlineData(6, null, 6)]
        public async void request_config___minrequestlength_returns_expected(int expected, int? def, int? endpoint)
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                MinRequestLength = def
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                MinRequestLength = endpoint
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.MinRequestLength.Should().Be(expected);
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.RequireContentLengthOnRequestBodyRequests.Should().Be(expected);
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.AuthorizationConfig.Should().NotBeNull();
            context.RequestConfig.AuthorizationConfig.Should().NotBeSameAs(defaultConfig.AuthorizationConfig);
            context.RequestConfig.AuthorizationConfig.Should().NotBeSameAs(endpointConfig.AuthorizationConfig);
            context.RequestConfig.AuthorizationConfig.Policy.Should().Be("TestPolicy");
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.AuthorizationConfig.Should().NotBeNull();
            context.RequestConfig.AuthorizationConfig.Should().NotBeSameAs(defaultConfig.AuthorizationConfig);
            context.RequestConfig.AuthorizationConfig.Should().NotBeSameAs(endpointConfig.AuthorizationConfig);
            context.RequestConfig.AuthorizationConfig.Policy.Should().Be("TestPolicyDefault");
        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData("endpoint", "default", "endpoint")]
        [InlineData("default", "default", null)]
        [InlineData("endpoint", null, "endpoint")]
        public async void request_config___authconfig_policy_returns_expected(string expected, string? def, string? endpoint)
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.AuthorizationConfig.Should().NotBeNull();
            context.RequestConfig.AuthorizationConfig.Should().NotBeSameAs(defaultConfig.AuthorizationConfig);
            context.RequestConfig.AuthorizationConfig.Should().NotBeSameAs(endpointConfig.AuthorizationConfig);
            context.RequestConfig.AuthorizationConfig.Policy.Should().Be(expected);
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.SupportedAuthenticationSchemes.Should().NotBeNull();
            context.RequestConfig.SupportedAuthenticationSchemes.Should().NotBeSameAs(defaultConfig.SupportedAuthenticationSchemes);
            context.RequestConfig.SupportedAuthenticationSchemes.Should().NotBeSameAs(endpointConfig.SupportedAuthenticationSchemes);
            context.RequestConfig.SupportedAuthenticationSchemes.Should().HaveCount(2);
            context.RequestConfig.SupportedAuthenticationSchemes[0].Should().Be("test1");
            context.RequestConfig.SupportedAuthenticationSchemes[1].Should().Be("test2");
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.SupportedAuthenticationSchemes.Should().NotBeNull();
            context.RequestConfig.SupportedAuthenticationSchemes.Should().NotBeSameAs(defaultConfig.SupportedAuthenticationSchemes);
            context.RequestConfig.SupportedAuthenticationSchemes.Should().NotBeSameAs(endpointConfig.SupportedAuthenticationSchemes);
            context.RequestConfig.SupportedAuthenticationSchemes.Should().HaveCount(2);
            context.RequestConfig.SupportedAuthenticationSchemes[0].Should().Be("test1");
            context.RequestConfig.SupportedAuthenticationSchemes[1].Should().Be("test2");
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.SupportedAuthenticationSchemes.Should().NotBeNull();
            context.RequestConfig.SupportedAuthenticationSchemes.Should().NotBeSameAs(defaultConfig.SupportedAuthenticationSchemes);
            context.RequestConfig.SupportedAuthenticationSchemes.Should().NotBeSameAs(endpointConfig.SupportedAuthenticationSchemes);
            context.RequestConfig.SupportedAuthenticationSchemes.Should().HaveCount(2);
            context.RequestConfig.SupportedAuthenticationSchemes[0].Should().Be("test3");
            context.RequestConfig.SupportedAuthenticationSchemes[1].Should().Be("test4");
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.SupportedLanguages.Should().NotBeNull();
            context.RequestConfig.SupportedLanguages.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.RequestConfig.SupportedLanguages.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.RequestConfig.SupportedLanguages.Should().HaveCount(2);
            context.RequestConfig.SupportedLanguages[0].Should().Be("test1");
            context.RequestConfig.SupportedLanguages[1].Should().Be("test2");
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.SupportedLanguages.Should().NotBeNull();
            context.RequestConfig.SupportedLanguages.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.RequestConfig.SupportedLanguages.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.RequestConfig.SupportedLanguages.Should().HaveCount(2);
            context.RequestConfig.SupportedLanguages[0].Should().Be("test1");
            context.RequestConfig.SupportedLanguages[1].Should().Be("test2");
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.SupportedLanguages.Should().NotBeNull();
            context.RequestConfig.SupportedLanguages.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.RequestConfig.SupportedLanguages.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.RequestConfig.SupportedLanguages.Should().HaveCount(2);
            context.RequestConfig.SupportedLanguages[0].Should().Be("test3");
            context.RequestConfig.SupportedLanguages[1].Should().Be("test4");
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.CacheDirective.Should().NotBeNull();
            context.RequestConfig.CacheDirective.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.RequestConfig.CacheDirective.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.RequestConfig.CacheDirective.Cacheability.Should().Be(HttpCacheType.Cacheable);
            context.RequestConfig.CacheDirective.CacheLocation.Should().Be(HttpCacheLocation.Public);
            context.RequestConfig.CacheDirective.ExpirationSeconds.Should().Be(100);
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.CacheDirective.Should().NotBeNull();
            context.RequestConfig.CacheDirective.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.RequestConfig.CacheDirective.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.RequestConfig.CacheDirective.Cacheability.Should().Be(HttpCacheType.Cacheable);
            context.RequestConfig.CacheDirective.CacheLocation.Should().Be(HttpCacheLocation.Public);
            context.RequestConfig.CacheDirective.ExpirationSeconds.Should().Be(100);
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.CacheDirective.Should().NotBeNull();
            context.RequestConfig.CacheDirective.Should().NotBeSameAs(defaultConfig.AuthorizationConfig);
            context.RequestConfig.CacheDirective.Should().NotBeSameAs(endpointConfig.AuthorizationConfig);
            context.RequestConfig.CacheDirective.Cacheability.Should().Be(expected);
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.CacheDirective.Should().NotBeNull();
            context.RequestConfig.CacheDirective.Should().NotBeSameAs(defaultConfig.AuthorizationConfig);
            context.RequestConfig.CacheDirective.Should().NotBeSameAs(endpointConfig.AuthorizationConfig);
            context.RequestConfig.CacheDirective.CacheLocation.Should().Be(expected);
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.CacheDirective.Should().NotBeNull();
            context.RequestConfig.CacheDirective.Should().NotBeSameAs(defaultConfig.AuthorizationConfig);
            context.RequestConfig.CacheDirective.Should().NotBeSameAs(endpointConfig.AuthorizationConfig);
            context.RequestConfig.CacheDirective.ExpirationSeconds.Should().Be(expected);
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.CrossOriginConfig.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.RequestConfig.CrossOriginConfig.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.RequestConfig.CrossOriginConfig.AllowCredentials.Should().Be(true);
            context.RequestConfig.CrossOriginConfig.AllowedHeaders.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.AllowedHeaders.Should().HaveCount(1);
            context.RequestConfig.CrossOriginConfig.AllowedHeaders[0].Should().Be("*");
            context.RequestConfig.CrossOriginConfig.AllowedOrigins.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.AllowedOrigins.Should().HaveCount(1);
            context.RequestConfig.CrossOriginConfig.AllowedOrigins[0].Should().Be("*");
            context.RequestConfig.CrossOriginConfig.ExposeHeaders.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.ExposeHeaders.Should().HaveCount(0);
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
                CrossOriginConfig = new CrossOriginConfiguration
                {
                    AllowCredentials = false,
                    AllowedHeaders = new string[] { "Header1", "Header2" },
                    AllowedOrigins = new string[] { "http://origin1.ut", "http://origin12.ut" },
                    ExposeHeaders = new string[] { "ExHeader1", "ExHeader2" }
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.CrossOriginConfig.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.RequestConfig.CrossOriginConfig.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.RequestConfig.CrossOriginConfig.AllowCredentials.Should().Be(false);
            context.RequestConfig.CrossOriginConfig.AllowedHeaders.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.AllowedHeaders.Should().HaveCount(2);
            context.RequestConfig.CrossOriginConfig.AllowedHeaders[0].Should().Be("Header1");
            context.RequestConfig.CrossOriginConfig.AllowedHeaders[1].Should().Be("Header2");
            context.RequestConfig.CrossOriginConfig.AllowedOrigins.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.AllowedOrigins.Should().HaveCount(2);
            context.RequestConfig.CrossOriginConfig.AllowedOrigins[0].Should().Be("http://origin1.ut");
            context.RequestConfig.CrossOriginConfig.AllowedOrigins[1].Should().Be("http://origin12.ut");
            context.RequestConfig.CrossOriginConfig.ExposeHeaders.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.ExposeHeaders.Should().HaveCount(2);
            context.RequestConfig.CrossOriginConfig.ExposeHeaders[0].Should().Be("ExHeader1");
            context.RequestConfig.CrossOriginConfig.ExposeHeaders[1].Should().Be("ExHeader2");
        }

        [Fact]
        public async void request_config___endpoint_crossoriginconfig_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                CrossOriginConfig = new CrossOriginConfiguration
                {
                    AllowCredentials = false,
                    AllowedHeaders = new string[] { "Header1", "Header2" },
                    AllowedOrigins = new string[] { "http://origin1.ut", "http://origin12.ut" },
                    ExposeHeaders = new string[] { "ExHeader1", "ExHeader2" }
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.CrossOriginConfig.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.RequestConfig.CrossOriginConfig.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.RequestConfig.CrossOriginConfig.AllowCredentials.Should().Be(false);
            context.RequestConfig.CrossOriginConfig.AllowedHeaders.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.AllowedHeaders.Should().HaveCount(2);
            context.RequestConfig.CrossOriginConfig.AllowedHeaders[0].Should().Be("Header1");
            context.RequestConfig.CrossOriginConfig.AllowedHeaders[1].Should().Be("Header2");
            context.RequestConfig.CrossOriginConfig.AllowedOrigins.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.AllowedOrigins.Should().HaveCount(2);
            context.RequestConfig.CrossOriginConfig.AllowedOrigins[0].Should().Be("http://origin1.ut");
            context.RequestConfig.CrossOriginConfig.AllowedOrigins[1].Should().Be("http://origin12.ut");
            context.RequestConfig.CrossOriginConfig.ExposeHeaders.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.ExposeHeaders.Should().HaveCount(2);
            context.RequestConfig.CrossOriginConfig.ExposeHeaders[0].Should().Be("ExHeader1");
            context.RequestConfig.CrossOriginConfig.ExposeHeaders[1].Should().Be("ExHeader2");
        }

        [Fact]
        public async void request_config___endpoint_crossoriginconfig__default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                CrossOriginConfig = new CrossOriginConfiguration
                {
                    AllowCredentials = true,
                    AllowedHeaders = new string[] { "D:Header1", "D:Header2" },
                    AllowedOrigins = new string[] { "http://dorigin1.ut", "http://dorigin12.ut" },
                    ExposeHeaders = new string[] { "D:ExHeader1", "D:ExHeader2" }
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                CrossOriginConfig = new CrossOriginConfiguration
                {
                    AllowCredentials = false,
                    AllowedHeaders = new string[] { "E:Header1", "E:Header2" },
                    AllowedOrigins = new string[] { "http://eorigin1.ut", "http://eorigin12.ut" },
                    ExposeHeaders = new string[] { "E:ExHeader1", "E:ExHeader2" }
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.CrossOriginConfig.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.Should().NotBeSameAs(defaultConfig.SupportedLanguages);
            context.RequestConfig.CrossOriginConfig.Should().NotBeSameAs(endpointConfig.SupportedLanguages);
            context.RequestConfig.CrossOriginConfig.AllowCredentials.Should().Be(false);
            context.RequestConfig.CrossOriginConfig.AllowedHeaders.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.AllowedHeaders.Should().HaveCount(2);
            context.RequestConfig.CrossOriginConfig.AllowedHeaders[0].Should().Be("E:Header1");
            context.RequestConfig.CrossOriginConfig.AllowedHeaders[1].Should().Be("E:Header2");
            context.RequestConfig.CrossOriginConfig.AllowedOrigins.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.AllowedOrigins.Should().HaveCount(2);
            context.RequestConfig.CrossOriginConfig.AllowedOrigins[0].Should().Be("http://eorigin1.ut");
            context.RequestConfig.CrossOriginConfig.AllowedOrigins[1].Should().Be("http://eorigin12.ut");
            context.RequestConfig.CrossOriginConfig.ExposeHeaders.Should().NotBeNull();
            context.RequestConfig.CrossOriginConfig.ExposeHeaders.Should().HaveCount(2);
            context.RequestConfig.CrossOriginConfig.ExposeHeaders[0].Should().Be("E:ExHeader1");
            context.RequestConfig.CrossOriginConfig.ExposeHeaders[1].Should().Be("E:ExHeader2");
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.HeaderValidationConfig.Should().NotBeNull();
            context.RequestConfig.HeaderValidationConfig.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.RequestConfig.HeaderValidationConfig.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.RequestConfig.HeaderValidationConfig.MaxHeaderLength.Should().Be(0);
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.HeaderValidationConfig.Should().NotBeNull();
            context.RequestConfig.HeaderValidationConfig.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.RequestConfig.HeaderValidationConfig.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.RequestConfig.HeaderValidationConfig.MaxHeaderLength.Should().Be(200);
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
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.HeaderValidationConfig.Should().NotBeNull();
            context.RequestConfig.HeaderValidationConfig.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.RequestConfig.HeaderValidationConfig.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.RequestConfig.HeaderValidationConfig.MaxHeaderLength.Should().Be(200);
        }

        [Fact]
        public async void request_config___endpointheadervalidationconfig_default_notnull_returns_expected()
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
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.HeaderValidationConfig.Should().NotBeNull();
            context.RequestConfig.HeaderValidationConfig.Should().NotBeSameAs(defaultConfig.HeaderValidationConfig);
            context.RequestConfig.HeaderValidationConfig.Should().NotBeSameAs(endpointConfig.HeaderValidationConfig);
            context.RequestConfig.HeaderValidationConfig.MaxHeaderLength.Should().Be(200);
        }

        [Fact]
        public async void request_config___endpoint_httpconfig_null_default_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                HttpConfig = null
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                HttpConfig = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.HttpConfig.Should().NotBeNull();
            context.RequestConfig.HttpConfig.Should().NotBeSameAs(defaultConfig.HttpConfig);
            context.RequestConfig.HttpConfig.Should().NotBeSameAs(endpointConfig.HttpConfig);
            context.RequestConfig.HttpConfig.RequireSSL.Should().Be(false);
            context.RequestConfig.HttpConfig.SupportedVersions.Should().NotBeNull();
            context.RequestConfig.HttpConfig.SupportedVersions.Should().HaveCount(5);
            context.RequestConfig.HttpConfig.SupportedVersions[0].Should().Be("http/1.1");
            context.RequestConfig.HttpConfig.SupportedVersions[1].Should().Be("http/1.2");
            context.RequestConfig.HttpConfig.SupportedVersions[2].Should().Be("http/2");
            context.RequestConfig.HttpConfig.SupportedVersions[3].Should().Be("http/2.0");
            context.RequestConfig.HttpConfig.SupportedVersions[4].Should().Be("http/2.1");
        }

        [Fact]
        public async void request_config___endpoint_httpconfig_notnull_default_null_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                HttpConfig = null
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                HttpConfig = new ApiHttpConfiguration
                {
                    RequireSSL = true,
                    SupportedVersions = new string[] { "test1/1.0", "test1/2.0" }
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.HttpConfig.Should().NotBeNull();
            context.RequestConfig.HttpConfig.Should().NotBeSameAs(defaultConfig.HttpConfig);
            context.RequestConfig.HttpConfig.Should().NotBeSameAs(endpointConfig.HttpConfig);
            context.RequestConfig.HttpConfig.RequireSSL.Should().Be(true);
            context.RequestConfig.HttpConfig.SupportedVersions.Should().NotBeNull();
            context.RequestConfig.HttpConfig.SupportedVersions.Should().HaveCount(2);
            context.RequestConfig.HttpConfig.SupportedVersions[0].Should().Be("test1/1.0");
            context.RequestConfig.HttpConfig.SupportedVersions[1].Should().Be("test1/2.0");
        }

        [Fact]
        public async void request_config___endpoint_httpconfig_null_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                HttpConfig = new ApiHttpConfiguration
                {
                    RequireSSL = true,
                    SupportedVersions = new string[] { "test1/1.0", "test1/2.0" }
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                HttpConfig = null
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = GetRequestInfo()
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.HttpConfig.Should().NotBeNull();
            context.RequestConfig.HttpConfig.Should().NotBeSameAs(defaultConfig.HttpConfig);
            context.RequestConfig.HttpConfig.Should().NotBeSameAs(endpointConfig.HttpConfig);
            context.RequestConfig.HttpConfig.RequireSSL.Should().Be(true);
            context.RequestConfig.HttpConfig.SupportedVersions.Should().NotBeNull();
            context.RequestConfig.HttpConfig.SupportedVersions.Should().HaveCount(2);
            context.RequestConfig.HttpConfig.SupportedVersions[0].Should().Be("test1/1.0");
            context.RequestConfig.HttpConfig.SupportedVersions[1].Should().Be("test1/2.0");
        }

        [Fact]
        public async void request_config___endpoint_httpconfig_default_notnull_returns_expected()
        {
            var defaultConfig = new DefaultApiRequestConfiguration
            {
                HttpConfig = new ApiHttpConfiguration
                {
                    RequireSSL = false,
                    SupportedVersions = new string[] { "test1/4.0", "test1/5.0" }
                }
            };

            var endpointConfig = new DefaultApiRequestConfiguration
            {
                HttpConfig = new ApiHttpConfiguration
                {
                    RequireSSL = true,
                    SupportedVersions = new string[] { "test1/1.0", "test1/2.0" }
                }
            };

            var routingTable = GetRoutingTable(endpointConfig);
            var routeResolver = new DefaultRouteResolver();

            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                RequestInfo = GetRequestInfo(),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpRequestRouting(routingTable, routeResolver, defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().NotBeNull();
            context.RouteInfo.TemplateInfo.Should().NotBeNull();
            context.RequestConfig.Should().NotBeNull();

            // Assert the request's configuration
            AssertConfiguration(context.RequestConfig, endpointConfig, defaultConfig);

            context.RequestConfig.HttpConfig.Should().NotBeNull();
            context.RequestConfig.HttpConfig.Should().NotBeSameAs(defaultConfig.HttpConfig);
            context.RequestConfig.HttpConfig.Should().NotBeSameAs(endpointConfig.HttpConfig);
            context.RequestConfig.HttpConfig.RequireSSL.Should().Be(true);
            context.RequestConfig.HttpConfig.SupportedVersions.Should().NotBeNull();
            context.RequestConfig.HttpConfig.SupportedVersions.Should().HaveCount(2);
            context.RequestConfig.HttpConfig.SupportedVersions[0].Should().Be("test1/1.0");
            context.RequestConfig.HttpConfig.SupportedVersions[1].Should().Be("test1/2.0");
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
            request.MinRequestLength.Should().Be(endpoint?.MinRequestLength ?? def?.MinRequestLength ?? system.MinRequestLength);
            request.RequireContentLengthOnRequestBodyRequests.Should().Be(endpoint?.RequireContentLengthOnRequestBodyRequests ?? def?.RequireContentLengthOnRequestBodyRequests ?? system.RequireContentLengthOnRequestBodyRequests);

            // -------------------
            // Authorization Config
            // -------------------
            request.AuthorizationConfig.Policy.Should().Be(endpoint?.AuthorizationConfig?.Policy ?? def?.AuthorizationConfig?.Policy ?? system.AuthorizationConfig.Policy);


            // -------------------
            // Cross Origin Config
            // -------------------
            request.CacheDirective.Cacheability.Should().Be(endpoint?.CacheDirective?.Cacheability ?? def?.CacheDirective?.Cacheability ?? system.CacheDirective.Cacheability);
            request.CacheDirective.CacheLocation.Should().Be(endpoint?.CacheDirective?.CacheLocation ?? def?.CacheDirective?.CacheLocation ?? system.CacheDirective.CacheLocation);
            request.CacheDirective.ExpirationSeconds.Should().Be(endpoint?.CacheDirective?.ExpirationSeconds ?? def?.CacheDirective?.ExpirationSeconds ?? system.CacheDirective.ExpirationSeconds);


            // -------------------
            // Cross Origin Config
            // -------------------
            request.CrossOriginConfig.AllowCredentials.Should().Be(endpoint?.CrossOriginConfig?.AllowCredentials ?? def?.CrossOriginConfig?.AllowCredentials ?? system.CrossOriginConfig.AllowCredentials);
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
            // Header Validation Config
            // ------------------------
            request.HeaderValidationConfig.MaxHeaderLength.Should().Be(endpoint?.HeaderValidationConfig?.MaxHeaderLength ?? def?.HeaderValidationConfig?.MaxHeaderLength ?? system.HeaderValidationConfig.MaxHeaderLength);


            // ------------------------
            // Http Config
            // ------------------------
            request.HttpConfig.RequireSSL.Should().Be(endpoint?.HttpConfig?.RequireSSL ?? def?.HttpConfig?.RequireSSL ?? system.HttpConfig.RequireSSL);
            request.HttpConfig.SupportedVersions.Count.Should().Be(endpoint?.HttpConfig?.SupportedVersions?.Count ?? def?.HttpConfig?.SupportedVersions?.Count ?? system.HttpConfig.SupportedVersions.Count);
            for (int i = 0; i < request.HttpConfig.SupportedVersions.Count; i++)
            {
                request.HttpConfig.SupportedVersions[i].Should().Be(endpoint?.HttpConfig?.SupportedVersions?[i] ?? def?.HttpConfig?.SupportedVersions?[i] ?? system.HttpConfig.SupportedVersions[i]);
            }


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


            // making sure references are not the same
            request.Should().NotBeSameAs(system);
            request.ApiErrorResponseProvider(null).Should().NotBeSameAs(system.ApiErrorResponseProvider(null));
            request.AuthorizationConfig.Should().NotBeSameAs(system.AuthorizationConfig);
            request.CacheDirective.Should().NotBeSameAs(system.CacheDirective);
            request.CrossOriginConfig.Should().NotBeSameAs(system.CrossOriginConfig);
            request.HeaderValidationConfig.Should().NotBeSameAs(system.HeaderValidationConfig);
            request.SupportedAuthenticationSchemes.Should().NotBeSameAs(system.SupportedAuthenticationSchemes);
            request.SupportedLanguages.Should().NotBeSameAs(system.SupportedLanguages);
            request.HttpConfig.Should().NotBeSameAs(system.HttpConfig);
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
                name: "GET_test/{id}/name",
                template: "test/{id}/name",
                httpMethod: "GET",
                controller: typeof(MockController),
                endpoint: nameof(MockController.Get),
                config: routeConfig);
        }
    }
}
