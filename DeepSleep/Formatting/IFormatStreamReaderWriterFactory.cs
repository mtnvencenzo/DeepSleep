namespace DeepSleep.Formatting
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IFormatStreamReaderWriterFactory
    {
        /// <summary>Gets the formatter.</summary>
        /// <param name="type">The type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        IFormatStreamReaderWriter Get(string type, string parameters, out string formatterType);

        /// <summary>Defaults the specified formatter type.</summary>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        IFormatStreamReaderWriter Default(out string formatterType);

        /// <summary>Adds the specified formatter.</summary>
        /// <param name="formatter">The formatter.</param>
        /// <param name="types">The types.</param>
        /// <param name="charsets">The charsets.</param>
        /// <returns></returns>
        IFormatStreamReaderWriterFactory Add(IFormatStreamReaderWriter formatter, string[] types, string[] charsets);

        /// <summary>Removes the specified types.</summary>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        IFormatStreamReaderWriterFactory Remove(params string[] types);

        /// <summary>Gets the types.</summary>
        /// <returns></returns>
        IEnumerable<string> GetTypes();

        /// <summary>Sets the default.</summary>
        /// <param name="default">The default.</param>
        /// <returns></returns>
        IFormatStreamReaderWriterFactory SetDefault(string @default);

        /// <summary>Gets the acceptable formatter.</summary>
        /// <param name="mediaHeader">The media header.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        Task<IFormatStreamReaderWriter> GetAcceptableFormatter(MediaHeaderValueWithQualityString mediaHeader, out string formatterType);

        /// <summary>Gets the media type formatter.</summary>
        /// <param name="mediaHeader">The media header.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        Task<IFormatStreamReaderWriter> GetMediaTypeFormatter(MediaHeaderValueWithParameters mediaHeader, out string formatterType);
    }
}
