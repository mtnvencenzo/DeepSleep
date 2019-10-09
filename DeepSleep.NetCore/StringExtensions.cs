using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep.NetCore
{
    /// <summary>The string extensions.</summary>
    internal static class StringExtensions
    {
        /// <summary>This allows for a case insensitive search for a string in a string.</summary>
        /// <param name="inString">The source string.</param>
        /// <param name="value">The string to search for.</param>
        /// <param name="comparisonType">The comparison type.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool Contains(this string inString, string value, StringComparison comparisonType)
        {
            return inString.IndexOf(value, comparisonType) >= 0;
        }
    }
}
