namespace DeepSleep.Media
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IDeepSleepMediaSerializerFactory
    {
        /// <summary>Gets the writeable types.</summary>
        /// <param name="objType">Type of the object.</param>
        /// <param name="overridingFormatters">The overriding formatters.</param>
        /// <returns></returns>
        IEnumerable<string> GetWriteableTypes(Type objType, IList<IDeepSleepMediaSerializer> overridingFormatters);

        /// <summary>Gets the readably types.</summary>
        /// <param name="objType">Type of the object.</param>
        /// <param name="overridingFormatters">The overriding formatters.</param>
        /// <returns></returns>
        IEnumerable<string> GetReadableTypes(Type objType, IList<IDeepSleepMediaSerializer> overridingFormatters);

        /// <summary>Gets the acceptable formatter.</summary>
        /// <param name="acceptHeader">The accept header.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <param name="writeableFormatters">The overriding formatters.</param>
        /// <param name="writeableMediaTypes">The writeable media types.</param>
        /// <returns></returns>
        Task<IDeepSleepMediaSerializer> GetAcceptableFormatter(
            AcceptHeader acceptHeader,
            Type objType,
            out string formatterType, 
            IList<IDeepSleepMediaSerializer> writeableFormatters = null, 
            IList<string> writeableMediaTypes = null);

        /// <summary>Gets the content type formatter.</summary>
        /// <param name="contentTypeHeader">The content type header.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <param name="readableFormatters">The overriding formatters.</param>
        /// <param name="readableMediaTypes">The readable media types.</param>
        /// <returns></returns>
        Task<IDeepSleepMediaSerializer> GetContentTypeFormatter(
            ContentTypeHeader contentTypeHeader,
            Type objType,
            out string formatterType, 
            IList<IDeepSleepMediaSerializer> readableFormatters = null, 
            IList<string> readableMediaTypes = null);
    }
}
