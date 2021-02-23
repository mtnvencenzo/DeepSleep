namespace DeepSleep.Web.Tests
{
    using FluentAssertions;
    using Xunit;

    public class ApiRequestContextPipelineComponentTests
    {
        [Theory]
        [InlineData("/api/hello", @"^/?api(/)?(?(1).*|)$", true)]
        [InlineData("/api/", @"^/?api(/)?(?(1).*|)$", true)]
        [InlineData("/api", @"^/?api(/)?(?(1).*|)$", true)]
        [InlineData("api/", @"^/?api(/)?(?(1).*|)$", true)]
        [InlineData("api/test/1/other", @"^/?api(/)?(?(1).*|)$", true)]
        [InlineData("/api/test/1/other", @"^/?api(/)?(?(1).*|)$", true)]
        [InlineData("/api/test-path/1_0/other", @"^/?api(/)?(?(1).*|)$", true)]
        [InlineData("/ap/test/1/other", @"^/?api(/)?(?(1).*|)$", false)]
        [InlineData("ap/test/1/other", @"^/?api(/)?(?(1).*|)$", false)]
        [InlineData("apitest/1/other", @"^/?api(/)?(?(1).*|)$", false)]
        [InlineData("/apitest/1/other", @"^/?api(/)?(?(1).*|)$", false)]
        [InlineData("/apitest", @"^/?api(/)?(?(1).*|)$", false)]
        [InlineData("/apitest/", @"^/?api(/)?(?(1).*|)$", false)]
        [InlineData("apitest", @"^/?api(/)?(?(1).*|)$", false)]
        [InlineData("/api-t/test/", @"^/?api(/)?(?(1).*|)$", false)]
        public void includepath__success(string path, string regex, bool expected)
        {
            var result = ApiRequestContextPipelineComponent.IsIncludedPath(path, new[] { regex });
            result.Should().Be(expected);
        }
    }
}
