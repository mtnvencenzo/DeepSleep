namespace DeepSleep
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>The API request context.</summary>
    public class ApiRequestContext
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRequestContext"/> class.</summary>
        public ApiRequestContext()
        {
            RequestInfo = new ApiRequestInfo();
            ProcessingInfo = new ApiProcessingInfo();
            Items = new Dictionary<string, object>();
            RouteInfo = new ApiRoutingInfo();
            ResponseInfo = new ApiResponseInfo();
            RequestAborted = new CancellationToken(false);
            RequestConfig = new DefaultApiRequestConfiguration();
        }

        /// <summary>Gets the items.</summary>
        /// <value>The items.</value>
        public virtual Dictionary<string, object> Items { get; private set; }

        /// <summary>Gets the request information.</summary>
        /// <value>The request information.</value>
        public virtual ApiRequestInfo RequestInfo { get; set; }

        /// <summary>Gets or sets the request configuration.</summary>
        /// <value>The request configuration.</value>
        public virtual IApiRequestConfiguration RequestConfig { get; set; }

        /// <summary>Gets or sets the response information.</summary>
        /// <value>The response information.</value>
        public virtual ApiResponseInfo ResponseInfo { get; set; }

        /// <summary>Gets or sets the route information.</summary>
        /// <value>The route information.</value>
        public virtual ApiRoutingInfo RouteInfo { get; set; }

        /// <summary>Gets the response information.</summary>
        /// <value>The response information.</value>
        public virtual ApiProcessingInfo ProcessingInfo { get; set; }

        /// <summary>Gets or sets the extended messages.</summary>
        /// <value>The extended messages.</value>
        public virtual List<string> ErrorMessages { get; set; } = new List<string>();

        /// <summary>Gets or sets the path base.</summary>
        /// <value>The path base.</value>
        public virtual string PathBase { get; set; }

        /// <summary>Gets or sets the request aborted.</summary>
        /// <value>The request aborted.</value>
        public virtual CancellationToken RequestAborted { get; set; }

        /// <summary>Gets or sets the request services.</summary>
        /// <value>The request services.</value>
        public virtual IServiceProvider RequestServices { get; set; }

        /// <summary>Adds the response cookie.</summary>
        /// <param name="cookie">The cookie.</param>
        /// <returns></returns>
        public ApiRequestContext AddResponseCookie(ApiCookie cookie)
        {
            this.ResponseInfo.Cookies.Add(cookie);
            return this;
        }

        /// <summary>Gets or sets the register for dispose.</summary>
        /// <value>The register for dispose.</value>
        public Action<IDisposable> RegisterForDispose { get; set; }

        /// <summary>Gets the default request configuration.</summary>
        /// <returns></returns>
        public static IApiRequestConfiguration GetDefaultRequestConfiguration()
        {
            return new DefaultApiRequestConfiguration
            {
                AllowAnonymous = false,
                ApiErrorResponseProvider = (p) => new ApiResultErrorResponseProvider(),
                CacheDirective = new HttpCacheDirective
                {
                    Cacheability = HttpCacheType.NoCache,
                    CacheLocation = HttpCacheLocation.Private,
                    ExpirationSeconds = -1
                },
                CrossOriginConfig = new CrossOriginConfiguration
                {
                    AllowCredentials = true,
                    AllowedOrigins = new List<string> { "*" },
                    AllowedHeaders = new List<string> { "*" },
                    ExposeHeaders = new List<string>()
                },
                Deprecated = false,
                FallBackLanguage = null,
                HeaderValidationConfig = new ApiHeaderValidationConfiguration
                {
                    MaxHeaderLength = 0
                },
                HttpConfig = new ApiHttpConfiguration
                {
                    RequireSSL = false,
                    SupportedVersions = new List<string>
                    {
                        "http/1.1",
                        "http/1.2",
                        "http/2",
                        "http/2.0",
                        "http/2.1"
                    }
                },
                AllowRequestBodyWhenNoModelDefined = false,
                RequireContentLengthOnRequestBodyRequests = true,
                MaxRequestLength = 0,
                MaxRequestUriLength = 0,
                MinRequestLength = 0,
                SupportedLanguages = new List<string>(),
                SupportedAuthenticationSchemes = new List<string>(),
                AuthorizationConfig = new ResourceAuthorizationConfiguration
                {
                    Policy = null
                }
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestContextExtensionMethods
    {
        /// <summary>Validations the state.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static ApiValidationState ValidationState(this ApiRequestContext context)
        {
            return (context?.ProcessingInfo?.Validation?.State ?? ApiValidationState.NotAttempted);
        }

        /// <summary>Adds the exception.</summary>
        /// <param name="request">The request.</param>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static ApiRequestContext AddException(this ApiRequestContext request, Exception ex)
        {
            request.ProcessingInfo.Exceptions.Add(ex);
            return request;
        }

        /// <summary>Adds the item.</summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="contextKey">The context key.</param>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static ApiRequestContext AddItem<TKey, TItem>(this ApiRequestContext request, string contextKey, TKey key, TItem item)
        {
            RequestContextItemGroup<TKey, TItem> itemGroup;
            if (!request.TryGetItemGroup(contextKey, out itemGroup))
            {
                itemGroup = new RequestContextItemGroup<TKey, TItem>();
                request.Items.Add(contextKey, itemGroup);
            }

            if (!itemGroup.Items.ContainsKey(key))
            {
                itemGroup.Items.Add(key, item);
            }

            return request;
        }

        /// <summary>Tries the get item.</summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="contextKey">The context key.</param>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>The <see cref="bool" />.</returns>
        public static bool TryGetItem<TKey, TItem>(this ApiRequestContext request, string contextKey, TKey key, out TItem item)
        {
            item = default;

            RequestContextItemGroup<TKey, TItem> itemGroup;
            if (!request.TryGetItemGroup(contextKey, out itemGroup))
            {
                itemGroup = new RequestContextItemGroup<TKey, TItem>();
                request.Items.Add(contextKey, itemGroup);
            }

            if (!itemGroup.Items.ContainsKey(key))
            {
                return false;
            }

            item = itemGroup.Items[key];
            return true;
        }

        /// <summary>Determines whether [is conditional request match] [the specified response].</summary>
        /// <param name="context">The context.</param>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        public static ApiCondtionalMatchType IsConditionalRequestMatch(this ApiRequestContext context, ApiResponseInfo response)
        {
            var etag = response?.Headers.GetValue("ETag");
            var lastModifiedRaw = response?.Headers.GetValue("Last-Modified");

            DateTimeOffset? lastModified = null;
            if (DateTimeOffset.TryParse(lastModifiedRaw, out var parsed))
            {
                lastModified = parsed;
            }

            return IsConditionalRequestMatch(context, etag, lastModified);
        }

        /// <summary>Determines whether [is conditional request match] [the specified etag].</summary>
        /// <param name="context">The context.</param>
        /// <param name="etag">The etag.</param>
        /// <param name="lastModified">The last modified.</param>
        /// <returns></returns>
        public static ApiCondtionalMatchType IsConditionalRequestMatch(this ApiRequestContext context, string etag, DateTimeOffset? lastModified)
        {
            var condtionalRequestETag = context.RequestInfo.IfMatch;
            var condtionalRequestLastModfied = context.RequestInfo.IfModifiedSince;

            // Conditional Get Request
            if (!string.IsNullOrWhiteSpace(condtionalRequestETag) || condtionalRequestLastModfied != null)
            {
                var match = true;
                if (!string.IsNullOrWhiteSpace(condtionalRequestETag) && condtionalRequestETag != etag)
                {
                    match = false;
                }

                if (condtionalRequestLastModfied != null && condtionalRequestLastModfied.Value.ToString("r") != lastModified?.ToString("r"))
                {
                    match = false;
                }

                if (match)
                {
                    return ApiCondtionalMatchType.ConditionalGetMatch;
                }
            }

            // Concurrency Request
            var currencyRequestETag = context.RequestInfo.IfNoneMatch;
            var currencyRequestLastModfied = context.RequestInfo.IfUnmodifiedSince;

            if (!string.IsNullOrWhiteSpace(currencyRequestETag) || currencyRequestLastModfied != null)
            {
                var match = true;
                if (!string.IsNullOrWhiteSpace(currencyRequestETag) && currencyRequestETag == etag)
                {
                    match = false;
                }

                if (currencyRequestLastModfied != null && currencyRequestLastModfied.Value.ToString("r") == lastModified?.ToString("r"))
                {
                    match = false;
                }

                if (match)
                {
                    return ApiCondtionalMatchType.ConditionalConcurrencyNoMatch;
                }
            }

            return ApiCondtionalMatchType.None;
        }

        /// <summary>Sets the HTTP status.</summary>
        /// <param name="context">The context.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public static ApiRequestContext SetHttpStatus(this ApiRequestContext context, int status)
        {
            if (context?.ResponseInfo != null)
            {
                context.ResponseInfo.StatusCode = status;
            }

            return context;
        }

        /// <summary>Sets the HTTP header.</summary>
        /// <param name="context">The context.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ApiRequestContext SetHttpHeader(this ApiRequestContext context, string name, string value)
        {
            if (context?.ResponseInfo != null)
            {
                context.ResponseInfo.AddHeader(name, value);
            }

            return context;
        }

        /// <summary>Tries the get item group.</summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="request">The request.</param>
        /// <param name="contextKey">The context key.</param>
        /// <param name="item">The item.</param>
        /// <returns>The <see cref="bool" />.</returns>
        private static bool TryGetItemGroup<TItem>(this ApiRequestContext request, string contextKey, out TItem item)
        {
            item = default;

            if (!request.Items.ContainsKey(contextKey))
            {
                return false;
            }

            item = (TItem)request.Items[contextKey];
            return true;
        }
    }
}