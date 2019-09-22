using System;
using System.Globalization;

namespace DeepSleep
{
    /// <summary>
    /// </summary>
    internal static class DateTimeExtensions
    {
        #region Static Fields

        /// <summary></summary>
        internal static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        /// <summary>Toes the UNIX time.</summary>
        /// <param name="date">The date.</param>
        /// <returns>The <see cref="long"/>.</returns>
        internal static long ToUnixTime(this DateTime date)
        {
            DateTime utc = (date.Kind != DateTimeKind.Utc)
                ? date.ChangeKind(DateTimeKind.Utc)
                : date;

            TimeSpan ts = utc.Subtract(_epoch);
            double epochtime = (((((ts.Days * 24) + ts.Hours) * 60) + ts.Minutes) * 60) + ts.Seconds;
            return Convert.ToInt64(epochtime);
        }

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

        /// <summary>This will change a one or two digit year into a four digit year.</summary>
        /// <param name="year">The one or two digit year to convert.</param>
        /// <returns>The four digit year.</returns>
        internal static int GetFourDigitYear(int year)
        {
            return GetFourDigitYear(year, 1);
        }

        /// <summary>This will change a one or two digit year into a four digit year.</summary>
        /// <param name="year">The one or two digit year to convert.</param>
        /// <param name="period">How many years to add to the current year to set the pivot point of the previous century.</param>
        /// <returns>The four digit year.</returns>
        internal static int GetFourDigitYear(int year, int period)
        {
            var cal = new GregorianCalendar
            {
                TwoDigitYearMax = DateTime.Now.Year + period
            };
            return cal.ToFourDigitYear(year);
        }
    }
}