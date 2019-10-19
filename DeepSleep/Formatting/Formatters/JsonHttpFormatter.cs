namespace DeepSleep.Formatting.Formatters
{
    using System;
    using System.Threading.Tasks;
    using System.IO;
    using Newtonsoft.Json;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Formatting.IFormatStreamReaderWriter" />
    public class JsonHttpFormatter : IFormatStreamReaderWriter
    {
        #region Helpers

        /// <summary>Gets the write settings.</summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private JsonSerializerSettings GetWriteSettings(IFormatStreamOptions options)
        {
            return new JsonSerializerSettings
            {
                Formatting = options.PrettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None,
                Culture = options.Culture,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                DefaultValueHandling = DefaultValueHandling.Include,
                NullValueHandling = NullValueHandling.Include,
                StringEscapeHandling = StringEscapeHandling.Default
            };
        }

        /// <summary>Gets the read settings.</summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private JsonSerializerSettings GetReadSettings(IFormatStreamOptions options)
        {
            return new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                DefaultValueHandling = DefaultValueHandling.Include,
                NullValueHandling = NullValueHandling.Include,
                StringEscapeHandling = StringEscapeHandling.Default,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        #endregion

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns></returns>
        public Task<object> ReadType(Stream stream, Type objType)
        {
            return ReadType(stream, objType, new FormatterOptions());
        }

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public async Task<object> ReadType(Stream stream, Type objType, IFormatStreamOptions options)
        {
            object obj = null;
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

            obj = JsonConvert.DeserializeObject(data, objType);
            return obj;
        }

        /// <summary>Writes the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public Task WriteType(Stream stream, object obj)
        {
            return WriteType(stream, obj, new FormatterOptions());
        }

        /// <summary>Writes the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public Task WriteType(Stream stream, object obj, IFormatStreamOptions options)
        {
            if (obj != null)
            {
                var data = JsonConvert.SerializeObject(obj, GetWriteSettings(options));

                if (options.Encoding != Encoding.Unicode)
                {
                    data = options.Encoding
                        .GetString(Encoding.Conver‌​t(Encoding.Unicode, options.Encoding, Encoding.Unicode.GetBytes(data)));
                }

                var wr = new StreamWriter(stream, options.Encoding);
                wr.Write(data);
                wr.Flush();
            }


            TaskCompletionSource<object> source = new TaskCompletionSource<object>();
            source.SetResult(null);
            return source.Task;
        }

        /// <summary>
        /// Gets a value indicating whether [supports pretty print].
        /// </summary>
        /// <value><c>true</c> if [supports pretty print]; otherwise, <c>false</c>.</value>
        public virtual bool SupportsPrettyPrint => true;
    }
}
