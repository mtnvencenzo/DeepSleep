using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DeepSleep
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

        /// <summary>Ins the length.</summary>
        /// <param name="inString">The in string.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool InLength(this string inString, int minLength, int maxLength)
        {
            return InLength(inString, minLength, maxLength, false);
        }

        /// <summary>Ins the length.</summary>
        /// <param name="inString">The in string.</param>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="allowNull">If set to <c>true</c> [allow null].</param>
        /// <returns>The <see cref="bool"/>.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">MinLength;Cannot be less than zero(0)
        ///     or
        ///     maxLength;Cannot be less than minLength.</exception>
        internal static bool InLength(this string inString, int minLength, int maxLength, bool allowNull)
        {
            if (minLength < 0)
            {
                throw new ArgumentOutOfRangeException("minLength", minLength, "Cannot be less than zero(0)");
            }

            if (maxLength < minLength)
            {
                throw new ArgumentOutOfRangeException("maxLength", maxLength, "Cannot be less than minLength");
            }

            if (inString == null && allowNull && minLength == 0)
            {
                return true;
            }

            if (inString == null)
            {
                return false;
            }

            if (inString.Length >= minLength && inString.Length <= maxLength)
            {
                return true;
            }

            return false;
        }

        /// <summary>The truncate.</summary>
        /// <param name="inString">The in string.</param>
        /// <param name="length">The length.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string Truncate(this string inString, int length)
        {
            if (string.IsNullOrEmpty(inString))
            {
                return inString;
            }

            if (inString.Length > length)
            {
                return inString.Substring(0, length);
            }

            return inString;
        }

        /// <summary>To the int enumerable.</summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        internal static IEnumerable<int> ToIntEnumerable(this string input)
        {
            if( string.IsNullOrWhiteSpace(input) )
                yield break;

            var items = input.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            if (items != null)
            {
                int iItem;
                foreach (var item in items)
                {
                    if( int.TryParse(
                        item,
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture, 
                        out iItem))
                    {
                        yield return iItem;
                    }
                }
            }

            yield break;
        }

        /// <summary>Ignores the case contains.</summary>
        /// <param name="compare">The compare.</param>
        /// <param name="compareTo">The compare automatic.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        internal static bool IgnoreCaseContains(this string compare, string compareTo)
        {
            if (string.IsNullOrWhiteSpace(compareTo))
            {
                return true;
            }

            if (compare == null)
            {
                return false;
            }

            return compare.ToLower().Contains(compareTo.ToLower());
        }

        /// <summary>Determines whether the specified pattern is match.</summary>
        /// <param name="inString">The in string.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns></returns>
        internal static bool IsMatch(this string inString, string pattern)
        {
            Regex reg = new Regex(pattern);
            return reg.IsMatch(inString);
        }

        /// <summary>Prefixes the specified prefix.</summary>
        /// <param name="instring">The instring.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns></returns>
        internal static string Prefix(this string instring, string prefix)
        {
            return prefix + instring;
        }

        /// <summary>Subs the string safe.</summary>
        /// <param name="instring">The instring.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        internal static string SubStringSafe(this string instring, int startIndex, int length)
        {
            return instring.SubStringSafe(startIndex, length, string.Empty);
        }

        /// <summary>Subs the string safe.</summary>
        /// <param name="instring">The instring.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="length">The length.</param>
        /// <param name="truncationIndicator">The truncation indicator.</param>
        /// <returns></returns>
        internal static string SubStringSafe(this string instring, int startIndex, int length, string truncationIndicator)
        {
            if (string.IsNullOrWhiteSpace(instring))
                return string.Empty;

            int lengthToEnd = instring.Length - startIndex;
            int safeLength = (length > lengthToEnd)
                ? lengthToEnd
                : length;

            return (!string.IsNullOrWhiteSpace(truncationIndicator) && safeLength != length)
                ? instring.Substring(startIndex, safeLength) + truncationIndicator
                : instring.Substring(startIndex, safeLength);
        }
    }
}