namespace DeepSleep.Formatting.Formatters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class MultipartFormDataFormatter : IFormatStreamReaderWriter
    {
        private readonly IMultipartStreamReader multipartStreamReader;
        private readonly IFormUrlEncodedObjectSerializer formUrlEncodedObjectSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipartFormDataFormatter"/> class.
        /// </summary>
        /// <param name="multipartStreamReader">The multipart stream reader.</param>
        /// <param name="formUrlEncodedObjectSerializer">The form URL encoded object serializer.</param>
        public MultipartFormDataFormatter(IMultipartStreamReader multipartStreamReader, IFormUrlEncodedObjectSerializer formUrlEncodedObjectSerializer)
        {
            this.multipartStreamReader = multipartStreamReader;
            this.formUrlEncodedObjectSerializer = formUrlEncodedObjectSerializer;
        }

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns></returns>
        public virtual async Task<object> ReadType(Stream stream, Type objType)
        {
            return await ReadType(stream, objType, new FormatterOptions()).ConfigureAwait(false);
        }

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public virtual async Task<object> ReadType(Stream stream, Type objType, IFormatStreamOptions options)
        {
            MultipartHttpRequest multipart;

            multipart = await this.multipartStreamReader.ReadAsMultipart(stream).ConfigureAwait(false);

            if (multipart == null)
            {
                return multipart;
            }

            if (objType.IsAssignableFrom(multipart.GetType()))
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

        /// <summary>Writes the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="preWriteCallback">The pre write callback.</param>
        /// <returns></returns>
        public virtual async Task<long> WriteType(Stream stream, object obj, Action<long> preWriteCallback = null)
        {
            return await WriteType(stream, obj, new FormatterOptions(), preWriteCallback).ConfigureAwait(false);
        }

        /// <summary>Writes the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <param name="preWriteCallback">The pre write callback.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public virtual Task<long> WriteType(Stream stream, object obj, IFormatStreamOptions options, Action<long> preWriteCallback = null)
        {
            throw new NotSupportedException($"{nameof(MultipartFormDataFormatter)} does not support writing.");
        }

        /// <summary>
        /// Gets a value indicating whether [supports pretty print].
        /// </summary>
        /// <value><c>true</c> if [supports pretty print]; otherwise, <c>false</c>.</value>
        public virtual bool SupportsPrettyPrint => false;

        /// <summary>Whether the formatter can read content
        /// </summary>
        public virtual bool SupportsRead => true;

        /// <summary>Whether the formatter can write content
        /// </summary>
        public virtual bool SupportsWrite => false;

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public virtual IList<string> ReadableMediaTypes => new[] { "multipart/form-data" };

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public virtual IList<string> WriteableMediaTypes => new string[] { };

        /// <summary>URLs the encode.</summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        private string UrlEncode(string s) => System.Web.HttpUtility.UrlEncode(s);
    }
}