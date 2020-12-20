namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class EncodingValueWithQuality
    {
        private readonly string comparisonValue;

        /// <summary>Initializes a new instance of the <see cref="EncodingValueWithQuality"/> class.</summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="quality">The quality.</param>
        /// <param name="parameters">The parameters.</param>
        public EncodingValueWithQuality(string encoding, float quality, List<string> parameters = null)
        {
            this.Encoding = encoding;
            Parameters = parameters ?? new List<string>();

            if (quality <= 0)
                this.Quality = 0f;
            else if (quality >= 1)
                this.Quality = 1f;
            else
                this.Quality = quality;

            this.comparisonValue = $"{Encoding}; q={QualityString()}{ParameterString()}";
        }

        /// <summary>Gets the encoding.</summary>
        /// <value>The encoding.</value>
        public string Encoding { get; private set; }

        /// <summary>Gets the quality.</summary>
        /// <value>The quality.</value>
        public float Quality { get; private set; }

        /// <summary>Gets the parameters.</summary>
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

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.comparisonValue;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class EncodingValueWithQualityExtensionMethods
    {
        /// <summary>Gets the encoding header with quality values.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IList<EncodingValueWithQuality> GetEncodingHeaderWithQualityValues(this string value)
        {
            var values = new List<EncodingValueWithQuality>();

            if (string.IsNullOrWhiteSpace(value))
            {
                return values;
            }

            string[] parts;
            foreach (var item in value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                parts = item.Trim().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                var encoding = parts[0].Trim();

                if (parts.Length == 1)
                {
                    values.Add(new EncodingValueWithQuality(encoding, 1.000f));
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

                    values.Add(new EncodingValueWithQuality(
                        encoding: encoding?.ToLowerInvariant()?.Trim(),
                        quality: quality,
                        parameters: parts.ToList().FindAll(p => !(p?.Trim().StartsWith("q=", StringComparison.InvariantCultureIgnoreCase) ?? false) && p != parts[0])));
                }
            }

            return values;
        }

        /// <summary>Sorts the encoding quality precedence.</summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IList<EncodingValueWithQuality> SortEncodingQualityPrecedence(this IList<EncodingValueWithQuality> values)
        {
            if (values.Count() <= 1)
                return values;

            var retValues = new List<EncodingValueWithQuality>();

            float max;
            IEnumerable<EncodingValueWithQuality> qualities;
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
        private static IEnumerable<EncodingValueWithQuality> SortValues(this IEnumerable<EncodingValueWithQuality> values)
        {
            if (values.Count() <= 1)
                return values;


            var processed = new List<Tuple<int, EncodingValueWithQuality>>();
            var retValues = new List<EncodingValueWithQuality>();
            var inValues = values.ToList();

            var item = inValues.FirstOrDefault(v => v.Encoding == "*");

            if (item != null)
            {
                processed.Add(new Tuple<int, EncodingValueWithQuality>(values.Count(), item));
            }

            int counter = 1;
            foreach (var v in inValues)
            {
                if (!processed.Exists(t => t.Item2.ToString() == v.ToString()))
                {
                    processed.Add(new Tuple<int, EncodingValueWithQuality>(counter, v));
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
