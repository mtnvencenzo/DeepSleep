namespace DeepSleep.Pipeline
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestLocalizationPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestLocalizationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestLocalizationPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestLocalization().ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestLocalizationPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request localization.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestLocalization(this IApiRequestPipeline pipeline)
        {
            //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            //CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            return pipeline.UsePipelineComponent<ApiRequestLocalizationPipelineComponent>();
        }

        /// <summary>Processes the HTTP request localization.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal static Task<bool> ProcessHttpRequestLocalization(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                var fallBackLanguage = !string.IsNullOrWhiteSpace(context.RequestConfig?.FallBackLanguage)
                    ? context.RequestConfig.FallBackLanguage
                    : CultureInfo.CurrentUICulture.Name;

                var supportedLanguages = context.RequestConfig?.SupportedLanguages != null
                    ? context.RequestConfig.SupportedLanguages
                    : new string[] { };

                var acceptedLanguage = GetAcceptedSupportedLanguage(supportedLanguages, context.RequestInfo?.AcceptLanguage?.Values);

                if (string.IsNullOrWhiteSpace(acceptedLanguage))
                {
                    acceptedLanguage = GetAcceptedSupportedLanguage(supportedLanguages, new LanguageValueWithQuality[] {
                        new LanguageValueWithQuality
                        {
                            Code = fallBackLanguage,
                            Parameters = new List<string>(),
                            Quality = 1.0f
                        }
                    });
                }


                if (string.IsNullOrWhiteSpace(acceptedLanguage) && context.RequestInfo?.AcceptLanguage?.Values != null)
                {
                    var neutralLangs = context.RequestInfo.AcceptLanguage.Values
                        .Where(s => (s.Code?.Trim()?.Length ?? 0) > 1)
                        .Select(s => new LanguageValueWithQuality
                        {
                            Code = s.Code.Trim().Substring(0, 2),
                            Parameters = s.Parameters,
                            Quality = s.Quality
                        });

                    acceptedLanguage = GetAcceptedSupportedLanguage(supportedLanguages, neutralLangs);
                }

                if (string.IsNullOrWhiteSpace(acceptedLanguage))
                {
                    acceptedLanguage = fallBackLanguage;
                }

                if (string.IsNullOrWhiteSpace(acceptedLanguage))
                {
                    context.RequestInfo.AcceptCulture = CultureInfo.CurrentUICulture;
                }
                else
                {
                    var culture = new CultureInfo(acceptedLanguage);
                    context.RequestInfo.AcceptCulture = culture;
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        /// <summary>Gets the accepted supported language.</summary>
        /// <param name="supportedLanguages">The supported languages.</param>
        /// <param name="acceptLanguages">The accept languages.</param>
        /// <returns></returns>
        internal static string GetAcceptedSupportedLanguage(IEnumerable<string> supportedLanguages, IEnumerable<LanguageValueWithQuality> acceptLanguages)
        {
            if (supportedLanguages == null)
                return string.Empty;
            if (supportedLanguages.Count() == 0)
                return string.Empty;
            if (acceptLanguages == null)
                return string.Empty;
            if (acceptLanguages.Count() == 0)
                return string.Empty;

            var supported = new Dictionary<string, string>();
            foreach (var s in supportedLanguages)
            {
                var prepared = PrepareCultureCodeAsNeutralCulture(s);
                if (!supported.Keys.Any(k => string.Compare(prepared, k, true) == 0))
                {
                    supported.Add(prepared, s);
                }
            }

            var accepted = acceptLanguages
                .OrderByDescending(s => s.Quality)
                .Select(s => PrepareCultureCodeAsNeutralCulture(s.Code));

            foreach (var accept in accepted)
            {
                var key = supported.Keys.FirstOrDefault(s => string.Compare(s, accept, true) == 0);
                if (key != null)
                {
                    return supported[key];
                }
            }

            return string.Empty;
        }

        /// <summary>Prepares the culture code as neutral culture.</summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        internal static string PrepareCultureCodeAsNeutralCulture(string code)
        {
            var cd = code?.Trim();

            if (string.IsNullOrWhiteSpace(cd))
                return cd;

            if (cd.Length == 5)
                return cd;

            if (cd.Length == 2)
                return $"{cd}-{cd.ToUpper()}";

            return cd;
        }
    }
}
