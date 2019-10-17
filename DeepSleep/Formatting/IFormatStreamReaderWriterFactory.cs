namespace DeepSleep.Formatting
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public interface IFormatStreamReaderWriterFactory
    {
        /// <summary>Gets the formatter.</summary>
        /// <param name="type">The type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        IFormatStreamReaderWriter Get(string type, string parameters, out string formatterType);

        /// <summary>Defaults the specified formatter type.</summary>
        /// <param name="formatterType">Type of the formatter.</param>
        /// <returns></returns>
        IFormatStreamReaderWriter Default(out string formatterType);

        /// <summary>Adds the specified formatter.</summary>
        /// <param name="formatter">The formatter.</param>
        /// <param name="types">The types.</param>
        /// <param name="charsets">The charsets.</param>
        /// <returns></returns>
        IFormatStreamReaderWriterFactory Add(IFormatStreamReaderWriter formatter, string[] types, string[] charsets);

        /// <summary>Removes the specified types.</summary>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        IFormatStreamReaderWriterFactory Remove(params string[] types);

        /// <summary>Gets the types.</summary>
        /// <returns></returns>
        IEnumerable<string> GetTypes();

        /// <summary>Sets the default.</summary>
        /// <param name="default">The default.</param>
        /// <returns></returns>
        IFormatStreamReaderWriterFactory SetDefault(string @default);
    }
}
