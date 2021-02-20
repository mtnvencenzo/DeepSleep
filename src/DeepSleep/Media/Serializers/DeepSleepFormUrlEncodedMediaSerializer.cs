namespace DeepSleep.Media.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Media.IDeepSleepMediaSerializer" />
    public class DeepSleepFormUrlEncodedMediaSerializer : DeepSleepMediaSerializerBase
    {
        private readonly IFormUrlEncodedObjectSerializer serializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        public DeepSleepFormUrlEncodedMediaSerializer(IFormUrlEncodedObjectSerializer serializer)
        {
            this.serializer = serializer;
        }

        /// <summary>Whether the formatter can read content
        /// </summary>
        public override bool SupportsRead => true;

        /// <summary>Whether the formatter can write content
        /// </summary>
        public override bool SupportsWrite => false;

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public override IList<string> ReadableMediaTypes => new[] { "application/x-www-form-urlencoded" };

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public override IList<string> WriteableMediaTypes => new string[] { };

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
            string data = null;
            Encoding readEncoding = Encoding.Default;

            using (var reader = new StreamReader(stream, true))
            {
                data = await reader.ReadToEndAsync().ConfigureAwait(false);
                readEncoding = reader.CurrentEncoding;
            }

            if (readEncoding.EncodingName != Encoding.Default.EncodingName)
            {
                data = Encoding.Default
                    .GetString(Encoding.Conver‌​t(readEncoding, Encoding.Default, readEncoding.GetBytes(data)));
            }

            var obj = await this.serializer.Deserialize(data, objType).ConfigureAwait(false);
            return obj;
        }

        /// <summary>Serializes the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        protected override Task Serialize(Stream stream, object obj, IMediaSerializerOptions options)
        {
            throw new NotSupportedException($"{nameof(DeepSleepFormUrlEncodedMediaSerializer)} does not support writing.");
        }
    }
}
