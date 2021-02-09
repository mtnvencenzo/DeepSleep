namespace DeepSleep.Tests.Discovery
{
    using DeepSleep.Discovery;
    using FluentAssertions;
    using System;
    using Xunit;

    public class DeepSleepRouteRegistrationTests
    {
        [Fact]
        public void routeregistration___should_throw_for_null_controller()
        {
            var exception = Assert.Throws<Exception>(() =>
            {
                new DeepSleepRouteRegistration(
                    template: "test",
                    httpMethods: new[] { "GET" },
                    controller: null,
                    endpoint: "TEST");
            });

            exception.Should().NotBeNull();
            exception.Message.Should().Be("Controller must be specified");
        }

        [Fact]
        public void routeregistration___should_throw_for_null_method_and_empty_endpoint()
        {
            var exception = Assert.Throws<Exception>(() =>
            {
                new DeepSleepRouteRegistration(
                    template: "test",
                    httpMethods: new[] { "GET" },
                    controller: typeof(string),
                    endpoint: "");
            });

            exception.Should().NotBeNull();
            exception.Message.Should().Be("endpoint or methodInfo must be specified");
        }

        [Fact]
        public void routeregistration___should_throw_for_empty_http_methods()
        {
            var exception = Assert.Throws<Exception>(() =>
            {
                new DeepSleepRouteRegistration(
                    template: "test",
                    httpMethods: new string[] { },
                    controller: typeof(string),
                    endpoint: "test");
            });

            exception.Should().NotBeNull();
            exception.Message.Should().Be("Http methods not specified on endpoint test in controller System.String");
        }
    }
}
