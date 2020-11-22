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
        /// <summary>Gets the types.</summary>
        /// <returns></returns>
        IEnumerable<string> GetTypes();

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
