namespace DeepSleep.Media
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public abstract class DeepSleepMediaSerializerFactoryBase : IDeepSleepMediaSerializerFactory
    {
        private readonly IServiceProvider serviceProvider;
        private IList<IDeepSleepMediaSerializer> availableFormatters;

        /// <summary>
        /// Prevents a default instance of the <see cref="DeepSleepMediaSerializerFactoryBase"/> class from being created.
        /// </summary>
        public DeepSleepMediaSerializerFactoryBase(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Determines whether this instance [can handle type] the specified formatter type.
        /// </summary>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <param name="type">The type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can handle type] the specified formatter type; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool CanHandleType(string formatterType, string type, string parameters);

        /// <summary>Gets the types.</summary>
        /// <param name="objType">Type of the object.</param>
        /// <param name="overridingFormatters">The overriding formatters.</param>
        /// <returns></returns>
        public virtual IEnumerable<string> GetWriteableTypes(Type objType, IList<IDeepSleepMediaSerializer> overridingFormatters)
        {
            var formatters = (overridingFormatters ?? this.GetFormatters())
                .Where(f => f != null)
                .Where(f => f.CanHandleType(objType))
                .Where(f => f.SupportsWrite);

            foreach (var f in formatters)
            {
                foreach (var type in f.WriteableMediaTypes ?? new List<string>())
                {
                    yield return type.ToLower();
                }
            };
        }

        /// <summary>Gets the readable types.</summary>
        /// <param name="objType">Type of the object.</param>
        /// <param name="overridingFormatters">The overriding formatters.</param>
        /// <returns></returns>
        public virtual IEnumerable<string> GetReadableTypes(Type objType, IList<IDeepSleepMediaSerializer> overridingFormatters)
        {
            var formatters = (overridingFormatters ?? this.GetFormatters())
                .Where(f => f != null)
                .Where(f => f.CanHandleType(objType))
                .Where(f => f.SupportsRead);

            foreach (var f in formatters)
            {
                foreach (var type in f.ReadableMediaTypes ?? new List<string>())
                {
                    yield return type.ToLower();
                }
            };
        }

        /// <summary>Gets the acceptable formatter.</summary>
        /// <param name="acceptHeader">The accept header.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <param name="writeableFormatters">The overriding formatters.</param>
        /// <param name="writeableMediaTypes">The writeable media types.</param>
        /// <returns></returns>
        public virtual Task<IDeepSleepMediaSerializer> GetAcceptableFormatter(
            AcceptHeader acceptHeader,
            Type objType,
            out string formatterType,
            IList<IDeepSleepMediaSerializer> writeableFormatters = null,
            IList<string> writeableMediaTypes = null)
        {
            formatterType = string.Empty;
            IDeepSleepMediaSerializer formatter = null;

            foreach (var mediaHeader in acceptHeader.Values.Where(m => m.Quality > 0))
            {
                formatter = this.Get(
                    contentType: $"{mediaHeader.Type}/{mediaHeader.SubType}",
                    objType: objType,
                    parameters: mediaHeader.ParameterString(),
                    forRead: false,
                    overridingFormatters: writeableFormatters,
                    overridingMediaTypes: writeableMediaTypes,
                    formatterType: out formatterType);

                if (formatter != null)
                    break;
            }

            return Task.FromResult(formatter);
        }

        /// <summary>Gets the content type formatter.</summary>
        /// <param name="mediaHeader">The media header.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <param name="readableFormatters">The overriding formatters.</param>
        /// <param name="readableMediaTypes">The readable media types.</param>
        /// <returns></returns>
        public virtual Task<IDeepSleepMediaSerializer> GetContentTypeFormatter(
            ContentTypeHeader mediaHeader,
            Type objType,
            out string formatterType,
            IList<IDeepSleepMediaSerializer> readableFormatters = null,
            IList<string> readableMediaTypes = null)
        {
            var formatter = this.Get(
                contentType: $"{mediaHeader.Type}/{mediaHeader.SubType}",
                objType: objType,
                parameters: mediaHeader.ParameterString(),
                forRead: true,
                overridingFormatters: readableFormatters,
                overridingMediaTypes: readableMediaTypes,
                formatterType: out formatterType);

            return Task.FromResult(formatter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IList<IDeepSleepMediaSerializer> GetFormatters()
        {
            if (availableFormatters == null)
            {
                if (this.serviceProvider != null)
                {
                    this.availableFormatters = this.serviceProvider.GetServices(typeof(IDeepSleepMediaSerializer))
                        .Select(o => o as IDeepSleepMediaSerializer)
                        .Where(o => o != null)
                        .ToList();
                }
                else
                {
                    this.availableFormatters = new List<IDeepSleepMediaSerializer>();
                }
            }

            return this.availableFormatters;
        }

        /// <summary>Gets the specified type.</summary>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="forRead">if set to <c>true</c> [for read].</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <param name="overridingFormatters">The overriding formatters.</param>
        /// <param name="overridingMediaTypes">The overriding media types.</param>
        /// <returns></returns>
        protected virtual IDeepSleepMediaSerializer Get(
            string contentType,
            Type objType,
            string parameters,
            bool forRead,
            out string formatterType,
            IList<IDeepSleepMediaSerializer> overridingFormatters = null,
            IList<string> overridingMediaTypes = null)
        {
            var formatters = (overridingFormatters ?? GetFormatters() ?? new List<IDeepSleepMediaSerializer>())
                .Where(f => f != null)
                .Where(f => f.CanHandleType(objType))
                .Where(f => (forRead && f.SupportsRead) || (!forRead && f.SupportsWrite));

            foreach (var formatter in formatters)
            {
                var formatterTypes = forRead
                    ? formatter.ReadableMediaTypes ?? new List<string>()
                    : formatter.WriteableMediaTypes ?? new List<string>();

                var supportedTypes = overridingMediaTypes ?? formatterTypes;
                if (supportedTypes.Count == 0)
                {
                    continue;
                }

                foreach (var mediaType in supportedTypes)
                {
                    if (this.CanHandleType(mediaType, contentType, parameters))
                    {
                        if (!formatterTypes.Any(f => string.Equals(f, mediaType, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            continue;
                        }

                        formatterType = mediaType;
                        return formatter;
                    }
                }
            }

            formatterType = string.Empty;
            return null;
        }
    }
}
