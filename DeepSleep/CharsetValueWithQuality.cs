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
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="CharsetValueWithQuality"/> class.
        /// </summary>
        public CharsetValueWithQuality()
        {
            Parameters = new List<string>();
            Quality = 1.000f;
        }

        private float _qual;

        #endregion

        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>Gets or sets the quality.</summary>
        /// <value>The quality.</value>
        public float Quality
        {
            get => _qual;
            set
            {
                if (value <= 0)
                    _qual = 0f;
                if (value >= 1)
                    _qual = 1f;

                _qual = value;
            }
        }

        /// <summary>Gets or sets the parameters.</summary>
        /// <value>The parameters.</value>
        public List<string> Parameters { get; internal set; }

        /// <summary>Qualities the string.</summary>
        /// <returns></returns>
        internal string QualityString()
        {
            if (Quality == 0)
                return "0";

            if (Quality == 1)
                return "1";

            return _qual.ToString(".###").TrimEnd(new char[] { '0' });
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
            return $"{Type}{ParameterString()}; q={QualityString()}";
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
        public static IEnumerable<CharsetValueWithQuality> GetCharsetHeaderWithQualityValues(this string value)
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

                var type = parts[0].Trim();

                if (parts.Length == 1)
                {
                    values.Add(new CharsetValueWithQuality
                    {
                        Type = type,
                        Quality = 1.000f
                    });
                }
                else
                {
                    float quality = 1.000f;
                    var qualityPart = parts.FirstOrDefault(p => p.StartsWith("q=", StringComparison.InvariantCultureIgnoreCase));
                    if (qualityPart != null)
                    {
                        float.TryParse(
                            qualityPart.Substring(2).Trim(), 
                            NumberStyles.Any, 
                            CultureInfo.InvariantCulture, 
                            out quality);
                    }

                    values.Add(new CharsetValueWithQuality
                    {
                        Type = type,
                        Quality = quality,
                        Parameters = parts.ToList().FindAll(p => !p.StartsWith("q=", StringComparison.InvariantCultureIgnoreCase) && p != parts[0])
                    });
                }
            }

            return values;
        }

        /// <summary>Sorts the quality precedence.</summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IEnumerable<CharsetValueWithQuality> SortCharsetQualityPrecedence(this IEnumerable<CharsetValueWithQuality> values)
        {
            if (values.Count() <= 1)
                return values;

            var retValues = new List<CharsetValueWithQuality>();

            float max;
            IEnumerable<CharsetValueWithQuality> qualities;
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
        private static IEnumerable<CharsetValueWithQuality> SortValues(this IEnumerable<CharsetValueWithQuality> values)
        {
            if (values.Count() <= 1)
                return values;


            var processed = new List<Tuple<int, CharsetValueWithQuality>>();
            var retValues = new List<CharsetValueWithQuality>();
            var inValues = values.ToList();

            var item = inValues.FirstOrDefault(v => v.Type == "*");

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
