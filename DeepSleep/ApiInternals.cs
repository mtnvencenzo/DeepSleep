using System.Globalization;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    internal class ApiInternals
    {
        /// <summary>Gets or sets a value indicating whether this instance is method not found.</summary>
        /// <value><c>true</c> if this instance is method not found; otherwise, <c>false</c>.</value>
        internal bool IsMethodNotFound { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is not found.</summary>
        /// <value><c>true</c> if this instance is not found; otherwise, <c>false</c>.</value>
        internal bool IsNotFound { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is overriding status code.</summary>
        /// <value><c>true</c> if this instance is overriding status code; otherwise, <c>false</c>.</value>
        internal bool IsOverridingStatusCode { get; set; }

        /// <summary>Gets or sets the current culture.</summary>
        /// <value>The current culture.</value>
        internal CultureInfo CurrentCulture { get; set; }

        /// <summary>Gets or sets the current UI culture.</summary>
        /// <value>The current UI culture.</value>
        internal CultureInfo CurrentUICulture { get; set; }
    }
}
