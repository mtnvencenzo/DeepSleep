namespace DeepSleep.Media.Serializers
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Media.IDeepSleepMediaSerializer" />
    public class DeepSleepJsonMediaSerializer : DeepSleepMediaSerializerBase
    {
        private readonly JsonMediaSerializerConfiguration jsonFormattingConfiguration;

        /// <summary>Initializes a new instance of the <see cref="DeepSleepJsonMediaSerializer" /> class.</summary>
        /// <param name="jsonFormattingConfiguration">The json formatting configuration.</param>
        public DeepSleepJsonMediaSerializer(JsonMediaSerializerConfiguration jsonFormattingConfiguration)
        {
            this.jsonFormattingConfiguration = jsonFormattingConfiguration;
        }

        /// <summary>Whether the formatter can read content
        /// </summary>
        public override bool SupportsRead => true;

        /// <summary>Whether the formatter can write content
        /// </summary>
        public override bool SupportsWrite => true;

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public override IList<string> ReadableMediaTypes => new[] { "application/json", "text/json", "application/json-patch+json" };

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public override IList<string> WriteableMediaTypes => new[] { "application/json", "text/json" };

        /// <summary>Determines whether this instance [can handle type] the specified type.</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if this instance [can handle type] the specified type; otherwise, <c>false</c>.</returns>
        public override bool CanHandleType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            if (Type.GetType(type.AssemblyQualifiedName) == Type.GetType(typeof(MultipartHttpRequest).AssemblyQualifiedName))
            {
                return false;
            }

            if (Type.GetType(type.AssemblyQualifiedName) == Type.GetType(typeof(MultipartHttpRequestSection).AssemblyQualifiedName))
            {
                return false;
            }

            return true;
        }

        /// <summary>Deserializes the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        protected override async Task<object> Deserialize(Stream stream, Type objType, IMediaSerializerOptions options)
        {
            var settings = this.jsonFormattingConfiguration?.SerializerOptions;

            return await JsonSerializer.DeserializeAsync(stream, objType, settings).ConfigureAwait(false);
        }

        /// <summary>Serializes the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        protected override async Task Serialize(Stream stream, object obj, IMediaSerializerOptions options)
        {
            if (obj != null)
            {
                var settings = this.jsonFormattingConfiguration?.SerializerOptions;

                await JsonSerializer.SerializeAsync(stream, obj, settings).ConfigureAwait(false);
            }
        }
    }
}
