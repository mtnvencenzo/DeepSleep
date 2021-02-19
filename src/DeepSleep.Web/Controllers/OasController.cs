namespace DeepSleep.Web.Controllers
{
    using DeepSleep.OpenApi;
    using Microsoft.OpenApi;
    using Microsoft.OpenApi.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class OasController
    {
        private readonly IApiRequestContextResolver contextResolver;

        /// <summary>Initializes a new instance of the <see cref="OasController" /> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public OasController(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver;
        }

        /// <summary>Documents the v2.</summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        internal async Task<OpenApiDocument> DocV2(string format = "json")
        {
            var context = this.contextResolver.GetContext();

            if (string.Compare(format, "yaml", true) == 0)
            {
                context.Configuration.ReadWriteConfiguration.AcceptHeaderOverride = "application/yaml";
            }

            context.TryAddItem("openapi_version", "2");

            var generator = new DeepSleepOasGenerator(context.RequestServices, OpenApiSpecVersion.OpenApi2_0);

            var document = await generator.Generate().ConfigureAwait(false);

            return document;
        }

        /// <summary>Documents the v3.</summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        internal async Task<OpenApiDocument> DocV3(string format = "json")
        {
            var context = this.contextResolver.GetContext();

            if (string.Compare(format, "yaml", true) == 0)
            {
                context.Configuration.ReadWriteConfiguration.AcceptHeaderOverride = "application/yaml";
            }

            context.TryAddItem("openapi_version", "3");

            var generator = new DeepSleepOasGenerator(context.RequestServices, OpenApiSpecVersion.OpenApi3_0);

            var document = await generator.Generate().ConfigureAwait(false);

            return document;
        }
    }
}
