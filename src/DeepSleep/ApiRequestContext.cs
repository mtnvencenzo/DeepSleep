﻿namespace DeepSleep
{
    using DeepSleep.Auth;
    using DeepSleep.Configuration;
    using DeepSleep.Media.Converters;
    using DeepSleep.Pipeline;
    using DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading;

    /// <summary>The API request context.</summary>
    [DebuggerDisplay("[{Request?.Method?.ToUpper()}] {PathBase}")]
    public class ApiRequestContext
    {
        /// <summary>Gets the items.</summary>
        /// <value>The items.</value>
        public virtual IDictionary<string, object> Items { get; internal set; } = new Dictionary<string, object>();

        /// <summary>Gets the request information.</summary>
        /// <value>The request information.</value>
        public virtual ApiRequestInfo Request { get; set; } = new ApiRequestInfo();

        /// <summary>Gets or sets the request configuration.</summary>
        /// <value>The request configuration.</value>
        public virtual IDeepSleepRequestConfiguration Configuration { get; set; } = new DeepSleepRequestConfiguration();

        /// <summary>Gets or sets the response information.</summary>
        /// <value>The response information.</value>
        public virtual ApiResponseInfo Response { get; internal set; } = new ApiResponseInfo();

        /// <summary>Gets or sets the routing.</summary>
        /// <value>The routing.</value>
        public virtual ApiRoutingInfo Routing { get; internal set; } = new ApiRoutingInfo();

        /// <summary>Gets the response information.</summary>
        /// <value>The response information.</value>
        public virtual ApiRuntimeInfo Runtime { get; internal set; } = new ApiRuntimeInfo();

        /// <summary>Gets or sets the path base.</summary>
        /// <value>The path base.</value>
        public virtual string PathBase { get; set; }

        /// <summary>Gets or sets the request aborted.</summary>
        /// <value>The request aborted.</value>
        [JsonIgnore]
        public virtual CancellationToken RequestAborted { get; set; } = new CancellationToken(false);

        /// <summary>Gets or sets the request services.</summary>
        /// <value>The request services.</value>
        [JsonIgnore]
        public virtual IServiceProvider RequestServices { get; set; }

        /// <summary>Adds the response cookie.</summary>
        /// <param name="cookie">The cookie.</param>
        /// <returns></returns>
        public virtual ApiRequestContext AddResponseCookie(ApiCookie cookie)
        {
            this.Response.Cookies.Add(cookie);
            return this;
        }

        /// <summary>Gets or sets the register for dispose.</summary>
        /// <value>The register for dispose.</value>
        [JsonIgnore]
        public Action<IDisposable> RegisterForDispose { get; set; }

        /// <summary>Gets or sets the length of the configure maximum request.</summary>
        /// <value>The length of the configure maximum request.</value>
        [JsonIgnore]
        public Action<long> ConfigureMaxRequestLength { get; set; }

        /// <summary>Gets or sets the validation.</summary>
        /// <value>The validation.</value>
        public ApiValidationInfo Validation { get; internal set; } = new ApiValidationInfo();

        /// <summary>Gets the default request configuration.</summary>
        /// <returns></returns>
        public static IDeepSleepRequestConfiguration GetDefaultRequestConfiguration()
        {
            return new DeepSleepRequestConfiguration
            {
                AllowAnonymous = false,
                ApiErrorResponseProvider = (p) => new ValidationErrorResponseProvider(),
                EnableHeadForGetRequests = true,
                CacheDirective = new ApiCacheDirectiveConfiguration
                {
                    Cacheability = HttpCacheType.NoCache,
                    CacheLocation = HttpCacheLocation.Private,
                    ExpirationSeconds = -1,
                    VaryHeaderValue = "Accept, Accept-Encoding, Accept-Language"
                },
                CrossOriginConfig = new ApiCrossOriginConfiguration
                {
                    AllowCredentials = true,
                    AllowedOrigins = new List<string> { "*" },
                    AllowedHeaders = new List<string> { "*" },
                    ExposeHeaders = new List<string>(),
                    MaxAgeSeconds = 0
                },
                RequestValidation = new ApiRequestValidationConfiguration
                {
                    AllowRequestBodyWhenNoModelDefined = false,
                    RequireContentLengthOnRequestBodyRequests = true,
                    MaxRequestLength = null,
                    MaxRequestUriLength = 0,
                    MaxHeaderLength = 0,
                },
                LanguageSupport = new ApiLanguageSupportConfiguration
                {
                    FallBackLanguage = null,
                    SupportedLanguages = new List<string>(),
                    UseAcceptedLanguageAsThreadCulture = false,
                    UseAcceptedLanguageAsThreadUICulture = false
                },
                ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                {
                    ReadableMediaTypes = null,
                    WriteableMediaTypes = null,
                    ReaderResolver = null,
                    WriterResolver = null,
                    AcceptHeaderOverride = null
                },
                ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = "'{paramName}' is in an incorrect format and could not be bound.",
                    UriBindingValueError = "Uri type conversion for '{paramName}' with value '{paramValue}' could not be converted to type {paramType}.",
                    RequestDeserializationError = "The request body could not be deserialized.",
                    HttpStatusMode = ValidationHttpStatusMode.StrictHttpSpecification
                },
                PipelineComponents = new List<IRequestPipelineComponent>(),
                Validators = new List<IEndpointValidatorComponent>(),
                AuthenticationProviders = new List<IAuthenticationComponent>(),
                AuthorizationProviders = new List<IAuthorizationComponent>()
            };
        }

        /// <summary>Dumps this instance.</summary>
        /// <returns></returns>
        public virtual string Dump()
        {
            var settings = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                AllowTrailingCommas = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                IgnoreReadOnlyProperties = false,
                IncludeFields = false,
                NumberHandling = JsonNumberHandling.Strict,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                WriteIndented = true
            };

            settings.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
            settings.Converters.Add(new TimeSpanConverter());
            settings.Converters.Add(new NullableTimeSpanConverter());
            settings.Converters.Add(new ContentDispositionConverter());
            settings.Converters.Add(new ContentTypeConverter());

            string json = JsonSerializer.Serialize(this, settings);
            return json;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestContextExtensionMethods
    {
        private static readonly object __itemSyncLock = new object();

        /// <summary>Validations the state.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static ApiValidationState ValidationState(this ApiRequestContext context)
        {
            return (context?.Validation?.State ?? ApiValidationState.NotAttempted);
        }

        /// <summary>Adds the exception.</summary>
        /// <param name="context">The context.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static ApiRequestContext AddException(this ApiRequestContext context, Exception ex)
        {
            if (context == null)
                return context;

            if (context.Runtime == null)
            {
                context.Runtime = new ApiRuntimeInfo();
            }

            context.Runtime.Exceptions.Add(ex);
            return context;
        }

        /// <summary>Adds the exception.</summary>
        /// <param name="context">The context.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        internal static ApiRequestContext AddInternalException(this ApiRequestContext context, Exception ex)
        {
            if (context == null)
                return context;

            if (context.Runtime == null)
            {
                context.Runtime = new ApiRuntimeInfo();
            }

            context.Runtime.Internals.Exceptions.Add(ex);
            return context;
        }

        /// <summary>Adds the validation error.</summary>
        /// <param name="context">The context.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public static ApiRequestContext AddValidationError(this ApiRequestContext context, string error)
        {
            if (context == null)
                return context;

            if (context.Validation == null)
            {
                context.Validation = new ApiValidationInfo();
            }

            if (context.Validation.Errors == null)
            {
                context.Validation.Errors = new List<string>();
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                context.Validation.Errors.Add(error);
            }
            return context;
        }

        /// <summary>Adds the validation errors.</summary>
        /// <param name="context">The context.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static ApiRequestContext AddValidationErrors(this ApiRequestContext context, IList<string> errors)
        {
            if (context == null || errors == null)
                return context;

            foreach (var error in errors)
            {
                context.AddValidationError(error);
            }

            return context;
        }

        /// <summary>Upserts the item.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static ApiRequestContext UpsertItem<TItem>(this ApiRequestContext context, string key, TItem item)
        {
            lock (__itemSyncLock)
            {
                if (context == null)
                {
                    return context;
                }

                if (context.Items == null)
                {
                    context.Items = new Dictionary<string, object>();
                }

                if (context.Items.ContainsKey(key))
                {
                    context.Items[key] = item;
                }
                else
                {
                    context.Items.Add(key, item);
                }
            }

            return context;
        }

        /// <summary>Tries the add item.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static bool TryAddItem<TItem>(this ApiRequestContext context, string key, TItem item)
        {
            lock (__itemSyncLock)
            {
                if (context == null)
                {
                    return false;
                }

                if (context.Items == null)
                {
                    context.Items = new Dictionary<string, object>();
                }

                if (context.Items.ContainsKey(key))
                {
                    return false;
                }
                else
                {
                    context.Items.Add(key, item);
                    return true;
                }
            }
        }

        /// <summary>Tries the get item.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static bool TryGetItem<TItem>(this ApiRequestContext context, string key, out TItem item)
        {
            lock (__itemSyncLock)
            {
                item = default;

                if (context?.Items == null)
                {
                    return false;
                }

                if (context.Items.ContainsKey(key))
                {
                    try
                    {
                        item = (TItem)context.Items[key];
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        /// <summary>Sets the thread culure.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static ApiRequestContext SetThreadCulure(this ApiRequestContext context)
        {
            if (context?.Runtime?.Internals == null)
            {
                return context;
            }

            if (context.Runtime.Internals.CurrentCulture != null && CultureInfo.CurrentCulture != context.Runtime.Internals.CurrentCulture)
            {
                CultureInfo.CurrentCulture = context.Runtime.Internals.CurrentCulture;
            }

            if (context.Runtime.Internals.CurrentUICulture != null && CultureInfo.CurrentUICulture != context.Runtime.Internals.CurrentUICulture)
            {
                CultureInfo.CurrentUICulture = context.Runtime.Internals.CurrentUICulture;
            }

            return context;
        }

        /// <summary>Logs the specified message.</summary>
        /// <param name="context">The context.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static ApiRequestContext Log(this ApiRequestContext context, string message)
        {
            if (context?.Runtime != null)
            {
                context.Runtime.Log(message);
            }

            return context;
        }
    }
}