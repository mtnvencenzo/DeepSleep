namespace DeepSleep.Api.OpenApiCheckTests.v3
{
    using FluentAssertions;
    using System.Threading.Tasks;
    using Xunit;

    public class Int32EndpointsTests : PipelineTestBase
    {
        [Fact]
        public async Task int32_endpoints___post_int32_uri_model_no_doc_attributes()
        {
            using (var client = base.GetClient())
            {
                var response = await client.PostInt32UriModelNoDocAttributesAsync(
                    int32Property: -1,
                    nullableInt32Property: null,
                    uInt32Property: 2,
                    nullableUInt32Property: null).ConfigureAwait(false);

                response.Int32Property.Should().Be(-1);
                response.NullableInt32Property.Should().BeNull();
                response.UInt32Property.Should().Be(2);
                response.NullableUInt32Property.Should().BeNull();
            }
        }

        [Fact]
        public async Task int32_endpoints___get_int32_values_overridden_opid()
        {
            using (var client = base.GetClient())
            {
                var response = await client.GetInt32ValuesOverriddenOpIdAsync(
                    routeint: 10,
                    queryInt1: 1,
                    queryInt2: 2,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Should().Be(13);
            }
        }

        [Fact]
        public async Task int32_endpoints___head_int32_values_overridden_opid()
        {
            using (var client = base.GetClient())
            {
                await client.HeadInt32ValuesOverriddenOpIdAsync(
                    routeint: 10,
                    queryInt1: 1,
                    queryInt2: 2,
                    cancellationToken: default).ConfigureAwait(false);
            }
        }
    }
}
