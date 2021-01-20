namespace DeepSleep.Tests
{
    using FluentAssertions;
    using Xunit;

    public class ContentTypeHeaderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void contenttype___ctor_returns_default_for_null_or_whitespace(string value)
        {
            var header = new ContentTypeHeader(value);

            header.Should().NotBeNull();
            header.Boundary.Should().BeEmpty();
            header.Charset.Should().BeEmpty();
            header.Value.Should().Be(value);
            header.MediaType.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void contenttype___assignment_returns_default_for_null_or_whitespace(string value)
        {
            ContentTypeHeader header = value;

            header.Should().NotBeNull();
            header.Boundary.Should().BeEmpty();
            header.Charset.Should().BeEmpty();
            header.Value.Should().Be(value);
            header.MediaType.Should().BeEmpty();
        }

        [Theory]
        [InlineData("application", "json")]
        [InlineData("application", "json ")]
        [InlineData(" application", "json")]
        [InlineData(" application", "json ")]
        [InlineData(" APPLICATION", "JSON ")]
        [InlineData(" APPLICATION", " JSON ")]
        [InlineData(" APPLICATION ", " JSON ")]
        public void contenttype___ctor_basic_returns_type_and_subtype(string type, string subtype)
        {
            var header = new ContentTypeHeader($"{type}/{subtype}");

            header.Should().NotBeNull();
            header.Value.Should().Be($"{type}/{subtype}");
            header.Boundary.Should().BeEmpty();
            header.Charset.Should().BeEmpty();
            header.MediaType.Should().Be("application/json");
            header.Type.Should().Be("application");
            header.SubType.Should().Be("json");
            header.Charset.Should().BeEmpty();
            header.Boundary.Should().BeEmpty();
        }

        [Theory]
        [InlineData("application", "json")]
        [InlineData("application", "json ")]
        [InlineData(" application", "json")]
        [InlineData(" application", "json ")]
        [InlineData(" APPLICATION", "JSON ")]
        [InlineData(" APPLICATION", " JSON ")]
        [InlineData(" APPLICATION ", " JSON ")]
        public void contenttype___assignment_basic_returns_type_and_subtype(string type, string subtype)
        {
            ContentTypeHeader header = $"{type}/{subtype}";

            header.Should().NotBeNull();
            header.Value.Should().Be($"{type}/{subtype}");
            header.Boundary.Should().BeEmpty();
            header.Charset.Should().BeEmpty();
            header.MediaType.Should().Be("application/json");
            header.Type.Should().Be("application");
            header.SubType.Should().Be("json");
            header.Charset.Should().BeEmpty();
            header.Boundary.Should().BeEmpty();
        }

        [Theory]
        [InlineData("application/json;", " charset=UTF-8")]
        [InlineData("application/json;", " charset=UTF-8 ")]
        [InlineData("application/json;", " charset= UTF-8")]
        [InlineData("application/json;", "charset= UTF-8 ")]
        [InlineData("application/json;", " charset= UTF-8 ")]
        public void contenttype___charset_ctor_has_correct_charset(string contentType, string charset)
        {
            var header = new ContentTypeHeader($"{contentType}{charset}");

            header.Should().NotBeNull();
            header.Value.Should().Be($"{contentType}{charset}");
            header.MediaType.Should().Be("application/json");
            header.Type.Should().Be("application");
            header.SubType.Should().Be("json");

            header.Boundary.Should().BeEmpty();
            header.Charset.Should().Be("utf-8");
        }

        [Theory]
        [InlineData("application/json;", " charset=UTF-8")]
        [InlineData("application/json;", " charset=UTF-8 ")]
        [InlineData("application/json;", " charset= UTF-8")]
        [InlineData("application/json;", "charset= UTF-8 ")]
        [InlineData("application/json;", " charset= UTF-8 ")]
        public void contenttype___charset_assignment_has_correct_charset(string contentType, string charset)
        {
            ContentTypeHeader header = $"{contentType}{charset}";

            header.Should().NotBeNull();
            header.Value.Should().Be($"{contentType}{charset}");
            header.MediaType.Should().Be("application/json");
            header.Type.Should().Be("application");
            header.SubType.Should().Be("json");

            header.Boundary.Should().BeEmpty();
            header.Charset.Should().Be("utf-8");
        }

        [Theory]
        [InlineData("application/json;", " boundary=--iuhfisdhfiuushdfhisudhfhsiduhfsdhf", "--iuhfisdhfiuushdfhisudhfhsiduhfsdhf")]
        [InlineData("application/json;", " boundary=--iuhfisdhfiuushdfhisudhfhsiduhfsdhf ", "--iuhfisdhfiuushdfhisudhfhsiduhfsdhf ")]
        [InlineData("application/json;", " boundary= --iuhfisdhfiuushdfhisudhfhsiduhfsdhf", " --iuhfisdhfiuushdfhisudhfhsiduhfsdhf")]
        [InlineData("application/json;", "boundary= --iuhfisdhfiuushdfhisudhfhsiduhfsdhf ", " --iuhfisdhfiuushdfhisudhfhsiduhfsdhf ")]
        [InlineData("application/json;", " boundary= --iuhfisdhfiuushdfhisudhfhsiduhfsdhf ", " --iuhfisdhfiuushdfhisudhfhsiduhfsdhf ")]
        public void contenttype___boundary_ctor_has_correct_charset(string contentType, string boundary, string expectedBoundary)
        {
            var header = new ContentTypeHeader($"{contentType}{boundary}");

            header.Should().NotBeNull();
            header.Value.Should().Be($"{contentType}{boundary}");
            header.MediaType.Should().Be("application/json");
            header.Type.Should().Be("application");
            header.SubType.Should().Be("json");

            header.Charset.Should().BeEmpty();
            header.Boundary.Should().Be(expectedBoundary);
        }

        [Theory]
        [InlineData("application/json;", " boundary=--iuhfisdhfiuushdfhisudhfhsiduhfsdhf", "--iuhfisdhfiuushdfhisudhfhsiduhfsdhf")]
        [InlineData("application/json;", " boundary=--iuhfisdhfiuushdfhisudhfhsiduhfsdhf ", "--iuhfisdhfiuushdfhisudhfhsiduhfsdhf ")]
        [InlineData("application/json;", " boundary= --iuhfisdhfiuushdfhisudhfhsiduhfsdhf", " --iuhfisdhfiuushdfhisudhfhsiduhfsdhf")]
        [InlineData("application/json;", "boundary= --iuhfisdhfiuushdfhisudhfhsiduhfsdhf ", " --iuhfisdhfiuushdfhisudhfhsiduhfsdhf ")]
        [InlineData("application/json;", " boundary= --iuhfisdhfiuushdfhisudhfhsiduhfsdhf ", " --iuhfisdhfiuushdfhisudhfhsiduhfsdhf ")]
        public void contenttype___boundary_assignment_has_correct_charset(string contentType, string boundary, string expectedBoundary)
        {
            ContentTypeHeader header = $"{contentType}{boundary}";

            header.Should().NotBeNull();
            header.Value.Should().Be($"{contentType}{boundary}");
            header.MediaType.Should().Be("application/json");
            header.Type.Should().Be("application");
            header.SubType.Should().Be("json");

            header.Charset.Should().BeEmpty();
            header.Boundary.Should().Be(expectedBoundary);
        }


        [Theory]
        [InlineData("application/json;", " charset=UTF-8;", " boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=", "--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("application/json;", " charset=UTF-8 ;", " boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf= ", "--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf= ")]
        [InlineData("application/json;", " charset= UTF-8;", " boundary= --iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=", " --iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("application/json;", "charset= UTF-8 ;", "boundary= --iuhfisdhfiuushdfhisudhfhsiduhf=sdhf= ", " --iuhfisdhfiuushdfhisudhfhsiduhf=sdhf= ")]
        [InlineData("application/json;", " charset= UTF-8 ;", " boundary= --iuhfisdhfiuushdfhisudhfhsiduhf=sdhf= ", " --iuhfisdhfiuushdfhisudhfhsiduhf=sdhf= ")]
        public void contenttype___boundaryand_charset_ctor_is_correct(string contentType, string charset, string boundary, string expectedBoundary)
        {
            var header = new ContentTypeHeader($"{contentType}{charset}{boundary}");

            header.Should().NotBeNull();
            header.Value.Should().Be($"{contentType}{charset}{boundary}");
            header.MediaType.Should().Be("application/json");
            header.Type.Should().Be("application");
            header.SubType.Should().Be("json");

            header.Charset.Should().Be("utf-8");
            header.Boundary.Should().Be(expectedBoundary);
        }

        [Theory]
        [InlineData("application/json;", " charset=UTF-8;", " boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=", "--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("application/json;", " charset=UTF-8 ;", " boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf= ", "--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf= ")]
        [InlineData("application/json;", " charset= UTF-8;", " boundary= --iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=", " --iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("application/json;", "charset= UTF-8 ;", "boundary= --iuhfisdhfiuushdfhisudhfhsiduhf=sdhf= ", " --iuhfisdhfiuushdfhisudhfhsiduhf=sdhf= ")]
        [InlineData("application/json;", " charset= UTF-8 ;", " boundary= --iuhfisdhfiuushdfhisudhfhsiduhf=sdhf= ", " --iuhfisdhfiuushdfhisudhfhsiduhf=sdhf= ")]
        public void contenttype___boundaryand_charset_assignment_is_correct(string contentType, string charset, string boundary, string expectedBoundary)
        {
            ContentTypeHeader header = $"{contentType}{charset}{boundary}";

            header.Should().NotBeNull();
            header.Value.Should().Be($"{contentType}{charset}{boundary}");
            header.MediaType.Should().Be("application/json");
            header.Type.Should().Be("application");
            header.SubType.Should().Be("json");

            header.Charset.Should().Be("utf-8");
            header.Boundary.Should().Be(expectedBoundary);
        }

        [Theory]
        [InlineData("application/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("application/Json; charset=uTf-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("APPlication/Json; charset=uTf-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        public void contenttype___equals_operator_header_success(string contentType)
        {
            var contentType1 = new ContentTypeHeader(contentType);
            var contentType2 = new ContentTypeHeader("application/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=");

            var equals = contentType1 == contentType2;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("application/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("application/Json; charset=uTf-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("APPlication/Json; charset=uTf-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        public void contenttype___equals_operator_string_success(string contentType)
        {
            var header = new ContentTypeHeader(contentType);

            var equals = header == "application/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=";
            equals.Should().Be(true);

            equals = "application/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=" == header;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("application/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("application/Json; charset=uTf-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("APPlication/Json; charset=uTf-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        public void contenttype___equals_override_success(string contentType)
        {
            string value = "application/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=";

            var header = new ContentTypeHeader(contentType);

            var equals = header.Equals(value);
            equals.Should().Be(true);

            equals = header.Equals(new ContentTypeHeader(value));
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("application/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("application/Json; charset=uTf-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("text/json; charset=uTf-9; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("text/json; charset=uTf-8; boundary=--9iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("text/json; charset=uTf-8; boundary=-- iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        public void contenttype___notequals_operator_header_success(string contentType)
        {
            var contentType1 = new ContentTypeHeader(contentType);
            var contentType2 = new ContentTypeHeader("text/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=");

            var equals = contentType1 != contentType2;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("application/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("application/Json; charset=uTf-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("text/json; charset=uTf-9; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("text/json; charset=uTf-8; boundary=--9iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("text/json; charset=uTf-8; boundary=-- iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        public void contenttype___notequals_operator_string_success(string contentType)
        {
            var header = new ContentTypeHeader(contentType);

            var equals = header != "text/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=";
            equals.Should().Be(true);

            equals = "text/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=" != header;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("application/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("application/Json; charset=uTf-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("text/json; charset=uTf-9; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("text/json; charset=uTf-8; boundary=--9iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        [InlineData("text/json; charset=uTf-8; boundary=-- iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=")]
        public void contenttype___notequals_override_success(string contentType)
        {
            string value = "text/json; charset=UTF-8; boundary=--iuhfisdhfiuushdfhisudhfhsiduhf=sdhf=";

            var header = new ContentTypeHeader(contentType);

            var equals = header.Equals(value);
            equals.Should().Be(false);

            equals = header.Equals(new ContentTypeHeader(value));
            equals.Should().Be(false);

            equals = value.Equals(header);
            equals.Should().Be(false);
        }
    }
}
