namespace DeepSleep.Tests.Media.Serializers
{
    using DeepSleep.Media.Serializers;
    using FluentAssertions;
    using Xunit;

    public class PreSerializationResultTests
    {
        [Fact]
        public void preserializationresult___handled()
        {
            var result = PreSerializationResult.Handled(1);

            result.Should().NotBeNull();
            result.ObjectResult.Should().Be(1);
            result.HasRead.Should().Be(true);
        }

        [Fact]
        public void preserializationresult___nothandled()
        {
            var result = PreSerializationResult.NotHandled();

            result.Should().NotBeNull();
            result.ObjectResult.Should().BeNull();
            result.HasRead.Should().Be(false);
        }
    }
}
