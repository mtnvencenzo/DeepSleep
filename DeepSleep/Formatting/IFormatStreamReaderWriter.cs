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
        /// <param name="preWriteCallback">The pre write callback.</param>
        /// <returns></returns>
        Task<long> WriteType(Stream stream, object obj, Action<long> preWriteCallback = null);

        /// <summary>Writes the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The object.</param>
        /// <param name="options">The options.</param>
        /// <param name="preWriteCallback">The pre write callback.</param>
        /// <returns></returns>
        Task<long> WriteType(Stream stream, object obj, IFormatStreamOptions options, Action<long> preWriteCallback = null);

        /// <summary>
        /// Gets a value indicating whether [supports pretty print].
        /// </summary>
        /// <value><c>true</c> if [supports pretty print]; otherwise, <c>false</c>.</value>
        bool SupportsPrettyPrint { get; }

        /// <summary>Whether the formatter can read content
        /// </summary>
        bool SupportsRead { get; }

        /// <summary>Whether the formatter can write content
        /// </summary>
        bool SupportsWrite { get; }

        /// <summary>Gets the suuported content types.</summary>
        /// <value>The suuported content types.</value>
        IList<string> SuuportedContentTypes { get; }

        /// <summary>Gets the suuported charsets.</summary>
        /// <value>The suuported charsets.</value>
        IList<string> SuuportedCharsets { get; }
    }
}
