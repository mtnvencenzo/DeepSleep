namespace DeepSleep.Tests.Formatting.Formatters
{
    using DeepSleep.Formatting.Formatters;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Xunit;

    public class JsonHttpFormatterTests
    {
        public class MyType
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public MyType Item { get; set; }
            public IList<MyType> Items { get; set; }
            public bool? MyBool { get; set; }
            public DateTimeOffset? MyDateTimeOffset { get; set; }
            public decimal MyDecimal { get; set; }
            public IList<string> PrimitiveItems { get; set; }
        }

        [Fact]
        public async Task WritesObjectCorretly()
        {
            var loggerMock = new Mock<ILogger<JsonHttpFormatter>>();
            var formatter = new JsonHttpFormatter(loggerMock.Object);
            long length = 0;

            var obj = new MyType
            {
                MyBool = true
            };

            using var ms = new MemoryStream();
            await formatter.WriteType(ms, obj, (l) => length = l).ConfigureAwait(false);

            ms.Length.Should().BeGreaterThan(0);
            length.Should().Be(ms.Length);
        }
    }
}
