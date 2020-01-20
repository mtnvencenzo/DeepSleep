namespace DeepSleep.Formatting.Formatters
{
    using System;
    using System.Threading.Tasks;
    using System.IO;
    using System.Xml.Serialization;
    using System.Xml;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Formatting.IFormatStreamReaderWriter" />
    public class XmlHttpFormatter : IFormatStreamReaderWriter
    {
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
        public Task<object> ReadType(Stream stream, Type objType, IFormatStreamOptions options)
        {
            object obj = null;
            var serializer = new XmlSerializer(objType);
            var settings = new XmlReaderSettings
            {
                CloseInput = false,
                ConformanceLevel = ConformanceLevel.Fragment,
                IgnoreComments = true,
                ValidationType = ValidationType.None
            };


            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                obj = serializer.Deserialize(reader);
            }


            TaskCompletionSource<object> source = new TaskCompletionSource<object>();
            source.SetResult(obj);
            return source.Task;
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
                var serializer = new XmlSerializer(obj.GetType());
                var settings = new XmlWriterSettings
                {
                    NewLineOnAttributes = false,
                    CloseOutput = false,
                    Encoding = options.Encoding,
                    Indent = options.PrettyPrint,
                    NamespaceHandling = NamespaceHandling.Default,
                    OmitXmlDeclaration = true
                };

                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, obj);
                    writer.Flush();
                }
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

        /// <summary>Internals the type of the write.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        private Task InternalWriteType(Stream stream, object obj, IFormatStreamOptions options)
        {
            if (obj != null)
            {
                var serializer = new XmlSerializer(obj.GetType());
                var settings = new XmlWriterSettings
                {
                    NewLineOnAttributes = false,
                    CloseOutput = false,
                    Encoding = Encoding.UTF8,
                    Indent = options.PrettyPrint,
                    NamespaceHandling = NamespaceHandling.Default,
                    OmitXmlDeclaration = true
                };

                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, obj);
                    writer.Flush();
                }
            }


            TaskCompletionSource<object> source = new TaskCompletionSource<object>();
            source.SetResult(null);
            return source.Task;
        }
    }
}
