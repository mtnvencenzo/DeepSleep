namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;

    public class AttributeDiscoveryLanguageSupportController
    {
        [ApiRoute("GET", "discovery/attribute/languagesupport/default")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteLanguageSupport]
        public AttributeDiscoveryModel GetLanguageSupportDefault()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/languagesupport/fallaback/de-DE")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteLanguageSupport(fallbackLanguage: "de-DE")]
        public AttributeDiscoveryModel GetLanguageSupportWithFallbackDeDe()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/languagesupport/fallaback/en")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteLanguageSupport(fallbackLanguage: "en")]
        public AttributeDiscoveryModel GetLanguageSupportWithFallbackEn()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/languagesupport/fallaback/en/with/supported")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteLanguageSupport(fallbackLanguage: "en", supportedLanguages: new[] { "es-ES", "en-GB" })]
        public AttributeDiscoveryModel GetLanguageSupportWithFallbackEnAndSupported()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
