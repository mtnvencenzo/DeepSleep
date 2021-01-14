namespace DeepSleep.Web
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    internal static class ListExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IList<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}
