namespace DeepSleep
{
    using System;

    /// <summary>The string extensions.</summary>
    internal static class StringExtensions
    {
        /// <summary>Ins the specified comparison type.</summary>
        /// <param name="inString">The in string.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        internal static bool In(this string inString, StringComparison comparisonType, params string[] values)
        {
            if (string.IsNullOrWhiteSpace(inString))
                return false;

            if (values == null)
                return false;

            foreach (var val in values)
            {
                if (inString.Equals(val, comparisonType))
                    return true;
            }

            return false;
        }
    }
}