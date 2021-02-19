namespace DeepSleep.Media
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IDeepSleepMediaSerializer
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
        Task<object> ReadType(Stream stream, Type objType, IMediaSerializerOptions options);

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
        Task<long> WriteType(Stream stream, object obj, IMediaSerializerOptions options, Action<long> preWriteCallback = null);

        /// <summary>Whether the formatter can read content
        /// </summary>
        bool SupportsRead { get; }

        /// <summary>Whether the formatter can write content
        /// </summary>
        bool SupportsWrite { get; }

        /// <summary>Determines whether this instance [can handle type] the specified type.</summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if this instance [can handle type] the specified type; otherwise, <c>false</c>.</returns>
        bool CanHandleType(Type type);

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        IList<string> ReadableMediaTypes { get; }

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        IList<string> WriteableMediaTypes { get; }
    }
}
