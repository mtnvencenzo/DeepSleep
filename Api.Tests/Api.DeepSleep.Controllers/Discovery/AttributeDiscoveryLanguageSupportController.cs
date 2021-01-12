namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using System.Globalization;

    public class AttributeDiscoveryLanguageSupportController
    {
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

    public class AttributeDiscoveryLanguageModel
    {
        public string CurrentUICulture { get; set; }

        public string CurrentCulture { get; set; }
    }
}
