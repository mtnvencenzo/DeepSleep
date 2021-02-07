namespace DeepSleep.Api.OpenApiCheckTests.v3
{
    using System.Threading.Tasks;
    using Xunit;

    public class VoidEndpointsTests : PipelineTestBase
    {
        [Fact]
        public async Task void_endpoints___post_basic_object_model_return_void()
        {
            using (var client = base.GetClient())
            {
                var body = new Models.BasicObject
                {
                };

                await client.PostBasicObjectModelReturnVoidAsync(
                    cancellationToken: default,
                    body: body).ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task void_endpoints___post_basic_object_model_return_task()
        {
            using (var client = base.GetClient())
            {
                var body = new Models.BasicObject
                {
                };

                await client.PostBasicObjectModelReturnTaskAsync(
                    cancellationToken: default,
                    body: body).ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task void_endpoints___post_basic_object_model_return_task_202_attribute()
        {
            using (var client = base.GetClient())
            {
                var body = new Models.BasicObject
                {
                };

                await client.PostBasicObjectModelReturnTaskWith202AttributeAsync(
                    cancellationToken: default,
                    body: body).ConfigureAwait(false);
            }
        }
    }
}
