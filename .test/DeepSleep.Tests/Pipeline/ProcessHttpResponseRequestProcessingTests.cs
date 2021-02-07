namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpResponseRequestProcessingTests
    {
        [Fact]
        public async void request_processing___returns_true_and_reqest_processed_handler_called()
        {
            var context = new ApiRequestContext();

            bool reqProcessed = false;

            var config = new DefaultApiServiceConfiguration
            {
                OnRequestProcessed = (ctx) => { reqProcessed = true; return Task.CompletedTask; }
            };


            var processed = await context.ProcessHttpResponseRequestProcessing(new ApiRequestContextResolver(), config).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(200);

            reqProcessed.Should().Be(true);
        }

        [Fact]
        public async void request_processing___returns_true_and_reqest_processed_null_handler_not_called()
        {
            var context = new ApiRequestContext();

            bool reqProcessed = false;

            var config = new DefaultApiServiceConfiguration
            {
                OnRequestProcessed = null
            };


            var processed = await context.ProcessHttpResponseRequestProcessing(new ApiRequestContextResolver(), config).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(200);

            reqProcessed.Should().Be(false);
        }

        [Fact]
        public async void request_processing___returns_true_and_reqest_processed_handler_called_doesnt_fail_if_handler_throws()
        {
            var context = new ApiRequestContext();

            bool reqProcessed = false;

            var config = new DefaultApiServiceConfiguration
            {
                OnRequestProcessed = (ctx) => { reqProcessed = true; throw new Exception("Error"); }
            };


            var processed = await context.ProcessHttpResponseRequestProcessing(new ApiRequestContextResolver(), config).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(200);

            reqProcessed.Should().Be(true);
        }
    }
}
