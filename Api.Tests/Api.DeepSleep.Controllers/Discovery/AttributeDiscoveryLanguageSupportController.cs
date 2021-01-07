namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using System.Globalization;

    public class AttributeDiscoveryLanguageSupportController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/languagesupport/default")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
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
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
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
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
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
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
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
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
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
