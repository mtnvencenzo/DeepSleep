using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class MediaValueWithParameters
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaValueWithQuality"/> class.
        /// </summary>
        public MediaValueWithParameters()
        {
            Parameters = new List<string>();
        }

        #endregion

        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>Gets or sets the type of the sub.</summary>
        /// <value>The type of the sub.</value>
        public string SubType { get; set; }

        /// <summary>Gets or sets the charset.</summary>
        /// <value>The charset.</value>
        public string Charset { get; set; }

        /// <summary>Gets the type of the media.</summary>
        /// <value>The type of the media.</value>
        public string MediaType
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(Type))
                    return $"{Type}/{SubType}";

                return string.Empty;
            }
        }

        /// <summary>Gets or sets the parameters.</summary>
        /// <value>The parameters.</value>
        public List<string> Parameters { get; internal set; }

        /// <summary>Charsets the string.</summary>
        /// <returns></returns>
        internal string CharsetString()
        {
            if (string.IsNullOrWhiteSpace(Charset))
                return string.Empty;

            return $"; charset={Charset}";
        }

        /// <summary>Parameters the string.</summary>
        /// <returns></returns>
        internal string ParameterString()
        {
            if (Parameters.Count == 0)
                return string.Empty;

            string parameters = string.Empty;

            foreach (var p in Parameters)
            {
                parameters += $"; {p}";
            }

            return parameters;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="includeCharset">if set to <c>true</c> [include charset].</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public virtual string ToString(bool includeCharset)
        {
            string charsetString = (includeCharset)
                ? CharsetString()
                : string.Empty;

            return $"{Type}/{SubType}{charsetString}{ParameterString()}";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class MediaValueWithParametersExtensionMethods
    {
        /// <summary>Gets the header with quality values.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static MediaValueWithParameters GetMediaHeaderWithParameters(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }


            var parts = value.Trim().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var typeSubType = parts[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var type = typeSubType[0].Trim();
            var subType = typeSubType.Length == 2
                ? typeSubType[1].Trim()
                : string.Empty;

            if (parts.Length == 1)
            {
                return new MediaValueWithParameters
                {
                    Type = type,
                    SubType = subType,
                    Charset = string.Empty,
                    Parameters = new List<string>()
                };
            }
            else
            {
                return new MediaValueWithParameters
                {
                    Type = type,
                    SubType = subType,
                    Charset = parts.FirstOrDefault(p => p.Trim().ToLower().StartsWith("charset=", StringComparison.InvariantCultureIgnoreCase))?.ToLower()?.Replace("charset=",string.Empty) ?? string.Empty,
                    Parameters = parts.ToList().FindAll(p => !p.Trim().StartsWith("charset=", StringComparison.InvariantCultureIgnoreCase) && p != parts[0])
                };
            }
        }
    }
}
