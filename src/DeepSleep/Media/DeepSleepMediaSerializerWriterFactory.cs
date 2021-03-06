﻿namespace DeepSleep.Media
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Media.DeepSleepMediaSerializerFactoryBase" />
    public class DeepSleepMediaSerializerWriterFactory : DeepSleepMediaSerializerFactoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeepSleepMediaSerializerWriterFactory"/> class.
        /// </summary>
        public DeepSleepMediaSerializerWriterFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
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
        public override bool CanHandleType(string formatterType, string type, string parameters)
        {
            if (type == "*/*")
                return true;

            var formatterParts = formatterType.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var typeParts = type.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            string formatterMediaType = formatterParts[0];
            string formatterMediaSubType = formatterParts.Length == 2
                ? formatterParts[1]
                : "*";

            string typeMediaType = typeParts[0];
            string typeMediaSubType = typeParts.Length == 2
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
