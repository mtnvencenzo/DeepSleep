namespace DeepSleep.Tests.PipelineTests
{
    using DeepSleep.Formatting;
    using DeepSleep.Formatting.Formatters;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Moq;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpRequestAcceptTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestAccept(null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRequestInfoIsNull()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = null
            };

            var processed = await context.ProcessHttpRequestAccept(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsEmptyAcceptAllowWhenFormatterFactoryIsNull()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo()
            };

            var processed = await context.ProcessHttpRequestAccept(null).ConfigureAwait(false);
            processed.Should().BeFalse();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(406);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("X-Allow-Accept");
            context.ResponseInfo.Headers[0].Value.Should().Be(string.Empty);

        }

        [Fact]
        public async void ReturnsAllFormatterTypesAcceptAllowWhenFormatterNotFound()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo()
            };

            var mockFormatterFactory = new Mock<IFormatStreamReaderWriterFactory>();
            mockFormatterFactory
                .Setup(m => m.GetTypes())
                .Returns(new string[] { "application/json", "text/xml", "text/plain" });

            var processed = await context.ProcessHttpRequestAccept(mockFormatterFactory.Object).ConfigureAwait(false);
            processed.Should().BeFalse();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(406);
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("X-Allow-Accept");
            context.ResponseInfo.Headers[0].Value.Should().Be("application/json, text/xml, text/plain");

        }

        [Fact]
        public async void ReturnsTrueForFoundFormatter()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo()
            };

            string formatterType;
            var mockFormatter = new Mock<IFormatStreamReaderWriter>();

            var mockFormatterFactory = new Mock<IFormatStreamReaderWriterFactory>();
            mockFormatterFactory
                .Setup(m => m.GetAcceptableFormatter(It.IsAny<MediaHeaderValueWithQualityString>(), out formatterType))
                .Returns(Task.FromResult(mockFormatter.Object));

            var processed = await context.ProcessHttpRequestAccept(mockFormatterFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(0);
        }

        [Theory]
        [InlineData("application/json")]
        [InlineData("application/*")]
        [InlineData("*/*")]
        public async void ReturnsTrueForFoundImplmentedFormatter(string requestAccept)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Accept = new MediaHeaderValueWithQualityString(requestAccept)
                }
            };

            var formatterFactory = new HttpMediaTypeStreamWriterFactory();
            formatterFactory.Add(new JsonHttpFormatter(), new string[] { "application/json" }, null);

            var processed = await context.ProcessHttpRequestAccept(formatterFactory).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(0);
        }
    }
}
