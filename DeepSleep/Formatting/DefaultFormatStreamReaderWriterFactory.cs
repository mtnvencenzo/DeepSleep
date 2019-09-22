using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepSleep.Formatting
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DefaultFormatStreamReaderWriterFactory : IFormatStreamReaderWriterFactory
    {
        #region Constructors & Initialization

        /// <summary>
        /// Prevents a default instance of the <see cref="DefaultFormatStreamReaderWriterFactory"/> class from being created.
        /// </summary>
        public DefaultFormatStreamReaderWriterFactory()
        {
            _formatters = new List<FormatterDefinition>();
        }

        private List<FormatterDefinition> _formatters;

        private string _defaultType;

        #endregion

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
            if (!string.IsNullOrWhiteSpace(_defaultType))
            {
                if (CanHandleType(_defaultType, type, parameters))
                {
                    formatter = _formatters.FirstOrDefault(f => f.Types.Contains(_defaultType))?.Formatter;
                    formatterType = _defaultType;
                }
            }

            if (formatter == null)
            {
                foreach (var formatterDef in _formatters)
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
            if (!string.IsNullOrWhiteSpace(_defaultType))
            {
                return Get(_defaultType, string.Empty, out formatterType);
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
                    if (_formatters.FirstOrDefault(f => f.Types.Contains(type.ToLower())) != null)
                    {
                        throw new Exception($"A formatter is already registered with type '{type}'");
                    }
                }

                _formatters.Add(new FormatterDefinition
                {
                    Formatter = formatter,
                    Types = types.Select(t => t.ToLower()),
                    Charsets = charsets.Select(c => c.ToLower())
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
                    _formatters.RemoveAll(f =>
                    {
                        foreach (var t in f.Types)
                        {
                            if (string.Compare(t, type, true) == 0)
                                return true;
                        }
                        return false;
                    });

                    if (!string.IsNullOrWhiteSpace(_defaultType) && string.Compare(_defaultType, type, true) == 0)
                    {
                        _defaultType = null;
                    }
                }
            }

            return this;
        }

        /// <summary>Gets the types.</summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetTypes()
        {
            foreach(var f in _formatters)
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
            if (!string.IsNullOrWhiteSpace(@default) && _formatters.FirstOrDefault(f => f.Types.Contains(@default)) == null)
            {
                throw new Exception($"Formater with registered type '{@default}' does not exist");
            }

            _defaultType = @default?.ToLower() ?? string.Empty;
            return this;
        }
    }
}
