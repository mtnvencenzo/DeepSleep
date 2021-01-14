namespace DeepSleep.Web.Tests
{
    using FluentAssertions;
    using System;
    using Xunit;

    public class DateTimeExtensionsTests
    {
        [Fact]
        public void datetimeextensions___toutcdates_returns_correct_date()
        {
            long epoch = 1609848686;
            var value = epoch.ToUtcDate();

            value.Year.Should().Be(2021);
            value.Month.Should().Be(1);
            value.Day.Should().Be(5);
            value.Hour.Should().Be(12);
            value.Minute.Should().Be(11);
            value.Second.Should().Be(26);
            value.Millisecond.Should().Be(0);
            value.Kind.Should().Be(DateTimeKind.Utc);
        }

        [Fact]
        public void datetimeextensions___toutcdates_returns_epoch_when_max_date()
        {
            long epoch = long.MaxValue;
            var value = epoch.ToUtcDate();

            value.Year.Should().Be(1970);
            value.Month.Should().Be(1);
            value.Day.Should().Be(1);
            value.Hour.Should().Be(0);
            value.Minute.Should().Be(0);
            value.Second.Should().Be(0);
            value.Millisecond.Should().Be(0);
            value.Kind.Should().Be(DateTimeKind.Utc);
        }

        [Fact]
        public void datetimeextensions___change_kind_returns_correct_date()
        {
            var now = DateTime.Now;
            var converted = now.ChangeKind(DateTimeKind.Utc);

            converted.Year.Should().Be(now.Year);
            converted.Month.Should().Be(now.Month);
            converted.Day.Should().Be(now.Day);
            converted.Hour.Should().Be(now.Hour);
            converted.Minute.Should().Be(now.Minute);
            converted.Second.Should().Be(now.Second);
            converted.Millisecond.Should().Be(now.Millisecond);
            converted.Kind.Should().Be(DateTimeKind.Utc);
        }
    }
}
