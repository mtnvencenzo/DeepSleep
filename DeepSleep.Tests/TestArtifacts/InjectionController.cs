namespace DeepSleep.Tests.TestArtifacts
{
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class InjectionController
    {
        private readonly ILogger logger;

        /// <summary>Injections the controller.</summary>
        /// <param name="logger">The logger.</param>
        public InjectionController(ILogger logger)
        {
            this.logger = logger;
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

        public void DefaultEndpointWithUri([UriBound] StandardModel requestUri)
        {
        }

        public void DefaultEndpointWithBody([BodyBound] StandardModel requestBody)
        {
        }

        public void DefaultEndpointWithUriAndBody([UriBound] StandardModel requestUri, [BodyBound] StandardNullableModel requestBody)
        {
        }

        public void DefaultEndpointWithUriAndBodyAndOthersBefore(string uri, string body, [UriBound] StandardModel requestUri, [BodyBound] StandardNullableModel requestBody)
        {
        }

        public void DefaultEndpointWithUriAndBodyAndOthersAfter([UriBound] StandardModel requestUri, [BodyBound] StandardNullableModel requestBody, string uri, string body)
        {
        }

        public ILogger Logger => logger;
    }
}
