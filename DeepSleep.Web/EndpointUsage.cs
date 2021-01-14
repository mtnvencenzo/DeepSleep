namespace DeepSleep.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class EndpointUsage
    {
        /// <summary>Gets or sets a value indicating whether this <see cref="EndpointUsage"/> is enabled.</summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>Gets or sets the relative path.</summary>
        /// <value>The relative path.</value>
        public string RelativePath { get; set; }
    }
}
