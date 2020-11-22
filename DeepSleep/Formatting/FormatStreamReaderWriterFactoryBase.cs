namespace DeepSleep.Formatting
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public abstract class FormatStreamReaderWriterFactoryBase : IFormatStreamReaderWriterFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger logger;
        private IList<IFormatStreamReaderWriter> availableFormatters;

        /// <summary>
        /// Prevents a default instance of the <see cref="FormatStreamReaderWriterFactoryBase"/> class from being created.
        /// </summary>
        public FormatStreamReaderWriterFactoryBase(IServiceProvider serviceProvider, ILogger logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
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
        /// <returns></returns>
        public virtual IEnumerable<string> GetTypes()
        {
            var formatters = this.GetFormatters();

            foreach(var f in formatters)
            {
                foreach (var type in f.SuuportedContentTypes)
                {
                    yield return type.ToLower();
                }
            };
        }

        /// <summary>Gets the acceptable formatter.</summary>
        /// <param name="mediaHeader">The media header.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        public virtual Task<IFormatStreamReaderWriter> GetAcceptableFormatter(MediaHeaderValueWithQualityString mediaHeader, out string formatterType)
        {
            formatterType = string.Empty;
            IFormatStreamReaderWriter formatter = null;

            foreach (var mediaValue in mediaHeader.Values.Where(m => m.Quality > 0))
            {
                formatter = this.Get($"{mediaValue.Type}/{mediaValue.SubType}", mediaValue.ParameterString(), true, out formatterType);
                if (formatter != null)
                    break;
            }

            return Task.FromResult(formatter);
        }

        /// <summary>Gets the media type formatter.</summary>
        /// <param name="mediaHeader">The media header.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        public virtual Task<IFormatStreamReaderWriter> GetMediaTypeFormatter(MediaHeaderValueWithParameters mediaHeader, out string formatterType)
        {
            var mediaValue = mediaHeader.MediaValue;

            var formatter = this.Get($"{mediaValue.Type}/{mediaValue.SubType}", mediaValue.ParameterString(), false, out formatterType);

            return Task.FromResult(formatter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IList<IFormatStreamReaderWriter> GetFormatters()
        {
            if (availableFormatters == null)
            {
                this.availableFormatters = this.serviceProvider.GetServices(typeof(IFormatStreamReaderWriter))
                    .Select(o => o as IFormatStreamReaderWriter)
                    .Where(o => o != null)
                    .ToList();
            }

            return this.availableFormatters;
        }

        /// <summary>Gets the formatter.</summary>
        /// <param name="type">The type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="forRead">true if for reading false for writing.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected virtual IFormatStreamReaderWriter Get(string type, string parameters, bool forRead, out string formatterType)
        {
            formatterType = string.Empty;
            IFormatStreamReaderWriter foundFormatter = null;

            var formatters = GetFormatters()
                .Where(f => (forRead && f.SupportsRead) || (!forRead && f.SupportsWrite))
                .ToList();

            if (foundFormatter == null)
            {
                foreach (var formatter in formatters)
                {
                    foreach (var contentType in formatter.SuuportedContentTypes)
                    {
                        if (CanHandleType(contentType, type, parameters))
                        {
                            formatterType = contentType;
                            foundFormatter = formatter;
                            break;
                        }
                    }

                    if (foundFormatter != null)
                        break;
                }
            }

            if (foundFormatter == null)
            {
                formatterType = string.Empty;
            }

            return foundFormatter;
        }
    }
}
