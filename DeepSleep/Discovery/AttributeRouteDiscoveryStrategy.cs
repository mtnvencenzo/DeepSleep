namespace DeepSleep.Discovery
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Discovery.IDeepSleepDiscoveryStrategy" />
    public class AttributeRouteDiscoveryStrategy : IDeepSleepDiscoveryStrategy
    {
        /// <summary>The assembly directory path</summary>
        protected readonly string assemblyDirectoryPath;
        /// <summary>The assembly match pattern</summary>
        protected readonly string assemblyMatchPattern;
        /// <summary>The search option</summary>
        protected readonly SearchOption searchOption;

        /// <summary>Initializes a new instance of the <see cref="AttributeRouteDiscoveryStrategy" /> class.</summary>
        /// <param name="assemblyDirectoryPath">The assembly directory path.</param>
        /// <param name="assemblyMatchPattern">The assembly match pattern.</param>
        /// <param name="searchOption">The search option.</param>
        public AttributeRouteDiscoveryStrategy(
            string assemblyDirectoryPath, 
            string assemblyMatchPattern = null,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            this.assemblyDirectoryPath = assemblyDirectoryPath;

            this.assemblyMatchPattern = assemblyMatchPattern ?? "*";

            this.searchOption = searchOption;
        }

        /// <summary>Discovers the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public virtual async Task<IList<DeepSleepRouteRegistration>> DiscoverRoutes(
            IServiceProvider serviceProvider)
        {
            if (this.assemblyDirectoryPath != null)
            {
                return await this.DiscoverRoutes(serviceProvider, this.assemblyDirectoryPath, this.assemblyMatchPattern).ConfigureAwait(false);
            }

            return new List<DeepSleepRouteRegistration>();
        }

        /// <summary>Discovers the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="assemblyDirectoryPath">The assembly directory path.</param>
        /// <param name="assemblyMatchPattern">The assembly match pattern.</param>
        /// <returns></returns>
        protected virtual Task<IList<DeepSleepRouteRegistration>> DiscoverRoutes(
            IServiceProvider serviceProvider, 
            string assemblyDirectoryPath, 
            string assemblyMatchPattern)
        {
            var registrations = new List<DeepSleepRouteRegistration>();

            var files = Directory.GetFiles(assemblyDirectoryPath, assemblyMatchPattern, this.searchOption);

            foreach (var file in files)
            {
                Assembly assembly = null;

                try
                {
                    assembly = Assembly.LoadFile(file);
                }
                catch { }


                if (assembly != null)
                {
                    IEnumerable<MethodInfo> methods = new List<MethodInfo>();

                    try
                    {
                        methods = assembly
                            .GetTypes()
                            .SelectMany(t => t.GetMethods(bindingAttr: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod))
                            .Where(m => m.GetCustomAttribute<ApiRouteAttribute>() != null);
                    }
                    catch { }

                    foreach (var method in methods)
                    {
                        var apiRoute = method.GetCustomAttribute<ApiRouteAttribute>(false);

                        DeepSleepRequestConfiguration configuration = null;
                        configuration = this.AssignRouteAttribute(configuration, method.GetCustomAttribute<ApiRouteAttribute>());
                        configuration = this.AssignRouteAllowAnonymousAttribute(configuration, method.GetCustomAttribute<ApiRouteAllowAnonymousAttribute>());
                        configuration = this.AssignRouteCacheDirectiveAttribute(configuration, method.GetCustomAttribute<ApiRouteCacheDirectiveAttribute>());
                        configuration = this.AssignRouteCrossOriginAttribute(configuration, method.GetCustomAttribute<ApiRouteCrossOriginAttribute>());
                        configuration = this.AssignRouteEnableHeadAttribute(configuration, method.GetCustomAttribute<ApiRouteEnableHeadAttribute>());
                        configuration = this.AssignRouteLanguageAttribute(configuration, method.GetCustomAttribute<ApiRouteLanguageSupportAttribute>());
                        configuration = this.AssignRouteRequestValidationAttribute(configuration, method.GetCustomAttribute<ApiRouteRequestValidationAttribute>());
                        configuration = this.AssignRouteIncludeRequestIdHeaderAttribute(configuration, method.GetCustomAttribute<ApiRouteIncludeRequestIdHeaderAttribute>());
                        configuration = this.AssignRouteValidationErrorConfigurationAttribute(configuration, method.GetCustomAttribute<ApiRouteValidationErrorConfigurationAttribute>());
                        configuration = this.AssignRouteErrorResponseProviderAttribute(configuration, method.GetCustomAttribute<ApiRouteErrorResponseProviderAttribute>());
                        configuration = this.AssignRouteMediaSerializerConfigurationAttribute(configuration, method.GetCustomAttribute<ApiRouteMediaSerializerConfigurationAttribute>());
                        configuration = this.AssignRouteUseCorrelationIdHeaderAttribute(configuration, method.GetCustomAttribute<ApiRouteUseCorrelationIdHeaderAttribute>());

                        var registration = new DeepSleepRouteRegistration(
                            template: apiRoute.Template,
                            httpMethods: apiRoute.HttpMethods,
                            controller: Type.GetType(method.DeclaringType.AssemblyQualifiedName),
                            methodInfo: method,
                            config: configuration);

                        registrations.Add(registration);
                    }
                }
            }

            return Task.FromResult(registrations as IList<DeepSleepRouteRegistration>);
        }

        private DeepSleepRequestConfiguration AssignRouteAttribute(DeepSleepRequestConfiguration configuration, ApiRouteAttribute attribute)
        {
            if (attribute?.Deprecated == null)
            {
                return configuration;
            }

            var config = configuration ?? new DeepSleepRequestConfiguration();

            config.Deprecated = attribute.Deprecated;

            return config;
        }

        private DeepSleepRequestConfiguration AssignRouteAllowAnonymousAttribute(DeepSleepRequestConfiguration configuration, ApiRouteAllowAnonymousAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DeepSleepRequestConfiguration();

            config.AllowAnonymous = attribute.AllowAnonymous;

            return config;
        }

        private DeepSleepRequestConfiguration AssignRouteUseCorrelationIdHeaderAttribute(DeepSleepRequestConfiguration configuration, ApiRouteUseCorrelationIdHeaderAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DeepSleepRequestConfiguration();

            config.UseCorrelationIdHeader = attribute.UseCorrelationIdHeader;

            return config;
        }

        private DeepSleepRequestConfiguration AssignRouteCacheDirectiveAttribute(DeepSleepRequestConfiguration configuration, ApiRouteCacheDirectiveAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DeepSleepRequestConfiguration();

            config.CacheDirective = new ApiCacheDirectiveConfiguration
            {
                Cacheability = attribute.Cacheability,
                CacheLocation = attribute.Location,
                ExpirationSeconds = attribute.ExpirationSeconds,
                VaryHeaderValue = attribute.VaryHeaderValue
            };

            return config;
        }

        private DeepSleepRequestConfiguration AssignRouteCrossOriginAttribute(DeepSleepRequestConfiguration configuration, ApiRouteCrossOriginAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DeepSleepRequestConfiguration();

            config.CrossOriginConfig = new ApiCrossOriginConfiguration
            {
                AllowCredentials = attribute.AllowCredentials,
                AllowedHeaders = attribute.AllowedHeaders,
                AllowedOrigins = attribute.AllowedOrigins,
                MaxAgeSeconds = attribute.MaxAgeSeconds,
                ExposeHeaders = attribute.ExposeHeaders
            };

            return config;
        }

        private DeepSleepRequestConfiguration AssignRouteEnableHeadAttribute(DeepSleepRequestConfiguration configuration, ApiRouteEnableHeadAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DeepSleepRequestConfiguration();

            config.EnableHeadForGetRequests = attribute.EnableHeadForGetRequests;

            return config;
        }

        private DeepSleepRequestConfiguration AssignRouteLanguageAttribute(DeepSleepRequestConfiguration configuration, ApiRouteLanguageSupportAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DeepSleepRequestConfiguration();

            config.LanguageSupport = new ApiLanguageSupportConfiguration
            {
                FallBackLanguage = attribute.FallbackLanguage,
                SupportedLanguages = attribute.SupportedLanguages,
                UseAcceptedLanguageAsThreadCulture = attribute.UseAcceptedLanguageAsThreadCulture,
                UseAcceptedLanguageAsThreadUICulture = attribute.UseAcceptedLanguageAsThreadUICulture
            };

            return config;
        }

        private DeepSleepRequestConfiguration AssignRouteRequestValidationAttribute(DeepSleepRequestConfiguration configuration, ApiRouteRequestValidationAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DeepSleepRequestConfiguration();

            config.RequestValidation = new ApiRequestValidationConfiguration
            {
                MaxHeaderLength = attribute.MaxHeaderLength,
                MaxRequestLength = attribute.MaxRequestLength,
                MaxRequestUriLength = attribute.MaxRequestUriLength,
                AllowRequestBodyWhenNoModelDefined = attribute.AllowRequestBodyWhenNoModelDefined,
                RequireContentLengthOnRequestBodyRequests = attribute.RequireContentLengthOnRequestBodyRequests
            };

            return config;
        }

        private DeepSleepRequestConfiguration AssignRouteIncludeRequestIdHeaderAttribute(DeepSleepRequestConfiguration configuration, ApiRouteIncludeRequestIdHeaderAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DeepSleepRequestConfiguration();

            config.IncludeRequestIdHeaderInResponse = attribute.IncludeRequestIdHeaderInResponse;

            return config;
        }

        private DeepSleepRequestConfiguration AssignRouteValidationErrorConfigurationAttribute(DeepSleepRequestConfiguration configuration, ApiRouteValidationErrorConfigurationAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DeepSleepRequestConfiguration();

            config.ValidationErrorConfiguration = new ApiValidationErrorConfiguration
            {
                UriBindingError = attribute.UriBindingError,
                UriBindingValueError = attribute.UriBindingValueError,
                RequestDeserializationError = attribute.RequestDeserializationError,
                HttpStatusMode = attribute.HttpStatusMode
            };

            return config;
        }

        private DeepSleepRequestConfiguration AssignRouteErrorResponseProviderAttribute(DeepSleepRequestConfiguration configuration, ApiRouteErrorResponseProviderAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DeepSleepRequestConfiguration();

            config.ApiErrorResponseProvider = (p) => attribute;

            return config;
        }

        private DeepSleepRequestConfiguration AssignRouteMediaSerializerConfigurationAttribute(DeepSleepRequestConfiguration configuration, ApiRouteMediaSerializerConfigurationAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DeepSleepRequestConfiguration();

            config.ReadWriteConfiguration = new ApiMediaSerializerConfiguration
            {
                ReadableMediaTypes = attribute.ReadableMediaTypes,
                WriteableMediaTypes = attribute.WriteableMediaTypes
            };

            if (attribute.AcceptHeaderOverride != null)
            {
                config.ReadWriteConfiguration.AcceptHeaderOverride = attribute.AcceptHeaderOverride;
            }

            if (attribute as IApiRouteReaderResolver != null)
            {
                config.ReadWriteConfiguration.ReaderResolver = ((IApiRouteReaderResolver)attribute).Resolve;
            }

            if (attribute as IApiRouteWriterResolver != null)
            {
                config.ReadWriteConfiguration.WriterResolver = ((IApiRouteWriterResolver)attribute).Resolve;
            }

            return config;
        }
    }
}
