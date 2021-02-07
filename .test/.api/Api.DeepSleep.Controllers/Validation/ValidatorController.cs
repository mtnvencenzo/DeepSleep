namespace Api.DeepSleep.Controllers.Validation
{
    using global::DeepSleep;
    using global::DeepSleep.Configuration;
    using global::DeepSleep.Discovery;
    using global::DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Discovery.IDeepSleepRegistrationProvider" />
    public class ValidatorController : IDeepSleepRegistrationProvider
    {
        /// <summary>Gets this instance.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "validators/get")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator1), continuation: ValidationContinuation.Always, 2)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator3), continuation: ValidationContinuation.OnlyIfValid, 1)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator2), continuation: ValidationContinuation.Always, 0)]
        public IApiResponse Get()
        {
            return ApiResponse.BadRequest(errors: new string[] { "Test Error" });
        }

        /// <summary>Gets the mixed.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "validators/get/with/mixed/success/failures")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidatorSuccess4), continuation: ValidationContinuation.Always, 0)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator1), continuation: ValidationContinuation.Always, 5)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator3), continuation: ValidationContinuation.OnlyIfValid, 4)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator2), continuation: ValidationContinuation.Always, 3)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidatorSuccess5), continuation: ValidationContinuation.Always, 6)]
        public IApiResponse GetMixed()
        {
            return ApiResponse.BadRequest(errors: new string[] { "Test Error" });
        }

        /// <summary>Gets the mixed multi failures.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "validators/get/with/mixed/success/failures/multi")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator1), continuation: ValidationContinuation.Always, 5)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator3), continuation: ValidationContinuation.OnlyIfValid, 4)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator2), continuation: ValidationContinuation.Always, 3)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator4), continuation: ValidationContinuation.Always, 2)]
        public IApiResponse GetMixedMultiFailures()
        {
            return ApiResponse.BadRequest(errors: new string[] { "Test Error" });
        }

        /// <summary>Gets the failure with404 status code.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "validators/get/failure/404")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator5), order: -1)]
        public IApiResponse GetFailureWith404StatusCode()
        {
            return ApiResponse.BadRequest(errors: new string[] { "Test Error" });
        }

        /// <summary>Gets the failure with mixed configured and attribute.</summary>
        /// <returns></returns>
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator5), continuation: ValidationContinuation.Always, order: 6)]
        public IApiResponse GetFailureWithMixedConfiguredAndAttribute()
        {
            return ApiResponse.BadRequest(errors: new string[] { "Test Error" });
        }

        /// <summary>Gets the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public Task<IList<DeepSleepRouteRegistration>> GetRoutes(IServiceProvider serviceProvider)
        {
            var routes = new List<DeepSleepRouteRegistration>();

            routes.Add(new DeepSleepRouteRegistration(
                template: "validators/get/failure/mixed/configured",
                httpMethods: new string[] { "GET" },
                controller: this.GetType(),
                endpoint: nameof(GetFailureWithMixedConfiguredAndAttribute),
                config: new DeepSleepRequestConfiguration
                {
                    AllowAnonymous = true,
                    Validators = new List<IEndpointValidatorComponent>
                    {
                        new EndpointValidatorComponent<RequestPipelineModelValidator1>(continuation: ValidationContinuation.Always, 5),
                        new EndpointValidatorComponent<RequestPipelineModelValidator3>(continuation: ValidationContinuation.OnlyIfValid, 4),
                        new EndpointValidatorComponent<RequestPipelineModelValidator2>(continuation: ValidationContinuation.Always, 3),
                        new EndpointValidatorComponent<RequestPipelineModelValidator4>(continuation: ValidationContinuation.Always, 2)
                    }
                }));

            return Task.FromResult(routes as IList<DeepSleepRouteRegistration>);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Validation.IEndpointValidator" />
    public class RequestPipelineModelValidator1 : IEndpointValidator
    {
        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();
            var suggestedStatusCode = context.Configuration.ValidationErrorConfiguration.UriBindingErrorStatusCode;

            return Task.FromResult(ApiValidationResult.Single("VALIDATOR-1", suggestedHttpStatusCode: suggestedStatusCode));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Validation.IEndpointValidator" />
    public class RequestPipelineModelValidator2 : IEndpointValidator
    {
        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();
            var suggestedStatusCode = context.Configuration.ValidationErrorConfiguration.UriBindingErrorStatusCode;

            return Task.FromResult(ApiValidationResult.Single("VALIDATOR-2", suggestedHttpStatusCode: suggestedStatusCode));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Validation.IEndpointValidator" />
    public class RequestPipelineModelValidator3 : IEndpointValidator
    {
        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="apiRequestContextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver apiRequestContextResolver)
        {
            var context = apiRequestContextResolver?.GetContext();
            var suggestedStatusCode = context.Configuration.ValidationErrorConfiguration.UriBindingErrorStatusCode;

            return Task.FromResult(ApiValidationResult.Single("VALIDATOR-3", suggestedHttpStatusCode: suggestedStatusCode));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Validation.IEndpointValidator" />
    public class RequestPipelineModelValidator4 : IEndpointValidator
    {
        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();
            var suggestedStatusCode = context.Configuration.ValidationErrorConfiguration.UriBindingErrorStatusCode;

            var results = new List<ApiValidationResult>();

            results.Add(ApiValidationResult.Failure("VALIDATOR-4.1", suggestedHttpStatusCode: suggestedStatusCode));
            results.Add(ApiValidationResult.Failure("VALIDATOR-4.2", suggestedHttpStatusCode: suggestedStatusCode));

            return Task.FromResult(results as IList<ApiValidationResult>);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Validation.IEndpointValidator" />
    public class RequestPipelineModelValidator5 : IEndpointValidator
    {
        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            return Task.FromResult(ApiValidationResult.Single("VALIDATOR-5", suggestedHttpStatusCode: 404));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Validation.IEndpointValidator" />
    public class RequestPipelineModelValidatorSuccess4 : IEndpointValidator
    {
        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            return Task.FromResult(null as IList<ApiValidationResult>);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Validation.IEndpointValidator" />
    public class RequestPipelineModelValidatorSuccess5 : IEndpointValidator
    {
        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            return Task.FromResult(ApiValidationResult.Success());
        }
    }
}
