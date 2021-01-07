namespace DeepSleep.NetCore
{
    using System;

    /// <summary>
    /// </summary>
    internal static class DateTimeExtensions
    {
        internal static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>Toes the UTC date.</summary>
        /// <param name="unixTime">The UNIX time.</param>
        /// <returns>The <see cref="DateTime"/>.</returns>
        internal static DateTime ToUtcDate(this long unixTime)
        {
            try
            {
                return _epoch.AddSeconds(unixTime);
            }
            catch
            {
                return _epoch;
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