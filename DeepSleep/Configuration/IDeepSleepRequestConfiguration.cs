namespace DeepSleep.Configuration
{
    using DeepSleep.Auth;
    using DeepSleep.Pipeline;
    using DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public interface IDeepSleepRequestConfiguration
    {
        /// <summary>Gets or sets the cross origin configuration.</summary>
        /// <value>The cross origin configuration.</value>
        ApiCrossOriginConfiguration CrossOriginConfig { get; set; }

        /// <summary>Gets or sets the API error response provider.</summary>
        /// <value>The API error response provider.</value>
        [JsonIgnore]
        Func<IServiceProvider, IValidationErrorResponseProvider> ApiErrorResponseProvider { get; set; }

        /// <summary>Gets or sets a value indicating whether [allow anonymous].</summary>
        /// <value><c>true</c> if [allow anonymous]; otherwise, <c>false</c>.</value>
        bool? AllowAnonymous { get; set; }

        /// <summary>Gets or sets the caching directive.</summary>
        /// <value>The caching directive.</value>
        ApiCacheDirectiveConfiguration CacheDirective { get; set; }

        /// <summary>Gets or sets the request validation.</summary>
        /// <value>The request validation.</value>
        ApiRequestValidationConfiguration RequestValidation { get; set; }

        /// <summary>Gets or sets the language support.</summary>
        /// <value>The language support.</value>
        ApiLanguageSupportConfiguration LanguageSupport { get; set; }

        /// <summary>Gets or sets a value indicating whether the resource is deprecated.</summary>
        /// <value><c>true</c> if deprecated; otherwise, <c>false</c>.</value>
        bool? Deprecated { get; set; }

        /// <summary>Gets or sets the enable head for get requests.</summary>
        /// <value>The enable head for get requests.</value>
        bool? EnableHeadForGetRequests { get; set; }

        /// <summary>Gets or sets the include request identifier header in response.</summary>
        /// <value>The include request identifier header in response.</value>
        bool? IncludeRequestIdHeaderInResponse { get; set; }

        /// <summary>Gets or sets the read write configuration.</summary>
        /// <value>The read write configuration.</value>
        ApiMediaSerializerConfiguration ReadWriteConfiguration { get; set; }

        /// <summary>Gets or sets the validation error configuration.</summary>
        /// <value>The validation error configuration.</value>
        ApiValidationErrorConfiguration ValidationErrorConfiguration { get; set; }

        /// <summary>Gets or sets the pipeline components.</summary>
        /// <value>The pipeline components.</value>
        IList<IRequestPipelineComponent> PipelineComponents { get; set; }

        /// <summary>Gets or sets the validators.</summary>
        /// <value>The validators.</value>
        IList<IEndpointValidatorComponent> Validators { get; set; }

        /// <summary>Gets or sets the authentication providers.</summary>
        /// <value>The authentication providers.</value>
        IList<IAuthenticationComponent> AuthenticationProviders { get; set; }

        /// <summary>Gets or sets the authorization providers.</summary>
        /// <value>The authorization providers.</value>
        IList<IAuthorizationComponent> AuthorizationProviders { get; set; }
    }
}
