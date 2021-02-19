namespace DeepSleep.Tests
{
    using FluentAssertions;
    using System;
    using Xunit;

    public class StringExtensionsTests
    {
        [Fact]
        public void stringextensions___in_should_be_true_for_case_insensitive()
        {
            string mystring = "test";

            mystring.In(StringComparison.OrdinalIgnoreCase, "Test").Should().BeTrue();
        }

        [Fact]
        public void stringextensions___in_should_be_false_for_case_sensitive()
        {
            string mystring = "test";

            mystring.In(StringComparison.Ordinal, "Test").Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void stringextensions___in_should_be_false_for_empty_string(string value)
        {
            value.In(StringComparison.OrdinalIgnoreCase, "Test").Should().BeFalse();
        }

        [Fact]
        public void stringextensions___in_should_be_false_for_empty_values()
        {
            string mystring = "test";

            mystring.In(StringComparison.OrdinalIgnoreCase, new string[] { }).Should().BeFalse();
        }

        [Fact]
        public void stringextensions___in_should_be_false_for_null_values()
        {
            string mystring = "test";

            mystring.In(StringComparison.OrdinalIgnoreCase, null).Should().BeFalse();
        }
    }
}
