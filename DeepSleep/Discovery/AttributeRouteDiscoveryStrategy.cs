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
    /// <seealso cref="DeepSleep.Discovery.IRouteDiscoveryStrategy" />
    public class AttributeRouteDiscoveryStrategy : IRouteDiscoveryStrategy
    {
        private readonly string assemblyDirectoryPath;
        private readonly string assemblyMatchPattern;

        /// <summary>Initializes a new instance of the <see cref="AttributeRouteDiscoveryStrategy"/> class.</summary>
        /// <param name="assemblyDirectoryPath">The assembly directory path.</param>
        /// <param name="assemblyMatchPattern">The assembly match pattern.</param>
        public AttributeRouteDiscoveryStrategy(string assemblyDirectoryPath, string assemblyMatchPattern = null)
        {
            this.assemblyDirectoryPath = assemblyDirectoryPath;

            this.assemblyMatchPattern = assemblyMatchPattern ?? "*";
        }

        /// <summary>Discovers the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public virtual async Task<IList<ApiRouteRegistration>> DiscoverRoutes(IServiceProvider serviceProvider)
        {
            if (this.assemblyDirectoryPath != null)
            {
                return await this.DiscoverRoutes(serviceProvider, this.assemblyDirectoryPath, this.assemblyMatchPattern).ConfigureAwait(false);
            }

            return new List<ApiRouteRegistration>();
        }

        /// <summary>Discovers the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="assemblyDirectoryPath">The assembly directory path.</param>
        /// <param name="assemblyMatchPattern">The assembly match pattern.</param>
        /// <returns></returns>
        protected virtual Task<IList<ApiRouteRegistration>> DiscoverRoutes(IServiceProvider serviceProvider, string assemblyDirectoryPath, string assemblyMatchPattern)
        {
            var registrations = new List<ApiRouteRegistration>();

            var files = Directory.GetFiles(assemblyDirectoryPath, assemblyMatchPattern, SearchOption.AllDirectories);

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
                    var methods = assembly
                        .GetTypes()
                        .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod))
                        .Where(m => m.GetCustomAttribute<ApiRouteAttribute>() != null);

                    foreach (var method in methods)
                    {
                        var apiRoute = method.GetCustomAttribute<ApiRouteAttribute>(false);

                        DefaultApiRequestConfiguration configuration = null;
                        configuration = this.AssignRouteAttribute(configuration, method.GetCustomAttribute<ApiRouteAttribute>());
                        configuration = this.AssignRouteAuthenticationAttribute(configuration, method.GetCustomAttribute<ApiRouteAuthenticationAttribute>());
                        configuration = this.AssignRouteAuthorizationAttribute(configuration, method.GetCustomAttribute<ApiRouteAuthorizationAttribute>());
                        configuration = this.AssignRouteCacheDirectiveAttribute(configuration, method.GetCustomAttribute<ApiRouteCacheDirectiveAttribute>());
                        configuration = this.AssignRouteCrossOriginAttribute(configuration, method.GetCustomAttribute<ApiRouteCrossOriginAttribute>());
                        configuration = this.AssignRouteEnableHeadAttribute(configuration, method.GetCustomAttribute<ApiRouteEnableHeadAttribute>());
                        configuration = this.AssignRouteLanguageAttribute(configuration, method.GetCustomAttribute<ApiRouteLanguageSupportAttribute>());
                        configuration = this.AssignRouteRequestValidationAttribute(configuration, method.GetCustomAttribute<ApiRouteRequestValidationAttribute>());
                        configuration = this.AssignRouteIncludeRequestIdHeaderAttribute(configuration, method.GetCustomAttribute<ApiRouteIncludeRequestIdHeaderAttribute>());
                        configuration = this.AssignRouteValidationErrorConfigurationAttribute(configuration, method.GetCustomAttribute<ApiRouteValidationErrorConfigurationAttribute>());
                        configuration = this.AssignRouteErrorResponseProviderAttribute(configuration, method.GetCustomAttribute<ApiRouteErrorResponseProviderAttribute>());
                        configuration = this.AssignRouteReadWriteConfigurationAttribute(configuration, method.GetCustomAttribute<ApiRouteReadWriteConfigurationAttribute>());

                        var registration = new ApiRouteRegistration(
                            template: apiRoute.Template,
                            httpMethods: apiRoute.HttpMethods,
                            controller: Type.GetType(method.DeclaringType.AssemblyQualifiedName),
                            endpoint: method.Name,
                            config: configuration);

                        registrations.Add(registration);
                    }
                }
            }

            return Task.FromResult(registrations as IList<ApiRouteRegistration>);
        }

        private DefaultApiRequestConfiguration AssignRouteAttribute(DefaultApiRequestConfiguration configuration, ApiRouteAttribute attribute)
        {
            if (attribute?.Deprecated == null)
            {
                return configuration;
            }

            var config = configuration ?? new DefaultApiRequestConfiguration();

            config.Deprecated = attribute.Deprecated;

            return config;
        }

        private DefaultApiRequestConfiguration AssignRouteAuthenticationAttribute(DefaultApiRequestConfiguration configuration, ApiRouteAuthenticationAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DefaultApiRequestConfiguration();

            config.AllowAnonymous = attribute.AllowAnonymous;
            config.SupportedAuthenticationSchemes = attribute.SupportedAuthenticationSchemes;

            return config;
        }

        private DefaultApiRequestConfiguration AssignRouteAuthorizationAttribute(DefaultApiRequestConfiguration configuration, ApiRouteAuthorizationAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DefaultApiRequestConfiguration();

            config.AuthorizationConfig = new ApiResourceAuthorizationConfiguration
            {
                Policy = attribute.Policy
            };

            return config;
        }

        private DefaultApiRequestConfiguration AssignRouteCacheDirectiveAttribute(DefaultApiRequestConfiguration configuration, ApiRouteCacheDirectiveAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DefaultApiRequestConfiguration();

            config.CacheDirective = new ApiCacheDirectiveConfiguration
            {
                Cacheability = attribute.Cacheability,
                CacheLocation = attribute.Location,
                ExpirationSeconds = attribute.ExpirationSeconds,
                VaryHeaderValue = attribute.VaryHeaderValue
            };

            return config;
        }

        private DefaultApiRequestConfiguration AssignRouteCrossOriginAttribute(DefaultApiRequestConfiguration configuration, ApiRouteCrossOriginAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DefaultApiRequestConfiguration();

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

        private DefaultApiRequestConfiguration AssignRouteEnableHeadAttribute(DefaultApiRequestConfiguration configuration, ApiRouteEnableHeadAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DefaultApiRequestConfiguration();

            config.EnableHeadForGetRequests = attribute.EnableHeadForGetRequests;

            return config;
        }

        private DefaultApiRequestConfiguration AssignRouteLanguageAttribute(DefaultApiRequestConfiguration configuration, ApiRouteLanguageSupportAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DefaultApiRequestConfiguration();

            config.LanguageSupport = new ApiLanguageSupportConfiguration
            {
                FallBackLanguage = attribute.FallbackLanguage,
                SupportedLanguages = attribute.SupportedLanguages,
                UseAcceptedLanguageAsThreadCulture = attribute.UseAcceptedLanguageAsThreadCulture,
                UseAcceptedLanguageAsThreadUICulture = attribute.UseAcceptedLanguageAsThreadUICulture
            };

            return config;
        }

        private DefaultApiRequestConfiguration AssignRouteRequestValidationAttribute(DefaultApiRequestConfiguration configuration, ApiRouteRequestValidationAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DefaultApiRequestConfiguration();

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

        private DefaultApiRequestConfiguration AssignRouteIncludeRequestIdHeaderAttribute(DefaultApiRequestConfiguration configuration, ApiRouteIncludeRequestIdHeaderAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DefaultApiRequestConfiguration();

            config.IncludeRequestIdHeaderInResponse = attribute.IncludeRequestIdHeaderInResponse;

            return config;
        }

        private DefaultApiRequestConfiguration AssignRouteValidationErrorConfigurationAttribute(DefaultApiRequestConfiguration configuration, ApiRouteValidationErrorConfigurationAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DefaultApiRequestConfiguration();

            config.ValidationErrorConfiguration = new ApiValidationErrorConfiguration
            {
                UriBindingError = attribute.UriBindingError,
                UriBindingValueError = attribute.UriBindingValueError,
                RequestDeserializationError = attribute.RequestDeserializationError,
                UseCustomStatusForRequestDeserializationErrors = attribute.UseCustomStatusForRequestDeserializationErrors
            };

            return config;
        }

        private DefaultApiRequestConfiguration AssignRouteErrorResponseProviderAttribute(DefaultApiRequestConfiguration configuration, ApiRouteErrorResponseProviderAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DefaultApiRequestConfiguration();

            config.ApiErrorResponseProvider = (p) => attribute;

            return config;
        }

        private DefaultApiRequestConfiguration AssignRouteReadWriteConfigurationAttribute(DefaultApiRequestConfiguration configuration, ApiRouteReadWriteConfigurationAttribute attribute)
        {
            if (attribute == null)
            {
                return configuration;
            }

            var config = configuration ?? new DefaultApiRequestConfiguration();

            config.ReadWriteConfiguration = new ApiReadWriteConfiguration
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
