namespace DeepSleep.Configuration
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Defines how to handle requests issued aith a Accept-Language header.  The deepsleep framework
    /// does not use this value to reject requests but rather to optionally set the <see cref="CultureInfo.CurrentCulture"/>
    /// and <see cref="CultureInfo.CurrentUICulture"/> values for the request thread.
    /// <para>
    /// When determining the culture to use and apply to the current <see cref="ApiRequestInfo.AcceptLanguage"/>, first the value(s) in the request's Accept-Langauge 
    /// header with precedence based on each values quality will be used.
    /// If any of the supplied values match a value defined in the configured <see cref="SupportedLanguages"/> then it will be used. If not matching any value then 
    /// the optionally configured <see cref="FallBackLanguage"/> will be used. If not matched and no <see cref="FallBackLanguage"/> is configured, the resulting
    /// <see cref="ApiRequestInfo.AcceptLanguage"/> will be set to the current <see cref="CultureInfo.CurrentUICulture"/>.
    /// </para>
    /// <para>
    /// When matching accept language values, first an exact case-insensitive match lookup is made against the configured <see cref="SupportedLanguages"/>. If no matches are found then a match is made using culture neutral
    /// versions of the language value.  I.e. if a value of 'en-US' is not matched, and no other values match then an attempt to match 'en' will be made.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// [see-also] Hypertext Transfer Protocol (14.4 Accept-Language: <see href="https://tools.ietf.org/html/rfc2616#section-14.4"/>)<br/>
    /// [tbd] Future consideration is being given to whether or not to optionally reqect requests not matching any <see cref="SupportedLanguages"/> defined.
    /// </para>
    /// </remarks>
    public class ApiLanguageSupportConfiguration
    {
        /// <summary>Gets or sets the fall back language to use when determining the Accept-Language header value.  This value
        /// is only used when none of the Accept-Header values (or their culture neutral version) match one of the configured <see cref="SupportedLanguages"/>.</summary>
        /// <remarks>
        /// The default configuration for this value is <c>null</c>.
        /// <para>
        /// [see-also] Hypertext Transfer Protocol (3.1.3.1.  Language Tags: <see href="https://tools.ietf.org/html/rfc7231#section-3.1.3.1"/>)
        /// </para>
        /// </remarks>
        /// <value>The fall back language.</value>
        public string FallBackLanguage { get; set; }

        /// <summary>A list of language codes that will be used to match with the request Accept-Language header value.  If a match is made, the <see cref="ApiRequestInfo.AcceptLanguage"/>
        /// will be set to the <see cref="CultureInfo"/> representing the matched language code.</summary>
        /// <remarks>
        /// The default configuration for this value is an empty list.
        /// </remarks>
        /// <value>The supported languages.</value>
        public IList<string> SupportedLanguages { get; set; }

        /// <summary>Gets or sets whether or not a corresponding <see cref="CultureInfo"/> object to a matched Accept-Header language code (or the <see cref="FallBackLanguage"/>) should be applied to the 
        /// request threads <see cref="CultureInfo.CurrentUICulture"/> object.</summary>
        /// <remarks>
        /// The default configuration for this value is <c>false</c>.
        /// </remarks>
        /// <value>Whether or not to set the <see cref="CultureInfo.CurrentUICulture"/> to a matched Accept-Language header value.</value>
        public bool? UseAcceptedLanguageAsThreadUICulture { get; set; }

        /// <summary>Gets or sets whether or not a corresponding <see cref="CultureInfo"/> object to a matched Accept-Header language code (or the <see cref="FallBackLanguage"/>) should be applied to the 
        /// request threads <see cref="CultureInfo.CurrentCulture"/> object.</summary>
        /// <remarks>
        /// The default configuration for this value is <c>false</c>.
        /// </remarks>
        /// <value>Whether or not to set the <see cref="CultureInfo.CurrentCulture"/> to a matched Accept-Language header value.</value>
        public bool? UseAcceptedLanguageAsThreadCulture { get; set; }
    }
}
