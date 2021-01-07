namespace DeepSleep.Configuration
{
    using System.Collections.Generic;

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

        /// <summary>Gets or sets the use accepted language as thread UI culture.</summary>
        /// <value>The use accepted language as thread UI culture.</value>
        public bool? UseAcceptedLanguageAsThreadUICulture { get; set; }

        /// <summary>Gets or sets the use accepted language as thread culture.</summary>
        /// <value>The use accepted language as thread culture.</value>
        public bool? UseAcceptedLanguageAsThreadCulture { get; set; }
    }
}
