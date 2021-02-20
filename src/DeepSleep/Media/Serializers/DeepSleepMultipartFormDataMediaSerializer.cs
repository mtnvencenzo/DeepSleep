namespace DeepSleep.Media.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class DeepSleepMultipartFormDataMediaSerializer : DeepSleepMediaSerializerBase
    {
        private readonly IMultipartStreamReader multipartStreamReader;
        private readonly IFormUrlEncodedObjectSerializer formUrlEncodedObjectSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeepSleepMultipartFormDataMediaSerializer"/> class.
        /// </summary>
        /// <param name="multipartStreamReader">The multipart stream reader.</param>
        /// <param name="formUrlEncodedObjectSerializer">The form URL encoded object serializer.</param>
        public DeepSleepMultipartFormDataMediaSerializer(IMultipartStreamReader multipartStreamReader, IFormUrlEncodedObjectSerializer formUrlEncodedObjectSerializer)
        {
            this.multipartStreamReader = multipartStreamReader;
            this.formUrlEncodedObjectSerializer = formUrlEncodedObjectSerializer;
        }

        /// <summary>Whether the formatter can read content
        /// </summary>
        public override bool SupportsRead => true;

        /// <summary>Whether the formatter can write content
        /// </summary>
        public override bool SupportsWrite => false;

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public override IList<string> ReadableMediaTypes => new[] { "multipart/form-data" };

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

            return true;
        }

        /// <summary>URLs the encode.</summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        private string UrlEncode(string s) => System.Web.HttpUtility.UrlEncode(s);

        /// <summary>Deserializes the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        protected override async Task<object> Deserialize(Stream stream, Type objType, IMediaSerializerOptions options)
        {
            MultipartHttpRequest multipart;

            multipart = await this.multipartStreamReader.ReadAsMultipart(stream).ConfigureAwait(false);

            if (multipart == null)
            {
                return multipart;
            }

            if (objType.IsAssignableFrom(multipart.GetType()) || objType.IsSubclassOf(typeof(MultipartHttpRequest)))
            {
                return multipart;
            }

            var simplePartNames = multipart.Sections
                .Where(s => s.IsFileSection() == false)
                .Select(s => s.Name)
                .Distinct()
                .ToList();

            var formUrlEncoded = string.Empty;

            foreach (var partName in simplePartNames)
            {
                var values = multipart.Sections
                    .Where(s => s.Name == partName)
                    .Select(s => s.Value)
                    .ToList();

                formUrlEncoded += $"{UrlEncode(partName)}={UrlEncode(string.Join(",", values))}&";
            }

            var filePartNames = multipart.Sections
                .Where(s => s.IsFileSection() == true)
                .Select(s => s.Name)
                .Distinct()
                .ToList();

            foreach (var partName in filePartNames)
            {
                var fileSections = multipart.Sections
                    .Where(s => s.Name == partName)
                    .ToList();

                for (int i = 0; i < fileSections.Count; i++)
                {
                    var section = fileSections[i];
                    formUrlEncoded += $"{UrlEncode(partName)}[{i}].{nameof(section.TempFileName)}={UrlEncode(section.TempFileName)}&";
                    formUrlEncoded += $"{UrlEncode(partName)}[{i}].{nameof(section.ContentType)}={UrlEncode(section.ContentType)}&";
                    formUrlEncoded += $"{UrlEncode(partName)}[{i}].{nameof(section.ContentDisposition)}={UrlEncode(section.ContentDisposition)}&";
                }
            }


            if (formUrlEncoded.EndsWith("&"))
            {
                formUrlEncoded = formUrlEncoded.Substring(0, formUrlEncoded.Length - 1);
            }

            var customMultipart = await this.formUrlEncodedObjectSerializer.Deserialize(formUrlEncoded, objType, false).ConfigureAwait(false);

            return customMultipart;
        }

        /// <summary>Serializes the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        protected override Task Serialize(Stream stream, object obj, IMediaSerializerOptions options)
        {
            throw new NotSupportedException($"{nameof(DeepSleepMultipartFormDataMediaSerializer)} does not support writing.");
        }
    }
}