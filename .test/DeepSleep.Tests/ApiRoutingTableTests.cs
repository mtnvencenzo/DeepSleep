namespace DeepSleep.Tests
{
    using DeepSleep.Discovery;
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Xunit;

    public class ApiRoutingTableTests
    {
        [Fact]
        public void apiroutingtable___addroute_returns_null_for_null_registration()
        {
            var routingTable = new ApiRoutingTable();

            routingTable.AddRoute(null);

            routingTable.GetRoutes().Count.Should().Be(0);
        }

        [Fact]
        public void apiroutingtable___addroute_throws_for_duplicate_route()
        {
            var routingTable = new ApiRoutingTable();

            routingTable.AddRoute(new DeepSleepRouteRegistration(
                template: "/test",
                httpMethods: new[] { "GET" },
                controller: typeof(ApiRoutingTableTestController),
                endpoint: nameof(ApiRoutingTableTestController.Test)));

            var exception = Assert.Throws<Exception>(() =>
            {
                routingTable.AddRoute(new DeepSleepRouteRegistration(
                    template: "/test",
                    httpMethods: new[] { "get" },
                    controller: typeof(ApiRoutingTableTestController),
                    endpoint: nameof(ApiRoutingTableTestController.Test2)));
            });

            exception.Should().NotBeNull();
            exception.Message.Should().Be("Route 'GET /test' already has been added.");
        }

        [Fact]
        public void apiroutingtable___addroute_throws_for_duplication_template_method()
        {
            var routingTable = new ApiRoutingTable();

            routingTable.AddRoute(new DeepSleepRouteRegistration(
                    template: "/test",
                    httpMethods: new[] { "GET" },
                    controller: typeof(ApiRoutingTableTestController),
                    endpoint: nameof(ApiRoutingTableTestController.Test)));

            var exception = Assert.Throws<Exception>(() =>
            {
                routingTable.AddRoute(new DeepSleepRouteRegistration(
                    template: "/test",
                    httpMethods: new[] { "get" },
                    controller: typeof(ApiRoutingTableTestController),
                    endpoint: nameof(ApiRoutingTableTestController.Test2)));
            });

            exception.Should().NotBeNull();
            exception.Message.Should().Be("Route 'GET /test' already has been added.");
        }

        [Fact]
        public void apiroutingtable___addroute_throws_for_missing_method()
        {
            var routingTable = new ApiRoutingTable();

            var exception = Assert.Throws<MissingMethodException>(() =>
            {
                routingTable.AddRoute(new DeepSleepRouteRegistration(
                    template: "/test",
                    httpMethods: new[] { "get" },
                    controller: typeof(ApiRoutingTableTestController),
                    endpoint: "test10000"));
            });

            exception.Should().NotBeNull();
            exception.Message.Should().Be("Endpoint 'test10000' does not exist on controller 'DeepSleep.Tests.ApiRoutingTableTestController'");
        }

        [Fact]
        public void apiroutingtable___addroute_throws_for_ambiguous_method_overload()
        {
            var routingTable = new ApiRoutingTable();

            var exception = Assert.Throws<Exception>(() =>
            {
                routingTable.AddRoute(new DeepSleepRouteRegistration(
                    template: "/test",
                    httpMethods: new[] { "get" },
                    controller: typeof(ApiRoutingTableTestController),
                    endpoint: nameof(ApiRoutingTableTestController.Test3)));
            });

            exception.Should().NotBeNull();
            exception.Message.Should().Be("DeepSleep api routing does not support routes mapped to overloaded methods for api routes.  You must rename or move the method for route '[GET] test'.");
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.GetType().Should().Be(typeof(AmbiguousMatchException));
        }

        [Fact]
        public void apiroutingtable___addroutes_returns_null_for_null_registrations()
        {
            var routingTable = new ApiRoutingTable();

            routingTable.AddRoutes(null);

            routingTable.GetRoutes().Count.Should().Be(0);
        }

        [Fact]
        public void apiroutingtable___addroutes_success()
        {
            var routingTable = new ApiRoutingTable();

            routingTable.AddRoutes(new List<DeepSleepRouteRegistration>
            {
                new DeepSleepRouteRegistration(
                    template: "/test",
                    httpMethods: new[] { "GET" },
                    controller: typeof(ApiRoutingTableTestController),
                    endpoint: nameof(ApiRoutingTableTestController.Test)),

                new DeepSleepRouteRegistration(
                    template: "/test2",
                    httpMethods: new[] { "GET" },
                    controller: typeof(ApiRoutingTableTestController),
                    endpoint: nameof(ApiRoutingTableTestController.Test2))
            });

            routingTable.GetRoutes().Count.Should().Be(2);
        }
    }

    public class ApiRoutingTableTestController
    {
        public void Test()
        {
        }

        public void Test2()
        {
        }

        public void Test3()
        {
        }

        public void Test3(int overload)
        {
        }
    }
}
