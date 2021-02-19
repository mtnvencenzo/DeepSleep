namespace DeepSleep.Tests
{
    using FluentAssertions;
    using Xunit;

    public class AcceptLanguageHeaderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void acceptlanguage___ctor_returns_default_for_null_or_whitespace(string value)
        {
            var header = new AcceptLanguageHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().BeEmpty();
        }

        [Theory]
        [InlineData("en-US", 1)]
        [InlineData("de", 1)]
        [InlineData("en-US, de", 2)]
        public void acceptlanguage___ctor_basic_returns_type(string value, int expectedCount)
        {
            var header = new AcceptLanguageHeader($"{value}");

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(expectedCount);
        }

        [Fact]
        public void acceptlanguage___ctor_standard_orders_byquality()
        {
            var value = "en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5";
            var header = new AcceptLanguageHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Code.Should().Be("es-ES");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].Code.Should().Be("de");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].Code.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].Code.Should().Be("en-US");
            header.Values[3].Quality.Should().Be(0.1f);
        }

        [Fact]
        public void acceptlanguage___assignment_standard_orders_byquality()
        {
            var value = "en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5";
            AcceptLanguageHeader header = value;

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Code.Should().Be("es-ES");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].Code.Should().Be("de");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].Code.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].Code.Should().Be("en-US");
            header.Values[3].Quality.Should().Be(0.1f);
        }

        [Fact]
        public void acceptlanguage___ctor_qualities_modified_when_outofrange()
        {
            var value = "en-US;q=-1, es-ES; q=1.1, de; q=0.7, *; q=0.5";
            var header = new AcceptLanguageHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Code.Should().Be("es-ES");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].Code.Should().Be("de");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].Code.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].Code.Should().Be("en-US");
            header.Values[3].Quality.Should().Be(0f);
        }

        [Fact]
        public void acceptlanguage___assignment_qualities_modified_when_outofrange()
        {
            var value = "en-US;q=-1, es-ES; q=1.1, de; q=0.7, *; q=0.5";
            AcceptLanguageHeader header = value;

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Code.Should().Be("es-ES");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].Code.Should().Be("de");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].Code.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].Code.Should().Be("en-US");
            header.Values[3].Quality.Should().Be(0f);
        }

        [Fact]
        public void acceptlanguage___ctor_charsets_quality_set_to_0_when_quality_invalid()
        {
            var value = "en-US;q=0.1, es-ES; q=ABC, de; q=0.7, *; q=0.5";
            var header = new AcceptLanguageHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Code.Should().Be("de");
            header.Values[0].Quality.Should().Be(0.7f);

            header.Values[1].Code.Should().Be("*");
            header.Values[1].Quality.Should().Be(0.5f);

            header.Values[2].Code.Should().Be("en-US");
            header.Values[2].Quality.Should().Be(0.1f);

            header.Values[3].Code.Should().Be("es-ES");
            header.Values[3].Quality.Should().Be(0f);
        }

        [Fact]
        public void acceptlanguage___assignment_charsets_quality_set_to_0_when_quality_invalid()
        {
            var value = "en-US;q=0.1, es-ES; q=ABC, de; q=0.7, *; q=0.5";
            AcceptLanguageHeader header = value;

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].Code.Should().Be("de");
            header.Values[0].Quality.Should().Be(0.7f);

            header.Values[1].Code.Should().Be("*");
            header.Values[1].Quality.Should().Be(0.5f);

            header.Values[2].Code.Should().Be("en-US");
            header.Values[2].Quality.Should().Be(0.1f);

            header.Values[3].Code.Should().Be("es-ES");
            header.Values[3].Quality.Should().Be(0f);
        }


        [Theory]
        [InlineData("en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("EN-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US;q=0.1, es-es; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US;q=0.1, es-ES; Q=1, de; q=0.7, *; q=0.5")]
        public void acceptlanguage___equals_operator_header_success(string encoding)
        {
            var value = "en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5";

            var header1 = new AcceptLanguageHeader(encoding);
            var header2 = new AcceptLanguageHeader(value);

            var equals = header1 == header2;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("EN-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US;q=0.1, es-es; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US;q=0.1, es-ES; Q=1, de; q=0.7, *; q=0.5")]
        public void acceptlanguage___equals_operator_string_success(string encoding)
        {
            var value = "en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5";

            var header = new AcceptLanguageHeader(encoding);

            var equals = header == value;
            equals.Should().Be(true);

            equals = value == header;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("EN-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US;q=0.1, es-es; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US;q=0.1, es-ES; Q=1, de; q=0.7, *; q=0.5")]
        public void acceptlanguage___equals_override_success(string encoding)
        {
            var value = "en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5";

            var header = new AcceptLanguageHeader(encoding);

            var equals = header.Equals(value);
            equals.Should().Be(true);

            equals = header.Equals(new AcceptLanguageHeader(value));
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("EN-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US;q=0.1, es-es; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US;q=0.1, es-ES; Q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US")]
        [InlineData("en-US; q=1")]
        public void acceptlanguage___notequals_operator_header_success(string encoding)
        {
            var value = "en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5, xi";

            var contentType1 = new AcceptLanguageHeader(encoding);
            var contentType2 = new AcceptLanguageHeader(value);

            var equals = contentType1 != contentType2;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("EN-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US;q=0.1, es-es; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US;q=0.1, es-ES; Q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US")]
        [InlineData("en-US; q=1")]
        public void acceptlanguage___notequals_operator_string_success(string encoding)
        {
            var value = "en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5, xi";

            var header = new AcceptLanguageHeader(encoding);

            var equals = header != value;
            equals.Should().Be(true);

            equals = value != header;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("EN-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US;q=0.1, es-es; q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US;q=0.1, es-ES; Q=1, de; q=0.7, *; q=0.5")]
        [InlineData("en-US")]
        [InlineData("en-US; q=1")]
        public void acceptlanguage___notequals_override_success(string encoding)
        {
            var value = "en-US;q=0.1, es-ES; q=1, de; q=0.7, *; q=0.5, xi";

            var header = new AcceptLanguageHeader(encoding);

            var equals = header.Equals(value);
            equals.Should().Be(false);

            equals = header.Equals(new AcceptLanguageHeader(value));
            equals.Should().Be(false);

            equals = value.Equals(header);
            equals.Should().Be(false);
        }

        [Fact]
        public void acceptlanguage___implicit_to_string_from_null_success()
        {
            string value = null as AcceptLanguageHeader;

            value.Should().BeNull();
        }

        [Fact]
        public void acceptlanguage___null_equals_operator_null_success()
        {
            var equals = (null as AcceptLanguageHeader) == (null as AcceptLanguageHeader);

            equals.Should().BeTrue();
        }

        [Fact]
        public void acceptlanguage___null_not_equals_operator_null_success()
        {
            var equals = (null as AcceptLanguageHeader) != (null as AcceptLanguageHeader);

            equals.Should().BeFalse();
        }

        [Fact]
        public void acceptlanguage___gethashcode_success()
        {
            AcceptLanguageHeader header = "en-us;";

            header.Should().NotBeNull();

            var hashCode = header.GetHashCode();
            hashCode.Should().NotBe(0);
        }

        [Fact]
        public void acceptlanguage___equals_overload_null_sccess()
        {
            AcceptLanguageHeader header1 = "en-us;";
            AcceptLanguageHeader header2 = null;

            var equals = header1.Equals(header2);

            equals.Should().BeFalse();
        }

        [Fact]
        public void acceptlanguage___equals_overload_not_string_or_equivelent_sccess()
        {
            AcceptLanguageHeader header1 = "en-us;";
            var other = 2;

            var equals = header1.Equals(other);

            equals.Should().BeFalse();
        }
    }
}
