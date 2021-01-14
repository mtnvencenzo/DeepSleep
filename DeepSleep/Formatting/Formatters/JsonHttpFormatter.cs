namespace DeepSleep.Formatting.Formatters
{
    using DeepSleep.Configuration;
    using DeepSleep.Formatting.Converters;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Formatting.IFormatStreamReaderWriter" />
    public class JsonHttpFormatter : IFormatStreamReaderWriter
    {
        private readonly IApiServiceConfiguration apiServiceConfiguration;

        /// <summary>Initializes a new instance of the <see cref="JsonHttpFormatter"/> class.</summary>
        /// <param name="apiServiceConfiguration">The API service configuration.</param>
        public JsonHttpFormatter(IApiServiceConfiguration apiServiceConfiguration)
        {
            this.apiServiceConfiguration = apiServiceConfiguration;
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

            var settings = this.apiServiceConfiguration?.JsonFormatterConfiguration?.ReadSerializerOptions ?? GetReaderSettings();

            using (var reader = new StreamReader(stream, true))
            {
                obj = await JsonSerializer.DeserializeAsync(stream, objType, settings).ConfigureAwait(false);
            }

            return obj;
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
        public virtual async Task<long> WriteType(Stream stream, object obj, IFormatStreamOptions options, Action<long> preWriteCallback = null)
        {
            long length = 0;

            if (obj != null)
            {
                var settings = this.apiServiceConfiguration?.JsonFormatterConfiguration?.WriteSerializerOptions ?? GetWriterSettings(options);

                using (var ms = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(ms, obj, settings).ConfigureAwait(false);
                    length = ms.Length;
                    ms.Seek(0, SeekOrigin.Begin);

                    preWriteCallback?.Invoke(length);

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

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public virtual IList<string> ReadableMediaTypes => new[] { "application/json", "text/json", "application/json-patch+json" };

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public virtual IList<string> WriteableMediaTypes => new[] { "application/json", "text/json" };

        /// <summary>Gets the writer settings.</summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static JsonSerializerOptions GetWriterSettings(IFormatStreamOptions options)
        {
            var settings = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                AllowTrailingCommas = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                IgnoreReadOnlyProperties = false,
                IncludeFields = false,
                NumberHandling = JsonNumberHandling.Strict,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReadCommentHandling = JsonCommentHandling.Skip,
                WriteIndented = options?.PrettyPrint ?? false,
            };

            settings.Converters.Add(new NullableBooleanConverter());
            settings.Converters.Add(new BooleanConverter());
            settings.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
            settings.Converters.Add(new NullableTimeSpanConverter());
            settings.Converters.Add(new TimeSpanConverter());
            settings.Converters.Add(new NullableDateTimeConverter());
            settings.Converters.Add(new DateTimeConverter());
            settings.Converters.Add(new NullableDateTimeOffsetConverter());
            settings.Converters.Add(new DateTimeOffsetConverter());
            settings.Converters.Add(new ContentDispositionConverter());
            settings.Converters.Add(new ContentTypeConverter());

            return settings;
        }

        /// <summary>Gets the reader settings.</summary>
        /// <returns></returns>
        public static JsonSerializerOptions GetReaderSettings()
        {
            var settings = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                AllowTrailingCommas = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = true,
                IncludeFields = false,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            };

            settings.Converters.Add(new NullableBooleanConverter());
            settings.Converters.Add(new BooleanConverter());
            settings.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
            settings.Converters.Add(new NullableTimeSpanConverter());
            settings.Converters.Add(new TimeSpanConverter());
            settings.Converters.Add(new NullableDateTimeConverter());
            settings.Converters.Add(new DateTimeConverter());
            settings.Converters.Add(new NullableDateTimeOffsetConverter());
            settings.Converters.Add(new DateTimeOffsetConverter());
            settings.Converters.Add(new ObjectConverter());
            settings.Converters.Add(new ContentDispositionConverter());
            settings.Converters.Add(new ContentTypeConverter());

            return settings;
        }
    }
}
