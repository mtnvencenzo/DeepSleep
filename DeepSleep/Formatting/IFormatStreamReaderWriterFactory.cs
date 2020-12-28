namespace DeepSleep.Formatting
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IFormatStreamReaderWriterFactory
    {
        /// <summary>Gets the writeable types.</summary>
        /// <param name="overridingFormatters">The overriding formatters.</param>
        /// <returns></returns>
        IEnumerable<string> GetWriteableTypes(IList<IFormatStreamReaderWriter> overridingFormatters);

        /// <summary>Gets the readably types.</summary>
        /// <returns></returns>
        IEnumerable<string> GetReadableTypes(IList<IFormatStreamReaderWriter> overridingFormatters);

        /// <summary>Gets the acceptable formatter.</summary>
        /// <param name="acceptHeader">The accept header.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <param name="writeableFormatters">The overriding formatters.</param>
        /// <param name="writeableMediaTypes">The writeable media types.</param>
        /// <returns></returns>
        Task<IFormatStreamReaderWriter> GetAcceptableFormatter(AcceptHeader acceptHeader, out string formatterType, IList<IFormatStreamReaderWriter> writeableFormatters = null, IList<string> writeableMediaTypes = null);

        /// <summary>Gets the content type formatter.</summary>
        /// <param name="contentTypeHeader">The content type header.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <param name="readableFormatters">The overriding formatters.</param>
        /// <param name="readableMediaTypes">The readable media types.</param>
        /// <returns></returns>
        Task<IFormatStreamReaderWriter> GetContentTypeFormatter(ContentTypeHeader contentTypeHeader, out string formatterType, IList<IFormatStreamReaderWriter> readableFormatters = null, IList<string> readableMediaTypes = null);
    }
}
