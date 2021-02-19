namespace DeepSleep.Media.Serializers
{
    using DeepSleep.Configuration;
    using Microsoft.OpenApi.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Media.IDeepSleepMediaSerializer" />
    public class DeepSleepXmlMediaSerializer : DeepSleepMediaSerializerBase
    {
        private readonly XmlMediaSerializerConfiguration xmlFormattingConfiguration;

        /// <summary>Initializes a new instance of the <see cref="DeepSleepXmlMediaSerializer"/> class.</summary>
        /// <param name="xmlFormattingConfiguration">The XML formatting configuration.</param>
        public DeepSleepXmlMediaSerializer(XmlMediaSerializerConfiguration xmlFormattingConfiguration)
        {
            this.xmlFormattingConfiguration = xmlFormattingConfiguration;
        }

        /// <summary>Whether the formatter can read content
        /// </summary>
        public override bool SupportsRead => true;

        /// <summary>Whether the formatter can write content
        /// </summary>
        public override bool SupportsWrite => true;

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public override IList<string> ReadableMediaTypes => new[] { "text/xml", "application/xml" };

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public override IList<string> WriteableMediaTypes => new[] { "text/xml", "application/xml" };

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
        protected override Task<object> Deserialize(Stream stream, Type objType, IMediaSerializerOptions options)
        {
            object obj = null;
            var serializer = new XmlSerializer(objType);
            var settings = xmlFormattingConfiguration?.ReaderSerializerSettings;

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                obj = serializer.Deserialize(reader);
            }

            return Task.FromResult(obj);
        }

        /// <summary>Serializes the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        protected override Task Serialize(Stream stream, object obj, IMediaSerializerOptions options)
        {
            if (obj != null)
            {
                var serializer = new XmlSerializer(obj.GetType());
                var settings = xmlFormattingConfiguration?.WriterSerializerSettings;

                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, obj);
                }
            }

            return Task.CompletedTask;
        }
    }
}
