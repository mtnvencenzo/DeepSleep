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
    public class AcceptValueWithQuality
    {
        private readonly string comparisonValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptValueWithQuality"/> class.
        /// </summary>
        public AcceptValueWithQuality(string value)
        {
            Value = value;
            MediaValue = value.GetMediaHeaderWithParameters();

            var rawquality = (this.MediaValue?.GetParameterValue("q") ?? string.Empty)
                .ToLowerInvariant()
                .Trim();

            if (string.IsNullOrWhiteSpace(rawquality))
            {
                this.Quality = 1f;
            }
            else
            {
                if (float.TryParse(rawquality, out var quality))
                {
                    if (quality <= 0)
                        this.Quality = 0f;
                    else if (quality >= 1)
                        this.Quality = 1f;
                    else
                        this.Quality = quality;
                }
                else
                {
                    this.Quality = 0f;
                }
            }

            this.comparisonValue = $"{MediaType}; q={QualityString()}{ParameterString()}";
        }

        /// <summary>Gets the type.</summary>
        /// <value>The type.</value>
        public string Type => this.MediaValue?.Type ?? string.Empty;

        /// <summary>Gets the type of the sub.</summary>
        /// <value>The type of the sub.</value>
        public string SubType => this.MediaValue?.SubType ?? string.Empty;

        /// <summary>Gets the type of the media.</summary>
        /// <value>The type of the media.</value>
        public string MediaType => this.MediaValue?.MediaType ?? string.Empty;

        /// <summary>Gets or sets the quality.</summary>
        /// <value>The quality.</value>
        public float Quality { get; private set; }

        /// <summary>Gets the parameter string.</summary>
        /// <value>The parameter string.</value>
        public virtual string ParameterString()
        {
            if (this.MediaValue?.Parameters == null)
                return string.Empty;

            string parameters = string.Empty;

            foreach (var p in this.MediaValue.Parameters)
            {
                if (p.Trim().ToLowerInvariant().StartsWith("q=") == false)
                {
                    parameters += $"; {p}";
                }
            }

            return parameters;
        }

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

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; private set; }

        /// <summary>Gets the values.</summary>
        /// <value>The values.</value>
        protected MediaValueWithParameters MediaValue { get; private set; }

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
    public static class AcceptValueWithQualityExtensionMethods
    {
        /// <summary>Gets the header with quality values.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IList<AcceptValueWithQuality> GetAcceptValueWithQualityValues(this string value)
        {
            var values = new List<AcceptValueWithQuality>();

            if (string.IsNullOrWhiteSpace(value))
            {
                return values;
            }

            foreach (var item in value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                values.Add(new AcceptValueWithQuality(item));
            }

            return values;
        }

        /// <summary>Sorts the quality precedence.</summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IList<AcceptValueWithQuality> SortAcceptValueQualityPrecedence(this IList<AcceptValueWithQuality> values)
        {
            if (values.Count() <= 1)
                return values;

            var retValues = new List<AcceptValueWithQuality>();

            float max;
            IEnumerable<AcceptValueWithQuality> qualities;
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
        private static IEnumerable<AcceptValueWithQuality> SortValues(this IEnumerable<AcceptValueWithQuality> values)
        {
            // */*, text/*;Level=1, text/html, application/json, text/html;Level=1
            // 

            if (values.Count() <= 1)
                return values;


            var processed = new List<Tuple<int, AcceptValueWithQuality>>();
            var retValues = new List<AcceptValueWithQuality>();
            var inValues = values.ToList();

            var item = inValues.FirstOrDefault(v => v.Type == "*" && v.SubType == "*");

            if (item != null)
            {
                processed.Add(new Tuple<int, AcceptValueWithQuality>(values.Count(), item));
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
                            processed.Add(new Tuple<int, AcceptValueWithQuality>(counter, like));
                            counter++;
                        }
                        processed.Add(new Tuple<int, AcceptValueWithQuality>(counter, v));
                        counter++;
                    }
                    else
                    {
                        processed.Add(new Tuple<int, AcceptValueWithQuality>(counter, v));
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
