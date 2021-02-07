namespace DeepSleep.Tests
{
    using FluentAssertions;
    using Xunit;

    public class ApiResponseInfoTests
    {
        [Fact]
        public void apiresponseinfo___has_success_status_returns_false_for_null_response()
        {
            var responseInfo = null as ApiResponseInfo;

            var result = responseInfo.HasSuccessStatus();

            result.Should().BeFalse();
        }

        [Fact]
        public void apiresponseinfo___set_httpstatus_for_null_response()
        {
            var responseInfo = null as ApiResponseInfo;

            var result = responseInfo.SetHttpStatus(201);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);
        }
    }
}
