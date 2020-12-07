﻿namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{ToString(true, true)}")]
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

        /// <summary>Gets or sets the boundary.</summary>
        /// <value>The boundary.</value>
        public string Boundary { get; set; }

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

        /// <summary>Boundaries the string.</summary>
        /// <returns></returns>
        internal string BoundaryString()
        {
            if (string.IsNullOrWhiteSpace(Boundary))
                return string.Empty;

            return $"; boundary={Boundary}";
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
            return ToString(true, true);
        }

        /// <summary>Converts to string.</summary>
        /// <param name="includeCharset">if set to <c>true</c> [include charset].</param>
        /// <param name="includeBoundary">if set to <c>true</c> [include boundary].</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public virtual string ToString(bool includeCharset, bool includeBoundary)
        {
            string charsetString = (includeCharset)
                ? CharsetString()
                : string.Empty;

            string boundaryString = (includeBoundary)
                ? BoundaryString()
                : string.Empty;

            return $"{Type}/{SubType}{charsetString}{boundaryString}{ParameterString()}";
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
                    Boundary = (parts.FirstOrDefault(p => p.Trim().ToLower().StartsWith("boundary=", StringComparison.InvariantCultureIgnoreCase))?.Replace("boundary=", string.Empty) ?? string.Empty).Trim(),
                    Parameters = parts
                        .ToList()
                        .FindAll(p => !p.Trim().StartsWith("charset=", StringComparison.InvariantCultureIgnoreCase) && !p.Trim().StartsWith("charset=", StringComparison.InvariantCultureIgnoreCase) && p != parts[0])
                };
            }
        }
    }
}
