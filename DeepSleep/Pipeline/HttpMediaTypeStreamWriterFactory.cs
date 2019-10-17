namespace DeepSleep.Pipeline
{
    using DeepSleep.Formatting;
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Formatting.DefaultFormatStreamReaderWriterFactory" />
    public class HttpMediaTypeStreamWriterFactory : DefaultFormatStreamReaderWriterFactory
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpMediaTypeStreamWriterFactory"/> class.
        /// </summary>
        public HttpMediaTypeStreamWriterFactory() : base()
        {
        }

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
        public override bool CanHandleType(string formatterType, string type, string parameters)
        {
            if (type == "*/*")
                return true;

            var formatterMediaType = string.Empty;
            var formatterMediaSubType = string.Empty;
            var typeMediaType = string.Empty;
            var typeMediaSubType = string.Empty;

            var formatterParts = formatterType.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var typeParts = type.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            formatterMediaType = formatterParts[0];
            formatterMediaSubType = formatterParts.Length == 2
                ? formatterParts[1]
                : "*";

            typeMediaType = typeParts[0];
            typeMediaSubType = typeParts.Length == 2
                ? typeParts[1]
                : "*";

            if (string.Compare(formatterMediaType, typeMediaType, true) != 0)
                return false;

            if (typeMediaSubType == "*")
                return true;

            var formatterMediaParameters = string.Empty;
            if (formatterMediaSubType.Contains(";") && formatterMediaSubType.Length > formatterMediaSubType.IndexOf(";") + 1)
            {
                formatterMediaParameters = formatterMediaSubType.Substring(formatterMediaSubType.IndexOf(";") + 1);
            }

            var typeMediaParameters = string.Empty;
            if (typeMediaSubType.Contains(";") && typeMediaSubType.Length > typeMediaSubType.IndexOf(";") + 1)
            {
                typeMediaParameters = typeMediaSubType.Substring(typeMediaSubType.IndexOf(";") + 1);
            }

            if (string.Compare(formatterMediaSubType, typeMediaSubType, true) == 0 && string.Compare(formatterMediaParameters, typeMediaParameters, true) == 0)
                return true;

            return false;

        }
    }
}
