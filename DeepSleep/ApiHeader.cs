namespace DeepSleep
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{Name}: {Value}")]
    public class ApiHeader
    {
        /// <summary>Initializes a new instance of the <see cref="ApiHeader"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public ApiHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiHeaderExtensionMethods
    {
        /// <summary>Determines whether the specified name has header.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified name has header; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasHeader(this IEnumerable<ApiHeader> headers, string name)
        {
            if (headers == null)
                return false;

            return headers.FirstOrDefault(h => string.Compare(h.Name, name, true) == 0) != null;
        }

        /// <summary>Gets the header.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static ApiHeader GetHeader(this IEnumerable<ApiHeader> headers, string name)
        {
            if (headers == null)
                return null;

            return headers.FirstOrDefault(h => string.Compare(h.Name, name, true) == 0);
        }

        /// <summary>Gets the value.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string GetValue(this IEnumerable<ApiHeader> headers, string name)
        {
            if (headers == null)
                return string.Empty;

            return headers.GetHeader(name)?.Value ?? string.Empty;
        }

        /// <summary>Sets the value.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IEnumerable<ApiHeader> SetValue(this IEnumerable<ApiHeader> headers, string name, string value)
        {
            if (headers != null)
            {
                foreach (var header in headers.Where(h => string.Compare(h.Name, name, true) == 0))
                {
                    header.Value = value;
                }
            }

            return headers;
        }
    }
}
