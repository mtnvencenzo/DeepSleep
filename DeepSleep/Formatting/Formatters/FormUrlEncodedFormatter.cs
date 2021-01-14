namespace DeepSleep.Formatting.Formatters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Formatting.IFormatStreamReaderWriter" />
    public class FormUrlEncodedFormatter : IFormatStreamReaderWriter
    {
        private readonly IFormUrlEncodedObjectSerializer serializer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        public FormUrlEncodedFormatter(IFormUrlEncodedObjectSerializer serializer)
        {
            this.serializer = serializer;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        /// <param name="preWriteCallback"></param>
        /// <returns></returns>
        public virtual async Task<long> WriteType(Stream stream, object obj, Action<long> preWriteCallback = null)
        {
            return await WriteType(stream, obj, new FormatterOptions(), preWriteCallback).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <param name="preWriteCallback"></param>
        /// <returns></returns>
        public virtual Task<long> WriteType(Stream stream, object obj, IFormatStreamOptions options, Action<long> preWriteCallback = null)
        {
            throw new NotSupportedException($"{nameof(FormUrlEncodedFormatter)} does not support writing.");
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
        public virtual IList<string> ReadableMediaTypes => new[] { "application/x-www-form-urlencoded" };

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public virtual IList<string> WriteableMediaTypes => new string[] { };
    }
}
