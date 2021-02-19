namespace DeepSleep.Tests
{
    using FluentAssertions;
    using System.Linq;
    using Xunit;

    public class ApiInternalsTests
    {
        [Fact]
        public void apiinternals___exceptions_tests_for_coverage()
        {
            var internals = new ApiInternals();

            internals.Exceptions.Should().NotBeNull();

            internals.Exceptions.Add(new System.Exception());

            var exception = internals.Exceptions.FirstOrDefault();

            exception.Should().NotBeNull();
        }
    }
}
