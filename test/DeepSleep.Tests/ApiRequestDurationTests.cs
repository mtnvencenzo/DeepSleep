namespace DeepSleep.Tests
{
    using FluentAssertions;
    using System;
    using Xunit;

    public class ApiRequestDurationTests
    {
        [Fact]
        public void apiRequestDuration___should_work_without_an_enddate()
        {
            var start = DateTimeOffset.UtcNow;

            var apiDuration = new ApiRequestDuration
            {
                UtcStart = start
            };

            var end = DateTimeOffset.UtcNow;
            var expectedDuration = (int)(end - start).TotalMilliseconds;

            var duration = apiDuration.Duration;

            duration.Should().BeGreaterOrEqualTo(expectedDuration);
            duration.Should().BeLessOrEqualTo(expectedDuration + 200);
        }
    }
}
