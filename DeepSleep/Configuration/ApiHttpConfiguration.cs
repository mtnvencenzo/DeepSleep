namespace DeepSleep.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class ApiHttpConfiguration
    {
        /// <summary>Gets or sets the supported versions.</summary>
        /// <value>The supported versions.</value>
        public IList<string> SupportedVersions { get; set; }

        /// <summary>Gets or sets a value indicating whether [require SSL].</summary>
        /// <value><c>true</c> if [require SSL]; otherwise, <c>false</c>.</value>
        public bool? RequireSSL { get; set; }
    }
}
