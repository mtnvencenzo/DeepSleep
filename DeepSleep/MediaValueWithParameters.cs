namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public class MediaValueWithParameters
    {
        private readonly string comparisonValue;

        /// <summary>Initializes a new instance of the <see cref="MediaValueWithParameters"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="subtype">The subtype.</param>
        /// <param name="parameters">The parameters.</param>
        public MediaValueWithParameters(string type, string subtype, List<string> parameters = null)
        {
            this.Type = type?.Trim()?.ToLowerInvariant() ?? string.Empty;

            this.SubType = subtype?.Trim()?.ToLowerInvariant() ?? string.Empty;

            this.MediaType = $"{this.Type}/{this.SubType}";

            this.Parameters = parameters ?? new List<string>();

            this.comparisonValue = $"{Type}/{SubType}{ParameterString()}";
        }

        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public string Type { get; private set; }

        /// <summary>Gets or sets the type of the sub.</summary>
        /// <value>The type of the sub.</value>
        public string SubType { get; private set; }

        /// <summary>Gets the type of the media.</summary>
        /// <value>The type of the media.</value>
        public string MediaType { get; private set; }

        /// <summary>Gets or sets the parameters.</summary>
        /// <value>The parameters.</value>
        public List<string> Parameters { get; private set; }

        /// <summary>Parameters the string.</summary>
        /// <returns></returns>
        internal virtual string ParameterString()
        {
            if (!Parameters.Any())
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
            return this.comparisonValue;
        }

        /// <summary>Gets the parameter value.</summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="trimValues">if set to <c>true</c> [trim values].</param>
        /// <returns></returns>
        public virtual string GetParameterValue(string parameterName, bool trimValues = true)
        {
            var param = this.Parameters?.FirstOrDefault(p => p.Replace(" ", string.Empty).StartsWith($"{parameterName}=", StringComparison.InvariantCultureIgnoreCase));

            if (param != null && param.Contains("="))
            {
                return trimValues
                    ? param.Substring(param.IndexOf("=") + 1).Trim()
                    : param.Substring(param.IndexOf("=") + 1);
            }

            return string.Empty;
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

            var parts = value
                .Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(p => p != null)
                .Select(p => p)
                .ToArray();

            var typeSubType = parts[0]
                .Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            var type = typeSubType[0]
                .Trim()
                .ToLowerInvariant();

            var subType = typeSubType.Length == 2
                ? typeSubType[1].Trim().ToLowerInvariant()
                : string.Empty;

            if (parts.Length == 1)
            {
                return new MediaValueWithParameters(type, subType);
            }
            else
            {
                var parameters = parts
                    .Where(p => p != null && p != parts[0])
                    .ToList();

                return new MediaValueWithParameters(type, subType, parameters);
            }
        }
    }
}
