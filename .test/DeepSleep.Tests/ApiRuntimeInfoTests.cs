namespace DeepSleep.Tests
{
    using FluentAssertions;
    using Xunit;

    public class ApiRuntimeInfoTests
    {
        [Fact]
        public void apiruntimeinfo___initialized_correctly()
        {
            var info = new ApiRuntimeInfo();

            info.Exceptions.Should().NotBeNull();
            info.Duration.Should().NotBeNull();
            info.Internals.Should().NotBeNull();
        }

        [Fact]
        public void apiruntimeinfo___log_dump()
        {
            var info = new ApiRuntimeInfo();

            info.Log("test1");
            info.Log("test2");
            info.Log("test3");
            info.Log("test4");

            var results = info.LogDump();

            results.Should().Contain("test1");
            results.Should().Contain("test2");
            results.Should().Contain("test3");
            results.Should().Contain("test4");
        }
    }
}
