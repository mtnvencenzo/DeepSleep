namespace DeepSleep.Tests
{
    using FluentAssertions;
    using System.Collections.Generic;
    using Xunit;

    public class ApiHeaderTests
    {
        [Fact]
        public void apiheader___does_not_error_on_setting_values_on_null_headers()
        {
            List<ApiHeader> headers = null;

            headers.SetValue("X-Test1", "test");

            headers.Should().BeNull();
        }

        [Fact]
        public void apiheader___does_not_error_on_setting_values_on_empty_headers()
        {
            List<ApiHeader> headers = new List<ApiHeader>();

            headers.SetValue("X-Test1", "test");

            headers.Should().BeEmpty();
        }

        [Fact]
        public void apiheader___does_not_update_values_on_non_matching_headers()
        {
            List<ApiHeader> headers = new List<ApiHeader>
            {
                new ApiHeader("X-Test", ""),
                new ApiHeader("X-Test", "value")
            };

            headers.SetValue("X-Test-UnMatched", "test");

            headers.Should().HaveCount(2);
            headers[0].Name.Should().Be("X-Test");
            headers[0].Value.Should().Be("");
            headers[1].Name.Should().Be("X-Test");
            headers[1].Value.Should().Be("value");
        }

        [Fact]
        public void apiheader___updates_setting_values_on_matching_headers()
        {
            List<ApiHeader> headers = new List<ApiHeader>
            {
                new ApiHeader("X-Test", ""),
                new ApiHeader("X-Test", "value")
            };

            headers.SetValue("X-Test", "test");

            headers.Should().HaveCount(2);
            headers[0].Name.Should().Be("X-Test");
            headers[0].Value.Should().Be("test");
            headers[1].Name.Should().Be("X-Test");
            headers[1].Value.Should().Be("test");
        }

        [Fact]
        public void apiheader___returns_empty_string_on_getvalue_on_null_headers()
        {
            List<ApiHeader> headers = null;

            var value = headers.GetValue("X-Test");

            value.Should().BeEmpty();
        }

        [Fact]
        public void apiheader___returns_empty_string_on_getvalue_on_empty_headers()
        {
            List<ApiHeader> headers = new List<ApiHeader>();

            var value = headers.GetValue("X-Test");

            value.Should().BeEmpty();
        }

        [Fact]
        public void apiheader___returns_empty_string_on_getvalue_on_headers_not_having_header()
        {
            List<ApiHeader> headers = new List<ApiHeader>
            {
                new ApiHeader("X-Test", "value")
            };

            var value = headers.GetValue("X-Test-NotMatched");

            value.Should().BeEmpty();
        }

        [Fact]
        public void apiheader___returns_empty_string_on_getvalue_on_headers_matching_header()
        {
            List<ApiHeader> headers = new List<ApiHeader>
            {
                new ApiHeader("X-Test", "value-test")
            };

            var value = headers.GetValue("X-Test");

            value.Should().Be("value-test");
        }

        [Fact]
        public void apiheader___returns_false_for_hasheader_on_null_headers()
        {
            List<ApiHeader> headers = null;

            var value = headers.HasHeader("X-Test");

            value.Should().BeFalse();
        }

        [Fact]
        public void apiheader___returns_false_for_hasheader_on_empty_headers()
        {
            List<ApiHeader> headers = new List<ApiHeader>();

            var value = headers.HasHeader("X-Test");

            value.Should().BeFalse();
        }

        [Fact]
        public void apiheader___returns_false_for_hasheader_on_no_match_headers()
        {
            List<ApiHeader> headers = new List<ApiHeader>
            {
                new ApiHeader("X-Test", "value-test")
            };

            var value = headers.HasHeader("X-Test-No-Match");

            value.Should().BeFalse();
        }

        [Fact]
        public void apiheader___returns_true_for_hasheader_on_matching_headers()
        {
            List<ApiHeader> headers = new List<ApiHeader>
            {
                new ApiHeader("X-Test", "value-test"),
                new ApiHeader("X-Test-Match", "value-test-match")
            };

            var value = headers.HasHeader("X-Test-Match");

            value.Should().BeTrue();
        }
    }
}
