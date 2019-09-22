using System;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>Concatenates the specified separator.</summary>
        /// <param name="list">The list.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string Concatenate(this System.Collections.IList list, string separator)
        {
            return list.Concatenate(separator, o => o.ToString());
        }

        /// <summary>Concatenates the specified separator.</summary>
        /// <param name="list">The list.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="projection">The projection.</param>
        /// <returns></returns>
        public static string Concatenate(this System.Collections.IList list, string separator, Func<object, string> projection)
        {
            if (list == null || list.Count == 0)
                return string.Empty;

            string retval = string.Empty;

            foreach (object o in list)
            {
                retval += projection(o);

                if (list.IndexOf(o) != list.Count - 1)
                    retval += separator;
            }

            return retval;
        }
    }
}
