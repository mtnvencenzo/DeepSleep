namespace DeepSleep.Api.OpenApiCheckTests.v2
{
    using Models;
    using FluentAssertions;
    using System.Threading.Tasks;
    using Xunit;

    public class EnumEndpointsTests : PipelineTestBase
    {
        [Fact]
        public async Task enum_endpoints___post_enum_uri_model_no_doc_attributes()
        {
            using (var client = base.GetClient())
            {
                var response = await client.PostEnumUriModelNoDocAttributesAsync(
                    explicitEnumProperty: TestEnum.Item1,
                    cancellationToken: default).ConfigureAwait(false);

                response.TestEnumProperty.Should().Be(TestEnum.Item1);
            }
        }

        [Fact]
        public async Task enum_endpoints___put_enum_uri_model_no_doc_attributes()
        {
            using (var client = base.GetClient())
            {
                var response = await client.PutEnumUriModelNoDocAttributesAsync(
                    explicitEnumProperty: TestEnum.Item2,
                    cancellationToken: default).ConfigureAwait(false);

                response.TestEnumProperty.Should().Be(TestEnum.Item2);
            }
        }

        [Fact]
        public async Task enum_endpoints___patch_enum_uri_model_no_doc_attributes()
        {
            using (var client = base.GetClient())
            {
                var response = await client.PatchEnumUriModelNoDocAttributesAsync(
                    explicitEnumProperty: TestEnum.Item2,
                    nullableExplicitEnumProperty: null,
                    cancellationToken: default).ConfigureAwait(false);

                response.TestEnumProperty.Should().Be(TestEnum.Item2);
            }
        }

        [Fact]
        public async Task enum_endpoints___get_enum_in_route_simple_member()
        {
            using (var client = base.GetClient())
            {
                var response = await client.GetEnumInRouteSimpleMemberAsync(
                    enumValue: TestEnum.Item2,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().Be(TestEnum.Item2);
            }
        }

        [Fact]
        public async Task enum_endpoints___head_enum_in_route_simple_member()
        {
            using (var client = base.GetClient())
            {
                // Matching Head Request
                await client.HeadEnumInRouteSimpleMemberAsync(
                    enumValue: TestEnum.Item2,
                    cancellationToken: default).ConfigureAwait(false);
            }
        }
    }
}
