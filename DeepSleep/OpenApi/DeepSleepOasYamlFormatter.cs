namespace DeepSleep.OpenApi
{
    using DeepSleep.Media;
    using DeepSleep.Media.Serializers;
    using Microsoft.OpenApi;
    using Microsoft.OpenApi.Extensions;
    using Microsoft.OpenApi.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Media.IDeepSleepMediaSerializer" />
    public class DeepSleepOasYamlFormatter : DeepSleepMediaSerializerBase
    {
        private readonly IApiRequestContextResolver contextResolver;

        /// <summary>Initializes a new instance of the <see cref="DeepSleepOasYamlFormatter"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public DeepSleepOasYamlFormatter(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver;
        }

        /// <summary>Whether the formatter can read content
        /// </summary>
        public override bool SupportsRead => false;

        /// <summary>Whether the formatter can write content
        /// </summary>
        public override bool SupportsWrite => true;

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public override IList<string> ReadableMediaTypes => new string[] { };

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public override IList<string> WriteableMediaTypes => new[] 
        { 
            "text/x-yaml", 
            "text/yaml", 
            "text/yml", 
            "application/x-yaml", 
            "application/x-yml", 
            "application/yaml", 
            "application/yml", 
            "application/vnd.yaml", 
            "text/vnd.yaml" 
        };

        /// <summary>Determines whether this instance [can handle type] the specified type.</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if this instance [can handle type] the specified type; otherwise, <c>false</c>.</returns>
        public override bool CanHandleType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (Type.GetType(type.AssemblyQualifiedName) == Type.GetType(typeof(OpenApiDocument).AssemblyQualifiedName))
            {
                return true;
            }

            return false;
        }

        /// <summary>Deserializes the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        protected override Task<object> Deserialize(Stream stream, Type objType, IMediaSerializerOptions options)
        {
            return Task.FromResult(null as object);
        }

        /// <summary>Serializes the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        protected override Task Serialize(Stream stream, object obj, IMediaSerializerOptions options)
        {
            var version = string.Empty;

            var context = this.contextResolver?.GetContext();
            context?.TryGetItem("openapi_version", out version);

            if (obj is OpenApiDocument document && document != null)
            {
                if (version == "2")
                {
                    document.Serialize(
                        stream: stream,
                        specVersion: OpenApiSpecVersion.OpenApi2_0,
                        format: OpenApiFormat.Yaml);
                }
                else
                {
                    document.Serialize(
                        stream: stream,
                        specVersion: OpenApiSpecVersion.OpenApi3_0,
                        format: OpenApiFormat.Yaml);
                }
            }

            return Task.CompletedTask;
        }
    }
}
