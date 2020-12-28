﻿namespace DeepSleep.Formatting.Formatters
{
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
        private readonly IJsonFormattingConfiguration jsonFormattingConfiguration;

        /// <summary>Initializes a new instance of the <see cref="JsonHttpFormatter"/> class.</summary>
        /// <param name="jsonFormattingConfiguration">The json formatting configuration.</param>
        public JsonHttpFormatter(IJsonFormattingConfiguration jsonFormattingConfiguration)
        {
            this.jsonFormattingConfiguration = jsonFormattingConfiguration;
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
                using (var ms = new MemoryStream())
                {
                    await JsonSerializer.SerializeAsync(ms, obj, GetWriteSettings(options)).ConfigureAwait(false);
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

        /// <summary>Gets the write settings.</summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private JsonSerializerOptions GetWriteSettings(IFormatStreamOptions options)
        {
            JsonNamingPolicy jsonNamingPolicy = null;
            var casing = this.jsonFormattingConfiguration?.CasingStyle ?? FormatCasingStyle.CamelCase;
            var nullValuesExcluded = this.jsonFormattingConfiguration?.NullValuesExcluded ?? true;

            if (casing == FormatCasingStyle.CamelCase)
            {
                jsonNamingPolicy = JsonNamingPolicy.CamelCase;
            }

            var settings = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                AllowTrailingCommas = false,
                DefaultIgnoreCondition = nullValuesExcluded == true ? JsonIgnoreCondition.WhenWritingNull : JsonIgnoreCondition.Never,
                IgnoreReadOnlyProperties = false,
                IncludeFields = false,
                NumberHandling = JsonNumberHandling.Strict,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = jsonNamingPolicy,
                ReadCommentHandling = JsonCommentHandling.Skip,
                WriteIndented = options?.PrettyPrint ?? false
            };

            settings.Converters.Add(new JsonStringEnumConverter(jsonNamingPolicy, false));
            settings.Converters.Add(new TimeSpanConverter());
            settings.Converters.Add(new NullableTimeSpanConverter());
            settings.Converters.Add(new ContentDispositionConverter());
            settings.Converters.Add(new ContentTypeConverter());

            return settings;
        }

        /// <summary>Gets the write settings.</summary>
        /// <returns></returns>
        private JsonSerializerOptions GetReadSettings()
        {
            return JsonReaderSerializationOptions.ReaderOptions;
        }

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public virtual IList<string> ReadableMediaTypes => new[] { "application/json", "text/json", "application/json-patch+json" };

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public virtual IList<string> WriteableMediaTypes => new[] { "application/json", "text/json" };
    }
}
