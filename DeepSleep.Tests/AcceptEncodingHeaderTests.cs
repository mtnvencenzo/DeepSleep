namespace DeepSleep.Tests
{
    using FluentAssertions;
    using System;
    using System.Globalization;
    using Xunit;

    public class AcceptEncodingHeaderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void acceptencoding___ctor_returns_default_for_null_or_whitespace(string value)
        {
            var header = new AcceptEncodingHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().BeEmpty();
        }

        [Theory]
        [InlineData("gzip", 1)]
        [InlineData("deflate", 1)]
        [InlineData("gzip, deflate", 2)]
        public void acceptencoding___ctor_basic_returns_type(string value, int expectedCount)
        {
            var header = new AcceptEncodingHeader($"{value}");

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(expectedCount);
        }

        [Fact]
        public void acceptencoding___ctor_standard_orders_byquality()
        {
            var value = "gzip;q=0.1, deflate; q=1, compress; q=0.7, *; q=0.5";
            var header = new AcceptEncodingHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Encoding.Should().Be("deflate");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].Encoding.Should().Be("compress");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].Encoding.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].Encoding.Should().Be("gzip");
            header.Values[3].Quality.Should().Be(0.1f);
        }

        [Fact]
        public void acceptencoding___assignment_standard_orders_byquality()
        {
            var value = "gzip;q=0.1, deflate; q=1, compress; q=0.7, *; q=0.5";
            AcceptEncodingHeader header = value;

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Encoding.Should().Be("deflate");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].Encoding.Should().Be("compress");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].Encoding.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].Encoding.Should().Be("gzip");
            header.Values[3].Quality.Should().Be(0.1f);
        }

        [Fact]
        public void acceptencoding___ctor_qualities_modified_when_outofrange()
        {
            var value = "gzip;q=-1, deflate; q=1.1, compress; q=0.7, *; q=0.5";
            var header = new AcceptEncodingHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Encoding.Should().Be("deflate");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].Encoding.Should().Be("compress");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].Encoding.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].Encoding.Should().Be("gzip");
            header.Values[3].Quality.Should().Be(0f);
        }

        [Fact]
        public void acceptencoding___assignment_qualities_modified_when_outofrange()
        {
            var value = "gzip;q=-1, deflate; q=1.1, compress; q=0.7, *; q=0.5";
            AcceptEncodingHeader header = value;

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Encoding.Should().Be("deflate");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].Encoding.Should().Be("compress");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].Encoding.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].Encoding.Should().Be("gzip");
            header.Values[3].Quality.Should().Be(0f);
        }

        [Fact]
        public void acceptencoding___ctor_charsets_quality_set_to_0_when_quality_invalid()
        {
            var value = "gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5";
            var header = new AcceptEncodingHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Encoding.Should().Be("compress");
            header.Values[0].Quality.Should().Be(0.7f);

            header.Values[1].Encoding.Should().Be("*");
            header.Values[1].Quality.Should().Be(0.5f);

            header.Values[2].Encoding.Should().Be("gzip");
            header.Values[2].Quality.Should().Be(0.1f);

            header.Values[3].Encoding.Should().Be("deflate");
            header.Values[3].Quality.Should().Be(0f);
        }

        [Fact]
        public void acceptencoding___assignment_charsets_quality_set_to_0_when_quality_invalid()
        {
            var value = "gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5";
            AcceptEncodingHeader header = value;

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Encoding.Should().Be("compress");
            header.Values[0].Quality.Should().Be(0.7f);

            header.Values[1].Encoding.Should().Be("*");
            header.Values[1].Quality.Should().Be(0.5f);

            header.Values[2].Encoding.Should().Be("gzip");
            header.Values[2].Quality.Should().Be(0.1f);

            header.Values[3].Encoding.Should().Be("deflate");
            header.Values[3].Quality.Should().Be(0f);
        }


        [Theory]
        [InlineData("gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("GZIP;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip;q=0.1, deflate; Q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip;q=0.1, Deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        public void acceptencoding___equals_operator_header_success(string encoding)
        {
            var value = "gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5";

            var header1 = new AcceptEncodingHeader(encoding);
            var header2 = new AcceptEncodingHeader(value);


            var equals = header1 == header2;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("GZIP;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip;q=0.1, deflate; Q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip;q=0.1, Deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        public void acceptencoding___equals_operator_string_success(string encoding)
        {
            var value = "gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5";

            var header = new AcceptEncodingHeader(encoding);

            var equals = header == value;
            equals.Should().Be(true);

            equals = value == header;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("GZIP;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip;q=0.1, deflate; Q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip;q=0.1, Deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        public void acceptencoding___equals_override_success(string encoding)
        {
            var value = "gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5";

            var header = new AcceptEncodingHeader(encoding);

            var equals = header.Equals(value);
            equals.Should().Be(true);

            equals = header.Equals(new AcceptEncodingHeader(value));
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("GZIP;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip;q=0.1, deflate; Q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip;q=0.1, Deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip")]
        [InlineData("gzip; q=1")]
        public void acceptencoding___notequals_operator_header_success(string encoding)
        {
            var value = "gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5, br";

            var contentType1 = new AcceptEncodingHeader(encoding);
            var contentType2 = new AcceptEncodingHeader(value);

            var equals = contentType1 != contentType2;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("GZIP;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip;q=0.1, deflate; Q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip;q=0.1, Deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip")]
        [InlineData("gzip; q=1")]
        public void acceptencoding___notequals_operator_string_success(string encoding)
        {
            var value = "gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5, br";

            var header = new AcceptEncodingHeader(encoding);

            var equals = header != value;
            equals.Should().Be(true);

            equals = value != header;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("GZIP;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip;q=0.1, deflate; Q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip;q=0.1, Deflate; q=ABC, compress; q=0.7, *; q=0.5")]
        [InlineData("gzip")]
        [InlineData("gzip; q=1")]
        public void acceptencoding___notequals_override_success(string encoding)
        {
            var value = "gzip;q=0.1, deflate; q=ABC, compress; q=0.7, *; q=0.5, br";

            var header = new AcceptEncodingHeader(encoding);

            var equals = header.Equals(value);
            equals.Should().Be(false);

            equals = header.Equals(new AcceptEncodingHeader(value));
            equals.Should().Be(false);

            equals = value.Equals(header);
            equals.Should().Be(false);
        }
    }
}
