﻿namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Formatting;
    using DeepSleep.Formatting.Formatters;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Moq;
    using System.Collections.Generic;
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
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(406);
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
                .Setup(m => m.GetWriteableTypes(It.IsAny<IList<IFormatStreamReaderWriter>>()))
                .Returns(new string[] { "application/json", "text/xml", "text/plain" });

            var processed = await context.ProcessHttpRequestAccept(mockFormatterFactory.Object).ConfigureAwait(false);
            processed.Should().BeFalse();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(406);
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
                .Setup(m => m.GetAcceptableFormatter(It.IsAny<AcceptHeader>(), out formatterType, It.IsAny<IList<IFormatStreamReaderWriter>>(), It.IsAny<IList<string>>()))
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
                    Accept = new AcceptHeader(requestAccept)
                }
            };

            var formatter = SetupJsonFormatterMock(null, new string[] { "application/json" });
            var mockFactory = SetupFormatterFactory(formatter.Object);


            var processed = await context.ProcessHttpRequestAccept(mockFactory.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(0);
        }

        private Mock<HttpMediaTypeStreamReaderWriterFactory> SetupFormatterFactory(params IFormatStreamReaderWriter[] formatters)
        {
            var mockFactory = new Mock<HttpMediaTypeStreamReaderWriterFactory>(new object[] { null })
            {
                CallBase = true
            };

            mockFactory.Setup(m => m.GetFormatters())
                .Returns(new List<IFormatStreamReaderWriter>(formatters));

            return mockFactory;
        }

        private Mock<JsonHttpFormatter> SetupJsonFormatterMock(string[] readableTypes, string[] writeableTypes)
        {
            var mockFormatter = new Mock<JsonHttpFormatter>(new object[] { null })
            {
                CallBase = true
            };
            mockFormatter.Setup(m => m.ReadableMediaTypes).Returns(readableTypes);
            mockFormatter.Setup(m => m.WriteableMediaTypes).Returns(writeableTypes);
            return mockFormatter;
        }
    }
}
