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
    public class MediaValueWithQuality
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaValueWithQuality"/> class.
        /// </summary>
        public MediaValueWithQuality()
        {
            Parameters = new List<string>();
            Quality = 1.000f;
        }

        private float _qual;

        #endregion

        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>Gets or sets the type of the sub.</summary>
        /// <value>The type of the sub.</value>
        public string SubType { get; set; }

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
            return $"{Type}/{SubType}{ParameterString()}; q={QualityString()}";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class MediaValueWithQualityExtensionMethods
    {
        /// <summary>Gets the header with quality values.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IEnumerable<MediaValueWithQuality> GetMediaHeaderWithQualityValues(this string value)
        {
            var values = new List<MediaValueWithQuality>();

            if (string.IsNullOrWhiteSpace(value))
            {
                return values;
            }

            string[] parts;
            foreach (var item in value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                parts = item.Trim().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                var typeSubType = parts[0].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                var type = typeSubType[0].Trim();
                var subType = typeSubType.Length == 2
                    ? typeSubType[1].Trim()
                    : "*";

                if (parts.Length == 1)
                {
                    values.Add(new MediaValueWithQuality
                    {
                        Type = type,
                        SubType = subType,
                        Quality = 1.000f
                    });
                }
                else
                {
                    float quality = 1.000f;
                    var qualityPart = parts.FirstOrDefault(p => p.Trim().StartsWith("q=", StringComparison.InvariantCultureIgnoreCase));
                    if (qualityPart != null)
                    {
                        float.TryParse(
                            qualityPart.Substring(2).Trim(),
                            NumberStyles.Any,
                            CultureInfo.InvariantCulture, 
                            out quality);
                    }

                    values.Add(new MediaValueWithQuality
                    {
                        Type = type,
                        SubType = subType,
                        Quality = quality,
                        Parameters = parts.ToList().FindAll(p => !p.Trim().StartsWith("q=", StringComparison.InvariantCultureIgnoreCase) && p != parts[0])
                    });
                }
            }

            return values;
        }

        /// <summary>Sorts the quality precedence.</summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IEnumerable<MediaValueWithQuality> SortMediaQualityPrecedence(this IEnumerable<MediaValueWithQuality> values)
        {
            if (values.Count() <= 1)
                return values;

            var retValues = new List<MediaValueWithQuality>();

            float max;
            IEnumerable<MediaValueWithQuality> qualities;
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
        private static IEnumerable<MediaValueWithQuality> SortValues(this IEnumerable<MediaValueWithQuality> values)
        {
            // */*, text/*;Level=1, text/html, application/json, text/html;Level=1
            // 

            if (values.Count() <= 1)
                return values;


            var processed = new List<Tuple<int, MediaValueWithQuality>>();
            var retValues = new List<MediaValueWithQuality>();
            var inValues = values.ToList();

            var item = inValues.FirstOrDefault(v => v.Type == "*" && v.SubType == "*");

            if (item != null)
            {
                processed.Add(new Tuple<int, MediaValueWithQuality>(values.Count(), item));
            }

            int counter = 1;
            foreach (var v in inValues)
            {
                if (!processed.Exists(t => t.Item2.ToString() == v.ToString()))
                {
                    if (v.SubType == "*")
                    {
                        foreach (var like in inValues.Where(a => string.Compare(a.Type, v.Type, true) == 0 && a.SubType != "*"))
                        {
                            processed.Add(new Tuple<int, MediaValueWithQuality>(counter, like));
                            counter++;
                        }
                        processed.Add(new Tuple<int, MediaValueWithQuality>(counter, v));
                        counter++;
                    }
                    else
                    {
                        processed.Add(new Tuple<int, MediaValueWithQuality>(counter, v));
                        counter++;
                    }
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
