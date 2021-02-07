namespace DeepSleep.Api.OpenApiCheckTests.v2
{
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class SimpleArrayEndpointsTests : PipelineTestBase
    {
        [Fact]
        public async Task simple_array___get_simple_ilist_int_array_response()
        {
            using (var client = base.GetClient())
            {
                var response = await client.GetSimpleIlistIntArrayResponseAsync(
                    count: 3,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Count.Should().Be(3);
            }
        }

        [Fact]
        public async Task simple_array___post_simple_ilist_int_array_request()
        {
            using (var client = base.GetClient())
            {
                var request = new List<int?>
                {
                    1,
                    2,
                    3
                };

                var response = await client.PostSimpleIlistIntArrayRequestAsync(
                    body: request,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response[0].Should().Be(1);
                response[1].Should().Be(2);
                response[2].Should().Be(3);
            }
        }

        [Fact]
        public async Task simple_array___get_simple_ienumerable_int_array_response()
        {
            using (var client = base.GetClient())
            {
                var response = await client.GetSimpleIenumerableIntArrayResponseAsync(
                    count: 4,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Count.Should().Be(4);
            }
        }

        [Fact]
        public async Task simple_array___post_simple_ienumerable_int_array_request()
        {
            using (var client = base.GetClient())
            {
                var request = new List<int?>
                {
                    1,
                    2,
                    3
                };

                var response = await client.PostSimpleIenumerableIntArrayRequestAsync(
                    body: request,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response[0].Should().Be(1);
                response[1].Should().Be(2);
                response[2].Should().Be(3);
            }
        }

        [Fact]
        public async Task simple_array___get_simple_array_int_array_response()
        {
            using (var client = base.GetClient())
            {
                var response = await client.GetSimpleArrayIntArrayResponseAsync(
                    count: 4,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.Count.Should().Be(4);
            }
        }

        [Fact]
        public async Task simple_array___post_simple_array_int_array_request()
        {
            using (var client = base.GetClient())
            {
                var request = new List<int?>
                {
                    1,
                    2,
                    3
                };

                var response = await client.PostSimpleArrayIntArrayRequestAsync(
                    body: request,
                    cancellationToken: default).ConfigureAwait(false);

                response.Should().NotBeNull();
                response[0].Should().Be(1);
                response[1].Should().Be(2);
                response[2].Should().Be(3);
            }
        }
    }
}
