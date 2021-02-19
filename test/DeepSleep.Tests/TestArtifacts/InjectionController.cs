namespace DeepSleep.Tests.TestArtifacts
{
    using System.Threading.Tasks;

    public class InjectionController
    {
        /// <summary>Injections the controller.</summary>
        /// <param name="logger">The logger.</param>
        public InjectionController()
        {
        }

        public void DefaultEndpoint()
        {
        }

        public void DefaultEndpointInternal()
        {
        }

        private void DefaultEndpointPrivate()
        {
        }

        private void DefaultEndpointProtected()
        {
        }

        public Task DefaultTaskEndpoint()
        {
            return Task.CompletedTask;
        }

        public void DefaultEndpointWithUri([InUri] StandardModel requestUri)
        {
        }

        public void DefaultEndpointWithBody([InBody] StandardModel requestBody)
        {
        }

        public void DefaultEndpointWithUriAndBody([InUri] StandardModel requestUri, [InBody] StandardNullableModel requestBody)
        {
        }

        public void DefaultEndpointWithUriAndBodyAndOthersBefore(string uri, string body, [InUri] StandardModel requestUri, [InBody] StandardNullableModel requestBody)
        {
        }

        public void DefaultEndpointWithUriAndBodyAndOthersAfter([InUri] StandardModel requestUri, [InBody] StandardNullableModel requestBody, string uri, string body)
        {
        }
    }
}
