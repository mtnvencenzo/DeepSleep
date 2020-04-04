namespace DeepSleep.Formatting
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IFormatStreamReaderWriter
    {
        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns></returns>
        Task<object> ReadType(Stream stream, Type objType);

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<object> ReadType(Stream stream, Type objType, IFormatStreamOptions options);

        /// <summary>Writes the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        Task WriteType(Stream stream, object obj);

        /// <summary>Writes the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task WriteType(Stream stream, object obj, IFormatStreamOptions options);

        /// <summary>
        /// Gets a value indicating whether [supports pretty print].
        /// </summary>
        /// <value><c>true</c> if [supports pretty print]; otherwise, <c>false</c>.</value>
        bool SupportsPrettyPrint { get; }

        /// <summary>
        /// 
        /// </summary>
        IList<string> SuuportedContentTypes { get; }

        /// <summary>
        /// 
        /// </summary>
        IList<string> SuuportedCharsets { get; }
    }
}
