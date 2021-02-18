namespace DeepSleep.Tests.Discovery
{
    using DeepSleep.Discovery;
    using FluentAssertions;
    using Xunit;

    public class DiscoveryStrategiesTests
    {
        [Fact]
        public void discoverystrategies___default_contains_correct_strategies()
        {
            var strategies = DiscoveryStrategies.Default();

            strategies.Should().NotBeNull();
            strategies.Should().HaveCount(2);
            strategies[0].GetType().Should().Be(typeof(AttributeRouteDiscoveryStrategy));
            strategies[1].GetType().Should().Be(typeof(DelegatedRouteDiscoveryStrategy));
        }
    }
}
