namespace DeepSleep.Tests
{
    using FluentAssertions;
    using System;
    using System.Globalization;
    using Xunit;

    public class ContentDispositionHeaderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void contentdisposition___ctor_returns_default_for_null_or_whitespace(string value)
        {
            var header = new ContentDispositionHeader(value);

            header.Should().NotBeNull();
            header.Type.Should().Be(ContentDispositionType.None);
            header.FileName.Should().BeEmpty();
            header.FileNameStar.Should().BeEmpty();
            header.Name.Should().BeEmpty();
            header.Size.Should().BeNull();
            header.CreationDate.Should().BeNull();
            header.ModificationDate.Should().BeNull();
            header.ReadDate.Should().BeNull();
            header.Value.Should().Be(value);
        }

        [Theory]
        [InlineData("none", ContentDispositionType.None)]
        [InlineData("aib", ContentDispositionType.aib)]
        [InlineData("alert", ContentDispositionType.alert)]
        [InlineData("attachment", ContentDispositionType.attachment)]
        [InlineData("by-reference", ContentDispositionType.by_reference)]
        [InlineData("early-session", ContentDispositionType.early_session)]
        [InlineData("form-data", ContentDispositionType.form_data)]
        [InlineData("icon", ContentDispositionType.icon)]
        [InlineData("info-package", ContentDispositionType.info_package)]
        [InlineData("inline", ContentDispositionType.inline)]
        [InlineData("notification", ContentDispositionType.notification)]
        [InlineData("recipient-list", ContentDispositionType.recipient_list)]
        [InlineData("recipient-list-history", ContentDispositionType.recipient_list_history)]
        [InlineData("render", ContentDispositionType.render)]
        [InlineData("session", ContentDispositionType.session)]
        [InlineData("signal", ContentDispositionType.signal)]
        public void contentdisposition___ctor_basic_returns_type(string type, ContentDispositionType expected)
        {
            var header = new ContentDispositionHeader($"{type}");

            header.Should().NotBeNull();
            header.Type.Should().Be(expected);
            header.FileName.Should().BeEmpty();
            header.FileNameStar.Should().BeEmpty();
            header.Name.Should().BeEmpty();
            header.Size.Should().BeNull();
            header.CreationDate.Should().BeNull();
            header.ModificationDate.Should().BeNull();
            header.ReadDate.Should().BeNull();
            header.Value.Should().Be($"{type}");
        }

        [Fact]
        public void contentdisposition___ctor_standard_returns_values()
        {
            string value = "form-data; name=\"test\"; filename=\"MyFile.jpg\"; filename*=\"MyFileName.jpg\"; size=10";

            var header = new ContentDispositionHeader(value);

            header.Should().NotBeNull();
            header.Type.Should().Be(ContentDispositionType.form_data);
            header.FileName.Should().Be("MyFile.jpg");
            header.FileNameStar.Should().Be("MyFileName.jpg");
            header.Name.Should().Be("test");
            header.Size.Should().Be(10);
            header.CreationDate.Should().BeNull();
            header.ModificationDate.Should().BeNull();
            header.ReadDate.Should().BeNull();
            header.Value.Should().Be(value);
        }

        [Fact]
        public void contentdisposition___assignment_standard_returns_values()
        {
            string value = "form-data; name=\"test\"; filename=\"MyFile.jpg\"; filename*=\"MyFileName.jpg\"; size=10";

            ContentDispositionHeader header = value;

            header.Should().NotBeNull();
            header.Type.Should().Be(ContentDispositionType.form_data);
            header.FileName.Should().Be("MyFile.jpg");
            header.FileNameStar.Should().Be("MyFileName.jpg");
            header.Name.Should().Be("test");
            header.Size.Should().Be(10);
            header.CreationDate.Should().BeNull();
            header.ModificationDate.Should().BeNull();
            header.ReadDate.Should().BeNull();
            header.Value.Should().Be(value);
        }

        [Fact]
        public void contentdisposition___ctor_standard_noquotes_returns_values()
        {
            string value = "form-data; name=test; filename=MyFile.jpg; filename*=MyFileName.jpg; size=10";

            var header = new ContentDispositionHeader(value);

            header.Should().NotBeNull();
            header.Type.Should().Be(ContentDispositionType.form_data);
            header.FileName.Should().Be("MyFile.jpg");
            header.FileNameStar.Should().Be("MyFileName.jpg");
            header.Name.Should().Be("test");
            header.Size.Should().Be(10);
            header.CreationDate.Should().BeNull();
            header.ModificationDate.Should().BeNull();
            header.ReadDate.Should().BeNull();
            header.Value.Should().Be(value);
        }

        [Fact]
        public void contentdisposition___assignment_standard_noquotes_returns_values()
        {
            string value = "form-data; name=test; filename=MyFile.jpg; filename*=MyFileName.jpg; size=10";

            ContentDispositionHeader header = value;

            header.Should().NotBeNull();
            header.Type.Should().Be(ContentDispositionType.form_data);
            header.FileName.Should().Be("MyFile.jpg");
            header.FileNameStar.Should().Be("MyFileName.jpg");
            header.Name.Should().Be("test");
            header.Size.Should().Be(10);
            header.CreationDate.Should().BeNull();
            header.ModificationDate.Should().BeNull();
            header.ReadDate.Should().BeNull();
            header.Value.Should().Be(value);
        }

        [Fact]
        public void contentdisposition___ctor_full_returns_values()
        {
            var now = DateTimeOffset.Now;
            var nowString = now.ToString(CultureInfo.InvariantCulture);

            string value = $"form-data; name=\"test\"; filename=\"MyFile.jpg\"; filename*=\"MyFileName.jpg\"; size=10; creation-date=\"{now}\"; modification-date=\"{now}\"; read-date=\"{now}\"; ";

            var header = new ContentDispositionHeader(value);

            header.Should().NotBeNull();
            header.Type.Should().Be(ContentDispositionType.form_data);
            header.FileName.Should().Be("MyFile.jpg");
            header.FileNameStar.Should().Be("MyFileName.jpg");
            header.Name.Should().Be("test");
            header.Size.Should().Be(10);
            header.CreationDate.Should().NotBeNull();
            header.CreationDate.Value.ToString(CultureInfo.InvariantCulture).Should().Be(nowString);
            header.ModificationDate.Should().NotBeNull();
            header.ModificationDate.Value.ToString(CultureInfo.InvariantCulture).Should().Be(nowString);
            header.ReadDate.Should().NotBeNull();
            header.ReadDate.Value.ToString(CultureInfo.InvariantCulture).Should().Be(nowString);
            header.Value.Should().Be(value);
        }

        [Fact]
        public void contentdisposition___assignment_full_returns_values()
        {
            var now = DateTimeOffset.Now;
            var nowString = now.ToString(CultureInfo.InvariantCulture);

            string value = $"form-data; name=\"test\"; filename=\"MyFile.jpg\"; filename*=\"MyFileName.jpg\"; size=10; creation-date=\"{now}\"; modification-date=\"{now}\"; read-date=\"{now}\"; ";

            ContentDispositionHeader header = value;

            header.Should().NotBeNull();
            header.Type.Should().Be(ContentDispositionType.form_data);
            header.FileName.Should().Be("MyFile.jpg");
            header.FileNameStar.Should().Be("MyFileName.jpg");
            header.Name.Should().Be("test");
            header.Size.Should().Be(10);
            header.CreationDate.Should().NotBeNull();
            header.CreationDate.Value.ToString(CultureInfo.InvariantCulture).Should().Be(nowString);
            header.ModificationDate.Should().NotBeNull();
            header.ModificationDate.Value.ToString(CultureInfo.InvariantCulture).Should().Be(nowString);
            header.ReadDate.Should().NotBeNull();
            header.ReadDate.Value.ToString(CultureInfo.InvariantCulture).Should().Be(nowString);
            header.Value.Should().Be(value);
        }

        [Theory]
        [InlineData("form-data; name=\"test\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"")]
        [InlineData("form-Data; name=\"test\"; filename*=\"MyFileName.jpg\"; size=10; creation-date=\"2020-02-04\"; filename=\"MyFile.jpg\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        public void contentdisposition___equals_operator_header_success(string contentDisposition)
        {
            string value = $"form-data; name=\"test\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"";

            var header1 = new ContentDispositionHeader(contentDisposition);
            var header2 = new ContentDispositionHeader(value);


            var equals = header1 == header2;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("form-data; name=\"test\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"")]
        [InlineData("form-Data; name=\"test\"; filename*=\"MyFileName.jpg\"; size=10; creation-date=\"2020-02-04\"; filename=\"MyFile.jpg\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        public void contentdisposition___equals_operator_string_success(string contentDisposition)
        {
            string value = $"form-data; name=\"test\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"";

            var header = new ContentDispositionHeader(contentDisposition);

            var equals = header == value;
            equals.Should().Be(true);

            equals = value == header;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("form-data; name=\"test\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"")]
        [InlineData("form-Data; name=\"test\"; filename*=\"MyFileName.jpg\"; size=10; creation-date=\"2020-02-04\"; filename=\"MyFile.jpg\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        public void contentdisposition___equals_override_success(string contentDisposition)
        {
            string value = $"form-data; name=\"test\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"";

            var header = new ContentDispositionHeader(contentDisposition);

            var equals = header.Equals(value);
            equals.Should().Be(true);

            equals = header.Equals(new ContentDispositionHeader(value));
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("form-data; name=\"Test\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"")]
        [InlineData("form-Data; name=\"test\"; filename*=\"myFileName.jpg\"; size=10; creation-date=\"2020-02-04\"; filename=\"MyFile.jpg\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.JPG\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2021-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2021-02-03\"")]
        [InlineData("form-data; name=\"tester\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2021-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2021-02-02\"; filename*=\"MyssFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data")]
        [InlineData("form-data; name=\"test\"")]
        public void contentdisposition___notequals_operator_header_success(string contentDisposition)
        {
            string value = $"form-data; name=\"test\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"";

            var contentType1 = new ContentDispositionHeader(contentDisposition);
            var contentType2 = new ContentDispositionHeader(value);

            var equals = contentType1 != contentType2;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("form-data; name=\"Test\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"")]
        [InlineData("form-Data; name=\"test\"; filename*=\"myFileName.jpg\"; size=10; creation-date=\"2020-02-04\"; filename=\"MyFile.jpg\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.JPG\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2021-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2021-02-03\"")]
        [InlineData("form-data; name=\"tester\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2021-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2021-02-02\"; filename*=\"MyssFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data")]
        [InlineData("form-data; name=\"test\"")]
        public void contentdisposition___notequals_operator_string_success(string contentType)
        {
            string value = $"form-data; name=\"test\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"";

            var header = new ContentDispositionHeader(contentType);

            var equals = header != value;
            equals.Should().Be(true);

            equals = value != header;
            equals.Should().Be(true);
        }

        [Theory]
        [InlineData("form-data; name=\"Test\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"")]
        [InlineData("form-Data; name=\"test\"; filename*=\"myFileName.jpg\"; size=10; creation-date=\"2020-02-04\"; filename=\"MyFile.jpg\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.JPG\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2021-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2021-02-03\"")]
        [InlineData("form-data; name=\"tester\"; modification-date=\"2020-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2021-02-02\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data; name=\"test\"; modification-date=\"2021-02-02\"; filename*=\"MyssFileName.jpg\"; filename=\"MyFile.jpg\"; size=9; creation-date=\"2020-02-04\"; read-date=\"2020-02-03\"")]
        [InlineData("form-data")]
        [InlineData("form-data; name=\"test\"")]
        public void contentdisposition___notequals_override_success(string contentType)
        {
            string value = $"form-data; name=\"test\"; filename*=\"MyFileName.jpg\"; filename=\"MyFile.jpg\"; size=10; creation-date=\"2020-02-04\"; modification-date=\"2020-02-02\"; read-date=\"2020-02-03\"";

            var header = new ContentDispositionHeader(contentType);

            var equals = header.Equals(value);
            equals.Should().Be(false);

            equals = header.Equals(new ContentDispositionHeader(value));
            equals.Should().Be(false);

            equals = value.Equals(header);
            equals.Should().Be(false);
        }

        [Fact]
        public void contentdisposition___implicit_to_string_from_null_success()
        {
            string value = null as ContentDispositionHeader;

            value.Should().BeNull();
        }

        [Fact]
        public void contentdisposition___null_equals_operator_null_success()
        {
            var equals = (null as ContentDispositionHeader) == (null as ContentDispositionHeader);

            equals.Should().BeTrue();
        }

        [Fact]
        public void contentdisposition___null_not_equals_operator_null_success()
        {
            var equals = (null as ContentDispositionHeader) != (null as ContentDispositionHeader);

            equals.Should().BeFalse();
        }

        [Fact]
        public void contentdisposition___gethashcode_success()
        {
            ContentDispositionHeader header = "form-data; name=\"Test\"";

            header.Should().NotBeNull();

            var hashCode = header.GetHashCode();
            hashCode.Should().NotBe(0);
        }

        [Fact]
        public void contentdisposition___equals_overload_null_sccess()
        {
            ContentDispositionHeader header1 = "form-data; name=\"Test\"";
            ContentDispositionHeader header2 = null;

            var equals = header1.Equals(header2);

            equals.Should().BeFalse();
        }

        [Fact]
        public void contentdisposition___equals_overload_not_string_or_equivelent_sccess()
        {
            ContentDispositionHeader header1 = "form-data; name=\"Test\"";
            var other = 2;

            var equals = header1.Equals(other);

            equals.Should().BeFalse();
        }
    }
}
