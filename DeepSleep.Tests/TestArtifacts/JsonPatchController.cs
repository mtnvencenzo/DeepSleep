namespace DeepSleep.Tests.TestArtifacts
{
    using DeepSleep.JsonPatch;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class JsonPatchController
    {
        private readonly ILogger logger;

        /// <summary>Injections the controller.</summary>
        /// <param name="logger">The logger.</param>
        public JsonPatchController(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="requestBody"></param>
        public Task<StandardNullableModel> Patch([UriBound] StandardModel requestUri, [BodyBound] IList<JsonPatchOperation> requestBody)
        {
            return Task.FromResult(new StandardNullableModel());
        }

        public ILogger Logger => logger;
    }
}
