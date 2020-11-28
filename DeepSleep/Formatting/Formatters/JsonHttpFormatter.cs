namespace DeepSleep.Formatting.Formatters
{
    using System;
    using System.Threading.Tasks;
    using System.IO;
    using System.Text;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using DeepSleep.Formatting.Converters;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Formatting.IFormatStreamReaderWriter" />
    public class JsonHttpFormatter : IFormatStreamReaderWriter
    {
        private readonly ILogger logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public JsonHttpFormatter(ILogger<JsonHttpFormatter> logger)
        {
            this.logger = logger;
        }

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns></returns>
        public virtual Task<object> ReadType(Stream stream, Type objType)
        {
            return ReadType(stream, objType, new FormatterOptions());
        }

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public virtual async Task<object> ReadType(Stream stream, Type objType, IFormatStreamOptions options)
        {
            object obj = null;

            using (var reader = new StreamReader(stream, true))
            {
                obj = await JsonSerializer.DeserializeAsync(stream, objType, GetReadSettings()).ConfigureAwait(false);
            }

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
        public virtual async Task<long> WriteType(Stream stream, object obj, IFormatStreamOptions options, Action<long> preWriteCallback = null)
        {
            long length = 0;

            if (obj != null)
            {
                using (var ms = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(ms, obj, GetWriteSettings(options)).ConfigureAwait(false);
                    length = ms.Length;
                    ms.Seek(0, SeekOrigin.Begin);

                    if (preWriteCallback != null)
                    {
                        preWriteCallback(length);
                    }

                    await ms.CopyToAsync(stream).ConfigureAwait(false);
                }
            }

            return length;
        }

        /// <summary>
        /// Gets a value indicating whether [supports pretty print].
        /// </summary>
        /// <value><c>true</c> if [supports pretty print]; otherwise, <c>false</c>.</value>
        public virtual bool SupportsPrettyPrint => true;

        /// <summary>Whether the formatter can read content
        /// </summary>
        public virtual bool SupportsRead => true;

        /// <summary>Whether the formatter can write content
        /// </summary>
        public virtual bool SupportsWrite => true;

        /// <summary>Gets the write settings.</summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private JsonSerializerOptions GetWriteSettings(IFormatStreamOptions options)
        {
            var settings = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                AllowTrailingCommas = false,
                DefaultIgnoreCondition =  JsonIgnoreCondition.WhenWritingNull,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                IgnoreReadOnlyFields = false,
                IgnoreReadOnlyProperties = false,
                IncludeFields = false,
                NumberHandling = JsonNumberHandling.Strict,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReadCommentHandling = JsonCommentHandling.Skip,
                WriteIndented = options.PrettyPrint
            };


            settings.Converters.Add(new JsonStringEnumConverter());

            return settings;
        }

        /// <summary>Gets the write settings.</summary>
        /// <returns></returns>
        private JsonSerializerOptions GetReadSettings()
        {
            return JsonReaderSerializationOptions.ReaderOptions;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IList<string> SuuportedContentTypes => new string[] { "application/json", "text/json", "application/json-patch+json" };

        /// <summary>
        /// 
        /// </summary>
        public virtual IList<string> SuuportedCharsets  => new string[] { "utf-32, utf-16, utf-8" };
    }
}
