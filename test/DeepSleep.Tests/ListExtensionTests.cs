namespace DeepSleep.Tests
{
    using DeepSleep;
    using FluentAssertions;
    using System.Collections.Generic;
    using Xunit;

    public class ListExtensionTests
    {
        [Fact]
        public void listextensions___concatenate_empty_list_returns_empty_string()
        {
            var list = new List<string>();

            var result = list.Concatenate(",");

            result.Should().Be(string.Empty);
        }

        [Fact]
        public void listextensions___concatenate_null_list_returns_empty_string()
        {
            var list = null as List<string>;

            var result = list.Concatenate(",");

            result.Should().Be(string.Empty);
        }
    }
}
