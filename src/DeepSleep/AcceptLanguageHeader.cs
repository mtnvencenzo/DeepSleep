﻿namespace DeepSleep
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class AcceptLanguageHeader
    {
        private readonly string comparisonValue;

        /// <summary>Initializes a new instance of the <see cref="AcceptCharsetHeader"/> class.</summary>
        /// <param name="value">The value.</param>
        public AcceptLanguageHeader(string value)
        {
            Value = value;
            Values = value
                .GetLanguageHeaderWithQualityValues()
                .SortLanguageQualityPrecedence();

            if (Values.Count == 0)
            {
                this.comparisonValue = null;
            }
            else
            {
                var values = Values
                    .Select(s => s.ToString())
                    .ToArray();

                this.comparisonValue = string.Join(", ", values);
            }
        }

        /// <summary>Performs an implicit conversion from <see cref="AcceptCharsetHeader"/> to <see cref="System.String"/>.</summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(AcceptLanguageHeader v)
        {
            return v?.ToString();
        }

        /// <summary>Performs an implicit conversion from <see cref="System.String"/> to <see cref="AcceptCharsetHeader"/>.</summary>
        /// <param name="s">The s.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator AcceptLanguageHeader(string s)
        {
            return new AcceptLanguageHeader(s);
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="header">The header.</param>
        /// <param name="str">The string.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(AcceptLanguageHeader header, string str)
        {
            return header == new AcceptLanguageHeader(str);
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="header">The header.</param>
        /// <param name="compareTo">The compare to.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(AcceptLanguageHeader header, AcceptLanguageHeader compareTo)
        {
            return header?.ToString() == compareTo?.ToString();
        }

        /// <summary>Implements the operator ==.</summary>
        /// <param name="str">The string.</param>
        /// <param name="header">The header.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(string str, AcceptLanguageHeader header)
        {
            return header == new AcceptLanguageHeader(str);
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="header">The header.</param>
        /// <param name="str">The string.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(AcceptLanguageHeader header, string str)
        {
            return header != new AcceptLanguageHeader(str);
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="header">The header.</param>
        /// <param name="compareTo">The compare to.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(AcceptLanguageHeader header, AcceptLanguageHeader compareTo)
        {
            return header?.ToString() != compareTo?.ToString();
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="str">The string.</param>
        /// <param name="header">The header.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(string str, AcceptLanguageHeader header)
        {
            return header != new AcceptLanguageHeader(str);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is AcceptLanguageHeader header)
            {
                return ToString().Equals(header?.ToString());
            }
            else if (obj is string str)
            {
                return ToString().Equals(new AcceptLanguageHeader(str).ToString());
            }

            return ToString().Equals(obj);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.comparisonValue;
        }

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; private set; }

        /// <summary>Gets the values.</summary>
        /// <value>The values.</value>
        public IList<LanguageValueWithQuality> Values { get; private set; }
    }
}
