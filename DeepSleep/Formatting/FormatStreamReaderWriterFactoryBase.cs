namespace DeepSleep.Formatting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public abstract class FormatStreamReaderWriterFactoryBase : IFormatStreamReaderWriterFactory
    {
        private readonly List<FormatterDefinition> formatters;
        private string defaultType;

        /// <summary>
        /// Prevents a default instance of the <see cref="FormatStreamReaderWriterFactoryBase"/> class from being created.
        /// </summary>
        public FormatStreamReaderWriterFactoryBase()
        {
            formatters = new List<FormatterDefinition>();
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

        /// <summary>Gets the formatter.</summary>
        /// <param name="type">The type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual IFormatStreamReaderWriter Get(string type, string parameters, out string formatterType)
        {
            formatterType = string.Empty;
            IFormatStreamReaderWriter formatter = null;


            // Check the default first
            if (!string.IsNullOrWhiteSpace(defaultType))
            {
                if (CanHandleType(defaultType, type, parameters))
                {
                    formatter = formatters.FirstOrDefault(f => f.Types.Contains(defaultType))?.Formatter;
                    formatterType = defaultType;
                }
            }

            if (formatter == null)
            {
                foreach (var formatterDef in formatters)
                {
                    foreach (var formatterDefType in formatterDef.Types)
                    {
                        if (CanHandleType(formatterDefType, type, parameters))
                        {
                            formatterType = formatterDefType;
                            formatter = formatterDef.Formatter;
                            break;
                        }
                    }

                    if (formatter != null)
                        break;
                }
            }

            if (formatter == null)
            {
                formatterType = string.Empty;
            }

            return formatter;
        }

        /// <summary>Defaults the specified formatter type.</summary>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        public virtual IFormatStreamReaderWriter Default(out string formatterType)
        {
            if (!string.IsNullOrWhiteSpace(defaultType))
            {
                return Get(defaultType, string.Empty, out formatterType);
            }

            formatterType = string.Empty;
            return null;
        }

        /// <summary>Adds the specified formatter.</summary>
        /// <param name="formatter">The formatter.</param>
        /// <param name="types">The types.</param>
        /// <param name="charsets">The charsets.</param>
        /// <returns></returns>
        public virtual IFormatStreamReaderWriterFactory Add(IFormatStreamReaderWriter formatter, string[] types, string[] charsets)
        {
            if (types != null)
            {
                foreach (var type in types)
                {
                    if (formatters.FirstOrDefault(f => f.Types.Contains(type.ToLower())) != null)
                    {
                        throw new Exception($"A formatter is already registered with type '{type}'");
                    }
                }

                formatters.Add(new FormatterDefinition
                {
                    Formatter = formatter,
                    Types = types.Select(t => t.ToLower()),
                    Charsets = charsets?.Select(c => c.ToLower()) ?? new string[] { }
                });
            }

            return this;
        }

        /// <summary>Removes the specified types.</summary>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        public virtual IFormatStreamReaderWriterFactory Remove(params string[] types)
        {
            if (types != null)
            {
                foreach (var type in types)
                {
                    formatters.RemoveAll(f =>
                    {
                        foreach (var t in f.Types)
                        {
                            if (string.Compare(t, type, true) == 0)
                                return true;
                        }
                        return false;
                    });

                    if (!string.IsNullOrWhiteSpace(defaultType) && string.Compare(defaultType, type, true) == 0)
                    {
                        defaultType = null;
                    }
                }
            }

            return this;
        }

        /// <summary>Gets the types.</summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetTypes()
        {
            foreach(var f in formatters)
            {
                foreach (var type in f.Types)
                {
                    yield return type.ToLower();
                }
            };
        }

        /// <summary>Sets the default.</summary>
        /// <param name="default">The default.</param>
        /// <returns></returns>
        public virtual IFormatStreamReaderWriterFactory SetDefault(string @default)
        {
            if (!string.IsNullOrWhiteSpace(@default) && formatters.FirstOrDefault(f => f.Types.Contains(@default)) == null)
            {
                throw new Exception($"Formater with registered type '{@default}' does not exist");
            }

            defaultType = @default?.ToLower() ?? string.Empty;
            return this;
        }

        /// <summary>Gets the acceptable formatter.</summary>
        /// <param name="mediaHeader">The media header.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        public Task<IFormatStreamReaderWriter> GetAcceptableFormatter(MediaHeaderValueWithQualityString mediaHeader, out string formatterType)
        {
            formatterType = string.Empty;
            IFormatStreamReaderWriter formatter = null;

            foreach (var mediaValue in mediaHeader.Values.Where(m => m.Quality > 0))
            {
                formatter = this.Get($"{mediaValue.Type}/{mediaValue.SubType}", mediaValue.ParameterString(), out formatterType);
                if (formatter != null)
                    break;
            }

            if (formatter == null)
            {
                formatter = this.Default(out formatterType);
            }

            return Task.FromResult(formatter);
        }

        /// <summary>Gets the media type formatter.</summary>
        /// <param name="mediaHeader">The media header.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        public Task<IFormatStreamReaderWriter> GetMediaTypeFormatter(MediaHeaderValueWithParameters mediaHeader, out string formatterType)
        {
            formatterType = string.Empty;
            IFormatStreamReaderWriter formatter = null;
            var mediaValue = mediaHeader.MediaValue;

            formatter = this.Get($"{mediaValue.Type}/{mediaValue.SubType}", mediaValue.ParameterString(), out formatterType);

            return Task.FromResult(formatter);
        }
    }
}
