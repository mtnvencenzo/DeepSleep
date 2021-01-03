using System.Collections.Generic;

namespace DeepSleep.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiLanguageSupportConfiguration
    {
        /// <summary>Gets or sets the fall back language.</summary>
        /// <value>The fall back language.</value>
        public string FallBackLanguage { get; set; }

        /// <summary>Gets or sets the supported languages.</summary>
        /// <value>The supported languages.</value>
        public IList<string> SupportedLanguages { get; set; }
    }
}
