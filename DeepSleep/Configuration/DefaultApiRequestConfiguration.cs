﻿namespace DeepSleep.Configuration
{
    using DeepSleep.Pipeline;
    using DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Configuration.IApiRequestConfiguration" />
    public class DefaultApiRequestConfiguration : IApiRequestConfiguration
    {
        /// <summary>Gets or sets the cross origin configuration.</summary>
        /// <value>The cross origin configuration.</value>
        public ApiCrossOriginConfiguration CrossOriginConfig { get; set; }

        /// <summary>Gets or sets the API error response provider.</summary>
        /// <value>The API error response provider.</value>
        [JsonIgnore]
        public Func<IServiceProvider, IValidationErrorResponseProvider> ApiErrorResponseProvider { get; set; }

        /// <summary>Gets or sets a value indicating whether [allow anonymous].</summary>
        /// <value><c>true</c> if [allow anonymous]; otherwise, <c>false</c>.</value>
        public bool? AllowAnonymous { get; set; }

        /// <summary>Gets or sets the caching directive.</summary>
        /// <value>The caching directive.</value>
        public ApiCacheDirectiveConfiguration CacheDirective { get; set; }

        /// <summary>Gets or sets the request validation.</summary>
        /// <value>The request validation.</value>
        public ApiRequestValidationConfiguration RequestValidation { get; set; }

        /// <summary>Gets or sets the language support.</summary>
        /// <value>The language support.</value>
        public ApiLanguageSupportConfiguration LanguageSupport { get; set; }

        /// <summary>Gets or sets a value indicating whether the resource is deprecated.</summary>
        /// <value><c>true</c> if deprecated; otherwise, <c>false</c>.</value>
        public bool? Deprecated { get; set; }

        /// <summary>Gets or sets the supported authentication schemes.  If not provided all available schemes are supported.</summary>
        /// <value>The supported authentication schemes.</value>
        public IList<string> SupportedAuthenticationSchemes { get; set; }

        /// <summary>Gets or sets the supported authorization configuration.</summary>
        /// <value>The supported authorization configuration.</value>
        public ApiResourceAuthorizationConfiguration AuthorizationConfig { get; set; }

        /// <summary>Gets or sets the include request identifier header in response.</summary>
        /// <value>The include request identifier header in response.</value>
        public bool? IncludeRequestIdHeaderInResponse { get; set; }

        /// <summary>Gets or sets the enable head for get requests.</summary>
        /// <value>The enable head for get requests.</value>
        public bool? EnableHeadForGetRequests { get; set; }

        /// <summary>Gets or sets the read write configuration.</summary>
        /// <value>The read write configuration.</value>
        public ApiReadWriteConfiguration ReadWriteConfiguration { get; set; }

        /// <summary>Gets or sets the validation error configuration.</summary>
        /// <value>The validation error configuration.</value>
        public ApiValidationErrorConfiguration ValidationErrorConfiguration { get; set; }

        /// <summary>Gets or sets the pipeline components.</summary>
        /// <value>The pipeline components.</value>
        public IList<IRequestPipelineComponent> PipelineComponents { get; set; }

        /// <summary>Gets or sets the validators.</summary>
        /// <value>The validators.</value>
        public IList<IEndpointValidatorComponent> Validators { get; set; }
    }
}
