namespace DeepSleep.Api.OpenApiCheckTests.v3
{
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class SimpleArrayQueryStringEndpointsTests : PipelineTestBase
    {
        [Fact]
        public async Task simple_array_querystring___get_simple_ilist_int_array_querystring()
        {
            using (var client = base.GetClient())
            {
                var items = new List<int?>
                {
                    1,
                    2,
                    3
                };

                var response = await client.GetSimpleIlistIntArrayQuerystringAsync(
                    queryItems: items,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Should().Be(items.Count);
            }
        }

        [Fact]
        public async Task simple_array_querystring___get_simple_ienumerable_int_array_querystring()
        {
            using (var client = base.GetClient())
            {
                var items = new List<int?>
                {
                    1,
                    2,
                    3
                };

                var response = await client.GetSimpleIenumerableIntArrayQuerystringAsync(
                    queryItems: items,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Should().Be(items.Count);
            }
        }

        [Fact]
        public async Task simple_array_querystring___get_simple_array_int_array_querystring()
        {
            using (var client = base.GetClient())
            {
                var items = new List<int?>
                {
                    1,
                    2,
                    3
                };

                var response = await client.GetSimpleArrayIntArrayQuerystringAsync(
                    queryItems: items,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Should().Be(items.Count);
            }
        }
    }
}
