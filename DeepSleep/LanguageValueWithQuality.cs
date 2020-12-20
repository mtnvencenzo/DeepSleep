namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public class LanguageValueWithQuality
    {
        private readonly string comparisonValue;

        /// <summary>Initializes a new instance of the <see cref="LanguageValueWithQuality"/> class.</summary>
        /// <param name="code">The code.</param>
        /// <param name="quality">The quality.</param>
        /// <param name="parameters">The parameters.</param>
        public LanguageValueWithQuality(string code, float quality, List<string> parameters = null)
        {
            this.Code = code;
            Parameters = parameters ?? new List<string>();

            if (quality <= 0)
                this.Quality = 0f;
            else if (quality >= 1)
                this.Quality = 1f;
            else
                this.Quality = quality;

            this.comparisonValue = $"{Code}; q={QualityString()}{ParameterString()}";
        }

        /// <summary>Gets or sets the code.</summary>
        /// <value>The code.</value>
        public string Code { get; private set; }

        /// <summary>Gets or sets the quality.</summary>
        /// <value>The quality.</value>
        public float Quality { get; private set; }

        /// <summary>Gets or sets the parameters.</summary>
        /// <value>The parameters.</value>
        public List<string> Parameters { get; private set; }

        /// <summary>Qualities the string.</summary>
        /// <returns></returns>
        internal string QualityString()
        {
            if (Quality == 0)
                return "0";

            if (Quality == 1)
                return "1";

            return Quality.ToString(".###", CultureInfo.InvariantCulture).TrimEnd(new char[] { '0' });
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
                if (p.Trim().ToLowerInvariant().StartsWith("q=") == false)
                {
                    parameters += $"; {p}";
                }
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
    }

    /// <summary>
    /// 
    /// </summary>
    public static class LanguageValueWithQualityExtensionMethods
    {
        /// <summary>Gets the header with quality values.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IList<LanguageValueWithQuality> GetLanguageHeaderWithQualityValues(this string value)
        {
            var values = new List<LanguageValueWithQuality>();

            if (string.IsNullOrWhiteSpace(value))
            {
                return values;
            }

            string[] parts;
            foreach (var item in value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                parts = item.Trim().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 1)
                {
                    values.Add(new LanguageValueWithQuality(parts[0].Trim(), 1.000f));
                }
                else
                {
                    float quality = 1.000f;
                    var qualityPart = parts.FirstOrDefault(p => p?.Trim().StartsWith("q=", StringComparison.InvariantCultureIgnoreCase) ?? false);
                    if (qualityPart != null)
                    {
                        float.TryParse(
                            qualityPart.Trim().Substring(2).Trim(),
                            NumberStyles.Any,
                            CultureInfo.InvariantCulture,
                            out quality);
                    }

                    if (quality < 0)
                        quality = 0;
                    if (quality > 1)
                        quality = 1;

                    var codeParts = parts[0]
                        .Trim()
                        .Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

                    var code = codeParts.Length == 1
                        ? codeParts[0].ToLowerInvariant()
                        : $"{codeParts[0].Trim().ToLowerInvariant()}-{codeParts[1].Trim().ToUpperInvariant()}";

                    values.Add(new LanguageValueWithQuality(
                        code: code,
                        quality: quality,
                        parameters: parts.ToList().FindAll(p => !(p?.Trim().StartsWith("q=", StringComparison.InvariantCultureIgnoreCase) ?? false) && p != parts[0])));
                }
            }

            return values;
        }

        /// <summary>Sorts the language quality precedence.</summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IList<LanguageValueWithQuality> SortLanguageQualityPrecedence(this IList<LanguageValueWithQuality> values)
        {
            if (values.Count() <= 1)
                return values;

            var retValues = new List<LanguageValueWithQuality>();

            float max;
            IEnumerable<LanguageValueWithQuality> qualities;
            while (values.Count() > retValues.Count())
            {
                max = (from s in values
                       where !retValues.Exists(r => r.Quality == s.Quality)
                       orderby s.Quality descending
                       select s.Quality).FirstOrDefault();

                qualities = values
                    .Where(v => v.Quality == max)
                    .SortValues();

                retValues.AddRange(qualities);
            }

            return retValues;
        }

        /// <summary>Sorts the values.</summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        private static IEnumerable<LanguageValueWithQuality> SortValues(this IEnumerable<LanguageValueWithQuality> values)
        {
            if (values.Count() <= 1)
                return values;


            var processed = new List<Tuple<int, LanguageValueWithQuality>>();
            var retValues = new List<LanguageValueWithQuality>();
            var inValues = values.ToList();

            var item = inValues.FirstOrDefault(v => v.Code == "*");

            if (item != null)
            {
                processed.Add(new Tuple<int, LanguageValueWithQuality>(values.Count(), item));
            }

            int counter = 1;
            foreach (var v in inValues)
            {
                if (!processed.Exists(t => t.Item2.ToString() == v.ToString()))
                {
                    processed.Add(new Tuple<int, LanguageValueWithQuality>(counter, v));
                    counter++;
                }
            }

            foreach (var v in processed.OrderBy(t => t.Item1))
            {
                retValues.Add(v.Item2);
            }

            return retValues;
        }
    }
}
