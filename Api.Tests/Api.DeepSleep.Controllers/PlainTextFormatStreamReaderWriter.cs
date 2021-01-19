namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep.Formatting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Formatting.IFormatStreamReaderWriter" />
    public class PlainTextFormatStreamReaderWriter : IFormatStreamReaderWriter
    {
        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public IList<string> ReadableMediaTypes => new string[] { "text/plain" };

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public IList<string> WriteableMediaTypes => new string[] { "text/plain" };

        /// <summary>Gets a value indicating whether [supports pretty print].</summary>
        /// <value><c>true</c> if [supports pretty print]; otherwise, <c>false</c>.</value>
        public bool SupportsPrettyPrint => false;

        /// <summary>Whether the formatter can read content</summary>
        public bool SupportsRead => true;

        /// <summary>Whether the formatter can write content</summary>
        public bool SupportsWrite => true;

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns></returns>
        public async Task<object> ReadType(Stream stream, Type objType)
        {
            return await ReadType(stream, objType, null);
        }

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public async Task<object> ReadType(Stream stream, Type objType, IFormatStreamOptions options)
        {
            string obj = null;

            using (var reader = new StreamReader(stream, options?.Encoding, true, 1024))
            {
                obj = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            return obj;
        }

        /// <summary>Writes the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="preWriteCallback">The pre write callback.</param>
        /// <returns></returns>
        public async Task<long> WriteType(Stream stream, object obj, Action<long> preWriteCallback = null)
        {
            return await WriteType(stream, obj, preWriteCallback);
        }

        /// <summary>Writes the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <param name="preWriteCallback">The pre write callback.</param>
        /// <returns></returns>
        public async Task<long> WriteType(Stream stream, object obj, IFormatStreamOptions options, Action<long> preWriteCallback = null)
        {
            long length = 0;

            if (obj != null)
            {
                using (var ms = new MemoryStream())
                using (var writer = new StreamWriter(ms, options?.Encoding, 1024))
                {
                    writer.Write(obj.ToString());
                    writer.Flush();
                    length = ms.Length;
                    ms.Seek(0, SeekOrigin.Begin);

                    preWriteCallback?.Invoke(length);

                    await ms.CopyToAsync(stream).ConfigureAwait(false);
                }
            }

            return length;
        }
    }
}
