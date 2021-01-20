namespace DeepSleep.Tests
{
    using FluentAssertions;
    using Xunit;

    public class NumericExtensionsTests
    {
        [Fact]
        public void numericextensions___isbetween_should_be_true_when_same_as_upper_and_lower()
        {
            100.IsBetween(100, 100).Should().BeTrue();
        }

        [Fact]
        public void numericextensions___isbetween_should_be_false_when_one_less_than_lower()
        {
            99.IsBetween(100, 101).Should().BeFalse();
        }

        [Fact]
        public void numericextensions___isbetween_should_be_false_when_one_more_than_upper()
        {
            102.IsBetween(100, 101).Should().BeFalse();
        }

        [Fact]
        public void numericextensions___isbetween_nullable_should_be_true_when_same_as_upper_and_lower()
        {
            int? val = 100;

            val.IsBetween(100, 100).Should().BeTrue();
        }

        [Fact]
        public void numericextensions___isbetween_nullable_should_be_false_when_one_less_than_lower()
        {
            int? val = 99;

            val.IsBetween(100, 101).Should().BeFalse();
        }

        [Fact]
        public void numericextensions___isbetween_nullable_should_be_false_when_one_more_than_upper()
        {
            int? val = 102;

            val.IsBetween(100, 101).Should().BeFalse();
        }

        [Fact]
        public void numericextensions___isbetween_nullable_should_be_false_when_null()
        {
            int? val = null;

            val.IsBetween(100, 101).Should().BeFalse();
        }
    }
}
