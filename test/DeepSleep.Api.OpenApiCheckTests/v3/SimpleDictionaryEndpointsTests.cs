namespace DeepSleep.Api.OpenApiCheckTests.v3
{
    using FluentAssertions;
    using System.Threading.Tasks;
    using Xunit;

    public class SimpleDictionaryEndpointsTests : PipelineTestBase
    {
        [Fact]
        public async Task simple_dictionary___get_simple_idictionary_string_string_response()
        {
            using (var client = base.GetClient())
            {
                var response = await client.GetSimpleIdctionaryStringStringResponseAsync(
                    count: 3,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Count.Should().Be(3);
                response.ContainsKey("0").Should().BeTrue();
                response.ContainsKey("1").Should().BeTrue();
                response.ContainsKey("2").Should().BeTrue();

                response["0"].Should().Be("0");
                response["1"].Should().Be("1");
                response["2"].Should().Be("2");
            }
        }

        [Fact]
        public async Task simple_dictionary___get_simple_idictionary_int_string_response()
        {
            using (var client = base.GetClient())
            {
                var response = await client.GetSimpleIdctionaryIntStringResponseAsync(
                    count: 3,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Count.Should().Be(3);
                response.ContainsKey("0").Should().BeTrue();
                response.ContainsKey("1").Should().BeTrue();
                response.ContainsKey("2").Should().BeTrue();

                response["0"].Should().Be("0");
                response["1"].Should().Be("1");
                response["2"].Should().Be("2");
            }
        }

        [Fact]
        public async Task object_dictionary___get_object_idictionary_string_dictionaryobject_response()
        {
            using (var client = base.GetClient())
            {
                var response = await client.GetObjectIdctionaryStringDictionaryobjectResponseAsync(
                    count: 3,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Count.Should().Be(3);
                response.ContainsKey("0").Should().BeTrue();
                response.ContainsKey("1").Should().BeTrue();
                response.ContainsKey("2").Should().BeTrue();

                response["0"].Id.Should().Be(0);
                response["0"].Items.Should().NotBeNull();
                response["0"].Items.Should().HaveCount(1);
                response["0"].Items["Key1"].Should().Be("Value1");

                response["1"].Id.Should().Be(1);
                response["1"].Items.Should().NotBeNull();
                response["1"].Items.Should().HaveCount(1);
                response["1"].Items["Key1"].Should().Be("Value1");

                response["2"].Id.Should().Be(2);
                response["2"].Items.Should().NotBeNull();
                response["2"].Items.Should().HaveCount(1);
                response["2"].Items["Key1"].Should().Be("Value1");
            }
        }
    }
}
