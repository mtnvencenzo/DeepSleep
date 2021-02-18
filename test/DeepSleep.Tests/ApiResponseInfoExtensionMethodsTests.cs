namespace DeepSleep.Tests
{
    using DeepSleep;
    using FluentAssertions;
    using System;
    using Xunit;

    public class ApiResponseInfoExtensionMethodsTests
    {
        [Fact]
        public void apiresponseinfo___getheadervalues_for_null_responseinfo_returns_empty_array()
        {
            var responseInfo = null as ApiResponseInfo;

            var values = responseInfo.GetHeaderValues("test");

            values.Should().NotBeNull();
            values.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void apiresponseinfo___getheadervalues_for_empty_header_name_returns_empty_array(string name)
        {
            var responseInfo = new ApiResponseInfo();

            var values = responseInfo.GetHeaderValues(name);

            values.Should().NotBeNull();
            values.Should().BeEmpty();
        }

        [Fact]
        public void apiresponseinfo___addheader_for_null_responseinfo_returns_null()
        {
            var responseInfo = null as ApiResponseInfo;

            var result = responseInfo.AddHeader("test", "value", false, false);

            result.Should().BeNull();
        }

        [Fact]
        public void apiresponseinfo___addheader_removes_existing_for_not_append()
        {
            var responseInfo = new ApiResponseInfo();

            responseInfo.AddHeader("test", "value", false, false);
            responseInfo.AddHeader("test", "value1", false, false);

            var values = responseInfo.GetHeaderValues("test");

            values.Should().NotBeNull();
            values.Should().HaveCount(1);
            values[0].Should().Be("value1");
        }

        [Fact]
        public void apiresponseinfo___addntitycaching_for_null_responseinfo_returns_null()
        {
            var responseInfo = null as ApiResponseInfo;

            var result = responseInfo.AddEntityCaching("test", DateTimeOffset.UtcNow);

            result.Should().BeNull();
        }

        [Fact]
        public void apiresponseinfo___addntitycaching()
        {
            var responseInfo = new ApiResponseInfo();
            var guid = Guid.NewGuid().ToString();
            var now = DateTimeOffset.UtcNow;

            var result = responseInfo.AddEntityCaching(guid, now);

            result.Should().NotBeNull();

            var etagHeader = result.GetHeaderValues("etag");
            etagHeader.Should().NotBeNull();
            etagHeader.Should().HaveCount(1);
            etagHeader[0].Should().Be(guid);

            var lastModifedHeader = result.GetHeaderValues("Last-Modified");
            lastModifedHeader.Should().NotBeNull();
            lastModifedHeader.Should().HaveCount(1);
            lastModifedHeader[0].Should().Be(now.ToString("r"));
        }

        [Fact]
        public void apiresponseinfo___addntitycaching_no_etag()
        {
            var responseInfo = new ApiResponseInfo();
            var now = DateTimeOffset.UtcNow;

            var result = responseInfo.AddEntityCaching(string.Empty, now);

            result.Should().NotBeNull();

            var etagHeader = result.GetHeaderValues("etag");
            etagHeader.Should().NotBeNull();
            etagHeader.Should().BeEmpty();

            var lastModifedHeader = result.GetHeaderValues("Last-Modified");
            lastModifedHeader.Should().NotBeNull();
            lastModifedHeader.Should().HaveCount(1);
            lastModifedHeader[0].Should().Be(now.ToString("r"));
        }

        [Fact]
        public void apiresponseinfo___addntitycaching_no_lastmodified()
        {
            var responseInfo = new ApiResponseInfo();
            var guid = Guid.NewGuid().ToString();

            var result = responseInfo.AddEntityCaching(guid, null);

            result.Should().NotBeNull();

            var etagHeader = result.GetHeaderValues("etag");
            etagHeader.Should().NotBeNull();
            etagHeader.Should().HaveCount(1);
            etagHeader[0].Should().Be(guid);

            var lastModifedHeader = result.GetHeaderValues("Last-Modified");
            lastModifedHeader.Should().NotBeNull();
            lastModifedHeader.Should().BeEmpty();
        }

        [Fact]
        public void apiresponseinfo___addntitycaching_no_values()
        {
            var responseInfo = new ApiResponseInfo();

            var result = responseInfo.AddEntityCaching(null, null);

            result.Should().NotBeNull();

            var etagHeader = result.GetHeaderValues("etag");
            etagHeader.Should().NotBeNull();
            etagHeader.Should().BeEmpty();

            var lastModifedHeader = result.GetHeaderValues("Last-Modified");
            lastModifedHeader.Should().NotBeNull();
            lastModifedHeader.Should().BeEmpty();
        }
    }
}
