using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class LanguageValueWithQuality
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageValueWithQuality"/> class.
        /// </summary>
        public LanguageValueWithQuality()
        {
            Parameters = new List<string>();
            Quality = 1.000f;
        }

        private float _qual;

        #endregion

        /// <summary>Gets or sets the code.</summary>
        /// <value>The code.</value>
        public string Code { get; set; }

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
            return $"{Code}{ParameterString()}; q={QualityString()}";
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
        public static IEnumerable<LanguageValueWithQuality> GetLanguageHeaderWithQualityValues(this string value)
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
                    values.Add(new LanguageValueWithQuality
                    {
                        Code = parts[0].Trim(),
                        Quality = 1.000f
                    });
                }
                else
                {
                    float quality = 1.000f;
                    var qualityPart = parts.FirstOrDefault(p => p.Trim().ToLower().StartsWith("q=", StringComparison.InvariantCultureIgnoreCase));
                    if (qualityPart != null)
                    {
                        float.TryParse(
                            qualityPart.Trim().Substring(2).Trim(), 
                            NumberStyles.Any, 
                            CultureInfo.InvariantCulture, 
                            out quality);
                    }

                    values.Add(new LanguageValueWithQuality
                    {
                        Code = parts[0].Trim(),
                        Quality = quality,
                        Parameters = parts.ToList().FindAll(p => !p.Trim().StartsWith("q=", StringComparison.InvariantCultureIgnoreCase) && p != parts[0])
                    });
                }
            }

            return values;
        }

        /// <summary>Sorts the language quality precedence.</summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IEnumerable<LanguageValueWithQuality> SortLanguageQualityPrecedence(this IEnumerable<LanguageValueWithQuality> values)
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
