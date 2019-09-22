using System;
using System.Globalization;

namespace DeepSleep.NetCore
{
    /// <summary>
    /// </summary>
    internal static class DateTimeExtensions
    {
        #region Static Fields

        /// <summary></summary>
        internal static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        /// <summary>Toes the UTC date.</summary>
        /// <param name="unixTime">The UNIX time.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        internal static DateTime ToUtcDate(this long unixTime)
        {
            try
            {
                return _epoch.AddSeconds(unixTime);
            }
            catch (System.Exception)
            {
                return new DateTime(1, 1, 1, 1, 0, 1, DateTimeKind.Utc);
            }
        }

        /// <summary>Changes the kind.</summary>
        /// <param name="date">The date.</param>
        /// <param name="kind">The kind.</param>
        /// <returns></returns>
        internal static DateTime ChangeKind(this DateTime date, DateTimeKind kind)
        {
            return new DateTime(date.Ticks, kind);
        }
    }
}