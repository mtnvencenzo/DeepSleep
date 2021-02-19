namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteLanguageSupportAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteLanguageSupportAttribute"/> class.</summary>
        /// <param name="fallbackLanguage">The fallback language.</param>
        /// <param name="supportedLanguages">The supported languages.</param>
        public ApiRouteLanguageSupportAttribute(string fallbackLanguage = null, string[] supportedLanguages = null)
        {
            this.FallbackLanguage = fallbackLanguage;
            this.SupportedLanguages = supportedLanguages;
        }

        /// <summary>Initializes a new instance of the <see cref="ApiRouteLanguageSupportAttribute"/> class.</summary>
        /// <param name="fallbackLanguage">The fallback language.</param>
        /// <param name="useAcceptedLanguageAsThreadCulture">if set to <c>true</c> [use accepted language as thread culture].</param>
        /// <param name="useAcceptedLanguageAsThreadUICulture">if set to <c>true</c> [use accepted language as thread UI culture].</param>
        /// <param name="supportedLanguages">The supported languages.</param>
        public ApiRouteLanguageSupportAttribute(string fallbackLanguage, bool useAcceptedLanguageAsThreadCulture, bool useAcceptedLanguageAsThreadUICulture, string[] supportedLanguages = null)
        {
            this.FallbackLanguage = fallbackLanguage;
            this.SupportedLanguages = supportedLanguages;
            this.UseAcceptedLanguageAsThreadCulture = useAcceptedLanguageAsThreadCulture;
            this.UseAcceptedLanguageAsThreadUICulture = useAcceptedLanguageAsThreadUICulture;
        }

        /// <summary>Gets the fallback language.</summary>
        /// <value>The fallback language.</value>
        public string FallbackLanguage { get; private set; }

        /// <summary>Gets the supported languages.</summary>
        /// <value>The supported languages.</value>
        public string[] SupportedLanguages { get; private set; }

        /// <summary>Gets the use accept language as thread culture.</summary>
        /// <value>The use accept language as thread culture.</value>
        public bool? UseAcceptedLanguageAsThreadCulture { get; private set; }

        /// <summary>Gets the use accept language as thread UI culture.</summary>
        /// <value>The use accept language as thread UI culture.</value>
        public bool? UseAcceptedLanguageAsThreadUICulture { get; private set; }
    }
}
