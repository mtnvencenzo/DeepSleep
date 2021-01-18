namespace DeepSleep.OpenApi
{
    using DeepSleep.Formatting;
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
    /// <seealso cref="DeepSleep.Formatting.IFormatStreamReaderWriter" />
    public class OpenApiJsonFormatter : IFormatStreamReaderWriter
    {
        private readonly IApiRequestContextResolver contextResolver;

        /// <summary>Initializes a new instance of the <see cref="OpenApiJsonFormatter"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public OpenApiJsonFormatter(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver;
        }

        /// <summary>Gets a value indicating whether [supports pretty print].</summary>
        /// <value><c>true</c> if [supports pretty print]; otherwise, <c>false</c>.</value>
        public bool SupportsPrettyPrint => true;

        /// <summary>Whether the formatter can read content
        /// </summary>
        public bool SupportsRead => false;

        /// <summary>Whether the formatter can write content
        /// </summary>
        public bool SupportsWrite => true;

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public IList<string> ReadableMediaTypes => new string[] { };

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public virtual IList<string> WriteableMediaTypes => new[] { "application/json", "text/json" };

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns></returns>
        public Task<object> ReadType(Stream stream, Type objType)
        {
            return Task.FromResult(null as object);
        }

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public Task<object> ReadType(Stream stream, Type objType, IFormatStreamOptions options)
        {
            return Task.FromResult(null as object);
        }

        /// <summary>Writes the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="preWriteCallback">The pre write callback.</param>
        /// <returns></returns>
        public async Task<long> WriteType(Stream stream, object obj, Action<long> preWriteCallback = null)
        {
            return await this.WriteType(stream, obj, null, preWriteCallback).ConfigureAwait(false);
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
            var version = string.Empty;

            var context = this.contextResolver?.GetContext();
            context?.TryGetItem("openapi_version", out version);

            if (obj is OpenApiDocument document && document != null)
            {
                using (var ms = new MemoryStream())
                {
                    if (version == "2")
                    {
                        document.Serialize(ms, OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);
                    }
                    else
                    {
                        document.Serialize(ms, OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json);
                    }

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
