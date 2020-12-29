namespace DeepSleep
{
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
    }
}
