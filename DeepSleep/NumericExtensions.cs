namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    internal static class NumericExtensions
    {
        /// <summary>Determines whether the specified command is negative.</summary>
        /// <param name="d">The command.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IsNegative(this decimal d)
        {
            return Math.Abs(d) > d;
        }

        /// <summary>
        /// Maximums the or zero.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        internal static int MaxOrZero<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            int max = 0;

            foreach (var item in source.Select(i => selector(i)))
            {
                if (item > max)
                    max = item;
            }

            return max;
        }

        /// <summary>Truncates the decimal to hundredths.</summary>
        /// <param name="d">The d.</param>
        /// <returns>The <see cref="decimal"/>.</returns>
        internal static decimal TruncateDecimalToHundreths(this decimal d)
        {
            return Math.Truncate(d * 100) / 100;
        }

        /// <summary>Determines whether the specified lower is between.</summary>
        /// <param name="inVal">The in value.</param>
        /// <param name="lower">The lower.</param>
        /// <param name="upper">The upper.</param>
        /// <returns>
        ///   <c>true</c> if the specified lower is between; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsBetween(this int? inVal, int lower, int upper)
        {
            return ((inVal ?? 0) >= lower && (inVal ?? 0) <= upper);
        }

        /// <summary>Determines whether the specified lower is between.</summary>
        /// <param name="inVal">The in value.</param>
        /// <param name="lower">The lower.</param>
        /// <param name="upper">The upper.</param>
        /// <returns>
        ///   <c>true</c> if the specified lower is between; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsBetween(this int inVal, int lower, int upper)
        {
            return (inVal >= lower && inVal <= upper);
        }
    }
}
