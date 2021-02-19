namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class CharsetValueWithQuality
    {
        private readonly string comparisonValue;

        /// <summary>Initializes a new instance of the <see cref="CharsetValueWithQuality"/> class.</summary>
        /// <param name="charset">The charset.</param>
        /// <param name="quality">The quality.</param>
        /// <param name="parameters">The parameters.</param>
        public CharsetValueWithQuality(string charset, float quality, List<string> parameters = null)
        {
            this.Charset = charset;
            Parameters = parameters ?? new List<string>();

            if (quality <= 0)
                this.Quality = 0f;
            else if (quality >= 1)
                this.Quality = 1f;
            else
                this.Quality = quality;


            this.comparisonValue = $"{Charset}; q={QualityString()}{ParameterString()}";
        }

        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public string Charset { get; private set; }

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
    public static class CharsetValueWithQualityExtensionMethods
    {
        /// <summary>Gets the header with quality values.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IList<CharsetValueWithQuality> GetCharsetHeaderWithQualityValues(this string value)
        {
            var values = new List<CharsetValueWithQuality>();

            if (string.IsNullOrWhiteSpace(value))
            {
                return values;
            }

            string[] parts;
            foreach (var item in value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                parts = item.Trim().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                var charset = parts[0].Trim();

                if (parts.Length == 1)
                {
                    values.Add(new CharsetValueWithQuality(charset, 1.000f));
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

                    values.Add(new CharsetValueWithQuality(
                        charset: charset?.ToLowerInvariant()?.Trim(),
                        quality: quality,
                        parameters: parts.ToList().FindAll(p => !(p?.Trim().StartsWith("q=", StringComparison.InvariantCultureIgnoreCase) ?? false) && p != parts[0])));
                }
            }

            return values;
        }

        /// <summary>Sorts the quality precedence.</summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IList<CharsetValueWithQuality> SortCharsetQualityPrecedence(this IList<CharsetValueWithQuality> values)
        {
            if (values.Count() <= 1)
                return values;

            var retValues = new List<CharsetValueWithQuality>();

            float max;
            List<CharsetValueWithQuality> qualities;

            while (values.Count() > retValues.Count())
            {
                max = (from s in values
                       where !retValues.Exists(r => r.Quality == s.Quality)
                       orderby s.Quality descending
                       select s.Quality).FirstOrDefault();

                qualities = values
                    .Where(v => v.Quality == max)
                    .ToList()
                    .SortValues()
                    .ToList();

                retValues.AddRange(qualities);
            }

            return retValues;
        }

        /// <summary>Sorts the values.</summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        private static IEnumerable<CharsetValueWithQuality> SortValues(this IEnumerable<CharsetValueWithQuality> values)
        {
            if (values.Count() <= 1)
                return values;


            var processed = new List<Tuple<int, CharsetValueWithQuality>>();
            var retValues = new List<CharsetValueWithQuality>();
            var inValues = values.ToList();

            var item = inValues.FirstOrDefault(v => v.Charset == "*");

            if (item != null)
            {
                processed.Add(new Tuple<int, CharsetValueWithQuality>(values.Count(), item));
            }

            int counter = 1;
            foreach (var v in inValues)
            {
                if (!processed.Exists(t => t.Item2.ToString() == v.ToString()))
                {
                    processed.Add(new Tuple<int, CharsetValueWithQuality>(counter, v));
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
