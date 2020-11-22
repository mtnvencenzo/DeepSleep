namespace DeepSleep.Formatting.Formatters
{
    using System;
    using System.Threading.Tasks;
    using System.IO;
    using Newtonsoft.Json;
    using System.Text;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json.Converters;

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
            string data = null;

            Encoding readEncoding = Encoding.Default;

            this.logger?.LogInformation($"Reading data from request stream");

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

            this.logger?.LogDebug($"Data read from stream:");
            this.logger?.LogDebug(data);

            this.logger?.LogDebug($"Deserializing data into type '{objType.FullName}'");
            object obj = JsonConvert.DeserializeObject(data, objType, GetReadSettings());
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
                using (var writer = new StreamWriter(ms))
                {
                    var data = JsonConvert.SerializeObject(obj, GetWriteSettings(options));

                    if (options.Encoding != Encoding.Unicode)
                    {
                        data = options.Encoding
                            .GetString(Encoding.Conver‌​t(Encoding.Unicode, options.Encoding, Encoding.Unicode.GetBytes(data)));
                    }


                    writer.Write(data);
                    writer.Flush();
                    ms.Seek(0, SeekOrigin.Begin);

                    length = ms.Length;

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
        private JsonSerializerSettings GetWriteSettings(IFormatStreamOptions options)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = options.PrettyPrint ? Formatting.Indented : Formatting.None,
                Culture = options.Culture,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                DefaultValueHandling = DefaultValueHandling.Include,
                NullValueHandling = options.NullValuesExcluded ? NullValueHandling.Ignore : NullValueHandling.Include,
                StringEscapeHandling = StringEscapeHandling.Default,
                ContractResolver = new DefaultContractResolver()
            };

            if (settings.Converters == null)
            {
                settings.Converters = new List<JsonConverter>();
            }

            settings.Converters.Add(new StringEnumConverter());
            return settings;
        }

        /// <summary>Gets the write settings.</summary>
        /// <returns></returns>
        private JsonSerializerSettings GetReadSettings()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
            };

            if (settings.Converters == null)
            {
                settings.Converters = new List<JsonConverter>();
            }

            settings.Converters.Add(new StringEnumConverter());
            return settings;
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
