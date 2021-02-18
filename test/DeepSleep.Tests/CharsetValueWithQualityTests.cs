namespace DeepSleep.Tests
{
    using FluentAssertions;
    using System.Collections.Generic;
    using Xunit;

    public class CharsetValueWithQualityTests
    {
        [Fact]
        public void charsetvaluewithquality___returns_tostring_with_parameters()
        {
            var value = new CharsetValueWithQuality("iso", 1, new List<string>
            {
                "p1=testparam1",
                "q=2.0",
                "p2=testparam2"
            });

            value.Parameters.Count.Should().Be(3);

            var paramString = value.ParameterString();
            paramString.Should().Be("; p1=testparam1; p2=testparam2");

            var tostring = value.ToString();
            tostring.Should().Be("iso; q=1; p1=testparam1; p2=testparam2");
        }
    }
}
