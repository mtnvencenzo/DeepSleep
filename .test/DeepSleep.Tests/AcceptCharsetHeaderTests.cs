namespace DeepSleep.Tests
{
    using FluentAssertions;
    using Xunit;

    public class AcceptCharsetHeaderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void acceptcharset___ctor_returns_default_for_null_or_whitespace(string value)
        {
            var header = new AcceptCharsetHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().BeEmpty();
        }

        [Theory]
        [InlineData("utf-8", 1)]
        [InlineData("UTF-8", 1)]
        [InlineData("UTF-8, us-ascii", 2)]
        public void acceptcharset___ctor_basic_returns_type(string value, int expectedCount)
        {
            var header = new AcceptCharsetHeader($"{value}");

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(expectedCount);
        }

        [Fact]
        public void acceptcharset___ctor_standard_orders_byquality()
        {
            var value = "iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5";
            var header = new AcceptCharsetHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Charset.Should().Be("utf-8");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].Charset.Should().Be("us-ascii");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].Charset.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].Charset.Should().Be("iso-8859-1");
            header.Values[3].Quality.Should().Be(0.1f);
        }

        [Fact]
        public void acceptcharset___assignment_standard_orders_byquality()
        {
            var value = "iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5";
            AcceptCharsetHeader header = value;

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Charset.Should().Be("utf-8");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].Charset.Should().Be("us-ascii");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].Charset.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].Charset.Should().Be("iso-8859-1");
            header.Values[3].Quality.Should().Be(0.1f);
        }

        [Fact]
        public void acceptcharset___ctor_qualities_modified_when_outofrange()
        {
            var value = "iso-8859-1;q=-1, utf-8; q=1.1, us-ascii; q=0.7, *; q=0.5";
            var header = new AcceptCharsetHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Charset.Should().Be("utf-8");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].Charset.Should().Be("us-ascii");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].Charset.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].Charset.Should().Be("iso-8859-1");
            header.Values[3].Quality.Should().Be(0f);
        }

        [Fact]
        public void acceptcharset___assignment_qualities_modified_when_outofrange()
        {
            var value = "iso-8859-1;q=-1, utf-8; q=1.1, us-ascii; q=0.7, *; q=0.5";
            AcceptCharsetHeader header = value;

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Charset.Should().Be("utf-8");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].Charset.Should().Be("us-ascii");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].Charset.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].Charset.Should().Be("iso-8859-1");
            header.Values[3].Quality.Should().Be(0f);
        }

        [Fact]
        public void acceptcharset___ctor_charsets_quality_set_to_0_when_quality_invalid()
        {
            var value = "iso-8859-1;q=0.1, utf-8; q=ABC, us-ascii; q=0.7, *; q=0.5";
            var header = new AcceptCharsetHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Charset.Should().Be("us-ascii");
            header.Values[0].Quality.Should().Be(0.7f);

            header.Values[1].Charset.Should().Be("*");
            header.Values[1].Quality.Should().Be(0.5f);

            header.Values[2].Charset.Should().Be("iso-8859-1");
            header.Values[2].Quality.Should().Be(0.1f);

            header.Values[3].Charset.Should().Be("utf-8");
            header.Values[3].Quality.Should().Be(0f);
        }

        [Fact]
        public void acceptcharset___assignment_charsets_quality_set_to_0_when_quality_invalid()
        {
            var value = "iso-8859-1;q=0.1, utf-8; q=ABC, us-ascii; q=0.7, *; q=0.5";
            AcceptCharsetHeader header = value;

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Charset.Should().Be("us-ascii");
            header.Values[0].Quality.Should().Be(0.7f);

            header.Values[1].Charset.Should().Be("*");
            header.Values[1].Quality.Should().Be(0.5f);

            header.Values[2].Charset.Should().Be("iso-8859-1");
            header.Values[2].Quality.Should().Be(0.1f);

            header.Values[3].Charset.Should().Be("utf-8");
            header.Values[3].Quality.Should().Be(0f);
        }


        [Theory]
        [InlineData("iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; q=1, US-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; Q=1, US-ascii; q=0.7, *; q=0.5")]
        public void acceptcharset___equals_operator_header_success(string charset)
        {
            string value = $"iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5";

            var header1 = new AcceptCharsetHeader(charset);
            var header2 = new AcceptCharsetHeader(value);


            var equals = header1 == header2;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; q=1, US-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; Q=1, US-ascii; q=0.7, *; q=0.5")]
        public void acceptcharset___equals_operator_string_success(string charset)
        {
            string value = $"iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5";

            var header = new AcceptCharsetHeader(charset);

            var equals = header == value;
            equals.Should().Be(true);

            equals = value == header;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; q=1, US-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; Q=1, US-ascii; q=0.7, *; q=0.5")]
        public void acceptcharset___equals_override_success(string charset)
        {
            string value = $"iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5";

            var header = new AcceptCharsetHeader(charset);

            var equals = header.Equals(value);
            equals.Should().Be(true);

            equals = header.Equals(new AcceptCharsetHeader(value));
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; q=1, US-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; Q=1, US-ascii; q=0.7, *; q=0.5")]
        [InlineData("utf-8")]
        [InlineData("utf-8; q=1")]
        public void acceptcharset___notequals_operator_header_success(string charset)
        {
            string value = $"iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.4";

            var contentType1 = new AcceptCharsetHeader(charset);
            var contentType2 = new AcceptCharsetHeader(value);

            var equals = contentType1 != contentType2;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; q=1, US-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; Q=1, US-ascii; q=0.7, *; q=0.5")]
        [InlineData("utf-8")]
        [InlineData("utf-8; q=1")]
        public void acceptcharset___notequals_operator_string_success(string charset)
        {
            string value = $"iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.4";

            var header = new AcceptCharsetHeader(charset);

            var equals = header != value;
            equals.Should().Be(true);

            equals = value != header;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; q=1, US-ascii; q=0.7, *; q=0.5")]
        [InlineData("ISO-8859-1;q=0.1, utf-8; Q=1, US-ascii; q=0.7, *; q=0.5")]
        [InlineData("utf-8")]
        [InlineData("utf-8; q=1")]
        public void acceptcharset___notequals_override_success(string charset)
        {
            string value = $"iso-8859-1;q=0.1, utf-8; q=1, us-ascii; q=0.7, *; q=0.4";

            var header = new AcceptCharsetHeader(charset);

            var equals = header.Equals(value);
            equals.Should().Be(false);

            equals = header.Equals(new AcceptCharsetHeader(value));
            equals.Should().Be(false);

            equals = value.Equals(header);
            equals.Should().Be(false);
        }

        [Fact]
        public void acceptcharset___implicit_to_string_from_null_success()
        {
            string value = null as AcceptCharsetHeader;

            value.Should().BeNull();
        }

        [Fact]
        public void acceptcharset___null_equals_operator_null_success()
        {
            var equals = (null as AcceptCharsetHeader) == (null as AcceptCharsetHeader);

            equals.Should().BeTrue();
        }

        [Fact]
        public void acceptcharset___null_not_equals_operator_null_success()
        {
            var equals = (null as AcceptCharsetHeader) != (null as AcceptCharsetHeader);

            equals.Should().BeFalse();
        }

        [Fact]
        public void acceptcharset___gethashcode_success()
        {
            AcceptCharsetHeader header = "en-us;";

            header.Should().NotBeNull();

            var hashCode = header.GetHashCode();
            hashCode.Should().NotBe(0);
        }

        [Fact]
        public void acceptcharset___equals_overload_null_sccess()
        {
            AcceptCharsetHeader header1 = "en-us;";
            AcceptCharsetHeader header2 = null;

            var equals = header1.Equals(header2);

            equals.Should().BeFalse();
        }

        [Fact]
        public void acceptcharset___equals_overload_not_string_or_equivelent_sccess()
        {
            AcceptCharsetHeader header1 = "en-us;";
            var other = 2;

            var equals = header1.Equals(other);

            equals.Should().BeFalse();
        }
    }
}
