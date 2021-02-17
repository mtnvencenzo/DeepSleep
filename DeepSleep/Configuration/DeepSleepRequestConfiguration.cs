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
    /// <seealso cref="DeepSleep.Configuration.IDeepSleepRequestConfiguration" />
    public class DeepSleepRequestConfiguration : IDeepSleepRequestConfiguration
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

        /// <summary>Gets or sets the enable head for get requests.</summary>
        /// <value>The enable head for get requests.</value>
        public bool? EnableHeadForGetRequests { get; set; }

        /// <summary>Gets or sets the read write configuration.</summary>
        /// <value>The read write configuration.</value>
        public ApiMediaSerializerConfiguration ReadWriteConfiguration { get; set; }

        /// <summary>Gets or sets the validation error configuration.</summary>
        /// <value>The validation error configuration.</value>
        public ApiValidationErrorConfiguration ValidationErrorConfiguration { get; set; }

        /// <summary>Gets or sets the pipeline components.</summary>
        /// <value>The pipeline components.</value>
        public IList<IRequestPipelineComponent> PipelineComponents { get; set; }

        /// <summary>Gets or sets the validators.</summary>
        /// <value>The validators.</value>
        public IList<IEndpointValidatorComponent> Validators { get; set; }

        /// <summary>Gets or sets the authentication providers.</summary>
        /// <value>The authentication providers.</value>
        public IList<IAuthenticationComponent> AuthenticationProviders { get; set; }

        /// <summary>Gets or sets the authorization providers.</summary>
        /// <value>The authorization providers.</value>
        public IList<IAuthorizationComponent> AuthorizationProviders { get; set; }
    }
}
