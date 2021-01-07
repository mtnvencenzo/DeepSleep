namespace DeepSleep.Tests
{
    using FluentAssertions;
    using System;
    using Xunit;

    public class ApiRequestContextResolverTests
    {
        [Fact]
        public void apiRequestContextResolve___does_not_allow_multiple_sets()
        {
            var resolver = new DefaultApiRequestContextResolver();

            resolver.SetContext(new ApiRequestContext());

            var exception = Assert.Throws<InvalidOperationException>(() => resolver.SetContext(new ApiRequestContext()));

            exception.Should().NotBeNull();
            exception.Message.Should().Be("Attempt to overwrite existing context not allowed");
        }
    }
}
