namespace DeepSleep.Tests
{
    using FluentAssertions;
    using Xunit;

    public class AcceptHeaderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void accept___ctor_returns_default_for_null_or_whitespace(string value)
        {
            var header = new AcceptHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().BeEmpty();
        }

        [Theory]
        [InlineData("text/json", 1)]
        [InlineData("text/xml", 1)]
        [InlineData("text/xml, text/json", 2)]
        public void accept___ctor_basic_returns_type(string value, int expectedCount)
        {
            var header = new AcceptHeader($"{value}");

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(expectedCount);
        }

        [Fact]
        public void accept___ctor_standard_orders_byquality()
        {
            var value = "text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5";
            var header = new AcceptHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].MediaType.Should().Be("text/xml");
            header.Values[0].Type.Should().Be("text");
            header.Values[0].SubType.Should().Be("xml");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].MediaType.Should().Be("text/plain");
            header.Values[1].Type.Should().Be("text");
            header.Values[1].SubType.Should().Be("plain");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].MediaType.Should().Be("*/*");
            header.Values[2].Type.Should().Be("*");
            header.Values[2].SubType.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].MediaType.Should().Be("text/json");
            header.Values[3].Type.Should().Be("text");
            header.Values[3].SubType.Should().Be("json");
            header.Values[3].Quality.Should().Be(0.1f);
        }

        [Fact]
        public void accept___assignment_standard_orders_byquality()
        {
            var value = "text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5";
            AcceptHeader header = value;

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].MediaType.Should().Be("text/xml");
            header.Values[0].Type.Should().Be("text");
            header.Values[0].SubType.Should().Be("xml");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].MediaType.Should().Be("text/plain");
            header.Values[1].Type.Should().Be("text");
            header.Values[1].SubType.Should().Be("plain");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].MediaType.Should().Be("*/*");
            header.Values[2].Type.Should().Be("*");
            header.Values[2].SubType.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].MediaType.Should().Be("text/json");
            header.Values[3].Type.Should().Be("text");
            header.Values[3].SubType.Should().Be("json");
            header.Values[3].Quality.Should().Be(0.1f);
        }

        [Fact]
        public void accept___ctor_qualities_modified_when_outofrange()
        {
            var value = "text/json;q=-1, text/xml; q=1.1, text/plain; q=0.7, */*; q=0.5";
            var header = new AcceptHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].MediaType.Should().Be("text/xml");
            header.Values[0].Type.Should().Be("text");
            header.Values[0].SubType.Should().Be("xml");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].MediaType.Should().Be("text/plain");
            header.Values[1].Type.Should().Be("text");
            header.Values[1].SubType.Should().Be("plain");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].MediaType.Should().Be("*/*");
            header.Values[2].Type.Should().Be("*");
            header.Values[2].SubType.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].MediaType.Should().Be("text/json");
            header.Values[3].Type.Should().Be("text");
            header.Values[3].SubType.Should().Be("json");
            header.Values[3].Quality.Should().Be(0f);
        }

        [Fact]
        public void accept___assignment_qualities_modified_when_outofrange()
        {
            var value = "text/json;q=-1, text/xml; q=1.1, text/plain; q=0.7, */*; q=0.5";
            AcceptHeader header = value;

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].MediaType.Should().Be("text/xml");
            header.Values[0].Type.Should().Be("text");
            header.Values[0].SubType.Should().Be("xml");
            header.Values[0].Quality.Should().Be(1f);

            header.Values[1].MediaType.Should().Be("text/plain");
            header.Values[1].Type.Should().Be("text");
            header.Values[1].SubType.Should().Be("plain");
            header.Values[1].Quality.Should().Be(0.7f);

            header.Values[2].MediaType.Should().Be("*/*");
            header.Values[2].Type.Should().Be("*");
            header.Values[2].SubType.Should().Be("*");
            header.Values[2].Quality.Should().Be(0.5f);

            header.Values[3].MediaType.Should().Be("text/json");
            header.Values[3].Type.Should().Be("text");
            header.Values[3].SubType.Should().Be("json");
            header.Values[3].Quality.Should().Be(0f);
        }

        [Fact]
        public void accept___ctor_charsets_quality_set_to_0_when_quality_invalid()
        {
            var value = "text/json;q=0.1, text/xml; q=ABC, text/plain; q=0.7, */*; q=0.5";
            var header = new AcceptHeader(value);

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].MediaType.Should().Be("text/plain");
            header.Values[0].Type.Should().Be("text");
            header.Values[0].SubType.Should().Be("plain");
            header.Values[0].Quality.Should().Be(0.7f);

            header.Values[1].MediaType.Should().Be("*/*");
            header.Values[1].Type.Should().Be("*");
            header.Values[1].SubType.Should().Be("*");
            header.Values[1].Quality.Should().Be(0.5f);

            header.Values[2].MediaType.Should().Be("text/json");
            header.Values[2].Type.Should().Be("text");
            header.Values[2].SubType.Should().Be("json");
            header.Values[2].Quality.Should().Be(0.1f);

            header.Values[3].MediaType.Should().Be("text/xml");
            header.Values[3].Type.Should().Be("text");
            header.Values[3].SubType.Should().Be("xml");
            header.Values[3].Quality.Should().Be(0f);
        }

        [Fact]
        public void accept___assignment_charsets_quality_set_to_0_when_quality_invalid()
        {
            var value = "text/json;q=0.1, text/xml; q=ABC, text/plain; q=0.7, */*; q=0.5";
            AcceptHeader header = value;

            header.Should().NotBeNull();
            header.Value.Should().Be(value);
            header.Values.Should().NotBeNull();
            header.Values.Should().HaveCount(4);

            header.Values[0].MediaType.Should().Be("text/plain");
            header.Values[0].Type.Should().Be("text");
            header.Values[0].SubType.Should().Be("plain");
            header.Values[0].Quality.Should().Be(0.7f);

            header.Values[1].MediaType.Should().Be("*/*");
            header.Values[1].Type.Should().Be("*");
            header.Values[1].SubType.Should().Be("*");
            header.Values[1].Quality.Should().Be(0.5f);

            header.Values[2].MediaType.Should().Be("text/json");
            header.Values[2].Type.Should().Be("text");
            header.Values[2].SubType.Should().Be("json");
            header.Values[2].Quality.Should().Be(0.1f);

            header.Values[3].MediaType.Should().Be("text/xml");
            header.Values[3].Type.Should().Be("text");
            header.Values[3].SubType.Should().Be("xml");
            header.Values[3].Quality.Should().Be(0f);
        }


        [Theory]
        [InlineData("text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/JSON;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json;q=0.1, TEXT/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json;q=0.1, text/xml; Q=1, text/plain; q=0.7, */*; q=0.5")]
        public void accept___equals_operator_header_success(string encoding)
        {
            var value = "text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5";

            var header1 = new AcceptHeader(encoding);
            var header2 = new AcceptHeader(value);


            var equals = header1 == header2;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/JSON;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json;q=0.1, TEXT/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json;q=0.1, text/xml; Q=1, text/plain; q=0.7, */*; q=0.5")]
        public void accept___equals_operator_string_success(string encoding)
        {
            var value = "text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5";

            var header = new AcceptHeader(encoding);

            var equals = header == value;
            equals.Should().Be(true);

            equals = value == header;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/JSON;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json;q=0.1, TEXT/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json;q=0.1, text/xml; Q=1, text/plain; q=0.7, */*; q=0.5")]
        public void accept___equals_override_success(string encoding)
        {
            var value = "text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5";

            var header = new AcceptHeader(encoding);

            var equals = header.Equals(value);
            equals.Should().Be(true);

            equals = header.Equals(new AcceptHeader(value));
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/JSON;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json;q=0.1, TEXT/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json;q=0.1, text/xml; Q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json")]
        [InlineData("text/json; q=1")]
        public void accept___notequals_operator_header_success(string encoding)
        {
            var value = "text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5, text/*";

            var header1 = new AcceptHeader(encoding);
            var header2 = new AcceptHeader(value);

            var equals = header1 != header2;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/JSON;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json;q=0.1, TEXT/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json;q=0.1, text/xml; Q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json")]
        [InlineData("text/json; q=1")]
        public void accept___notequals_operator_string_success(string encoding)
        {
            var value = "text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5, text/*";

            var header = new AcceptHeader(encoding);

            var equals = header != value;
            equals.Should().Be(true);

            equals = value != header;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/JSON;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json;q=0.1, TEXT/xml; q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json;q=0.1, text/xml; Q=1, text/plain; q=0.7, */*; q=0.5")]
        [InlineData("text/json")]
        [InlineData("text/json; q=1")]
        public void accept___notequals_override_success(string encoding)
        {
            var value = "text/json;q=0.1, text/xml; q=1, text/plain; q=0.7, */*; q=0.5, text/*";

            var header = new AcceptHeader(encoding);

            var equals = header.Equals(value);
            equals.Should().Be(false);

            equals = header.Equals(new AcceptHeader(value));
            equals.Should().Be(false);

            equals = value.Equals(header);
            equals.Should().Be(false);
        }
    }
}
