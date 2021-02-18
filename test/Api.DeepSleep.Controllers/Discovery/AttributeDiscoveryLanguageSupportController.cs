namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using System.Globalization;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryLanguageSupportController
    {
        /// <summary>Gets the language support default.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/languagesupport/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteLanguageSupport]
        public AttributeDiscoveryLanguageModel GetLanguageSupportDefault()
        {
            return new AttributeDiscoveryLanguageModel
            {
                CurrentCulture = CultureInfo.CurrentCulture.Name,
                CurrentUICulture = CultureInfo.CurrentUICulture.Name
            };
        }

        /// <summary>Gets the language support with fallback de de.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/languagesupport/fallaback/de-DE")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteLanguageSupport(fallbackLanguage: "de-DE")]
        public AttributeDiscoveryLanguageModel GetLanguageSupportWithFallbackDeDe()
        {
            return new AttributeDiscoveryLanguageModel
            {
                CurrentCulture = CultureInfo.CurrentCulture.Name,
                CurrentUICulture = CultureInfo.CurrentUICulture.Name
            };
        }

        /// <summary>Gets the language support with fallback en.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/languagesupport/fallaback/en")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteLanguageSupport(fallbackLanguage: "en")]
        public AttributeDiscoveryLanguageModel GetLanguageSupportWithFallbackEn()
        {
            return new AttributeDiscoveryLanguageModel
            {
                CurrentCulture = CultureInfo.CurrentCulture.Name,
                CurrentUICulture = CultureInfo.CurrentUICulture.Name
            };
        }

        /// <summary>Gets the language support with fallback en and supported.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/languagesupport/fallaback/en/with/supported")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteLanguageSupport(fallbackLanguage: "en", supportedLanguages: new[] { "es-ES", "en-GB" })]
        public AttributeDiscoveryLanguageModel GetLanguageSupportWithFallbackEnAndSupported()
        {
            return new AttributeDiscoveryLanguageModel
            {
                CurrentCulture = CultureInfo.CurrentCulture.Name,
                CurrentUICulture = CultureInfo.CurrentUICulture.Name
            };
        }

        /// <summary>Gets the language support with fallback en and supported sets thread cultures.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/languagesupport/fallaback/en/with/supported/thread/cultures")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteLanguageSupport(fallbackLanguage: "en", supportedLanguages: new[] { "es-ES", "en-GB" }, useAcceptedLanguageAsThreadCulture: true, useAcceptedLanguageAsThreadUICulture: true)]
        public AttributeDiscoveryLanguageModel GetLanguageSupportWithFallbackEnAndSupportedSetsThreadCultures()
        {
            return new AttributeDiscoveryLanguageModel
            {
                CurrentCulture = CultureInfo.CurrentCulture.Name,
                CurrentUICulture = CultureInfo.CurrentUICulture.Name
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryLanguageModel
    {
        /// <summary>Gets or sets the current UI culture.</summary>
        /// <value>The current UI culture.</value>
        public string CurrentUICulture { get; set; }

        /// <summary>Gets or sets the current culture.</summary>
        /// <value>The current culture.</value>
        public string CurrentCulture { get; set; }
    }
}
