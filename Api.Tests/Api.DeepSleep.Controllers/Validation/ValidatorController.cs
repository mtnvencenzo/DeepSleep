namespace Api.DeepSleep.Controllers.Validation
{
    using global::DeepSleep;
    using global::DeepSleep.Configuration;
    using global::DeepSleep.Discovery;
    using global::DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ValidatorController : IRouteRegistrationProvider
    {
        [ApiRoute(new[] { "GET" }, "validators/get")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator1), continuation: ValidationContinuation.Always, 2)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator3), continuation: ValidationContinuation.OnlyIfValid, 1)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator2), continuation: ValidationContinuation.Always, 0)]
        public IApiResponse Get()
        {
            return ApiResponse.BadRequest(errors: new string[] { "Test Error" });
        }

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

        [ApiRoute(new[] { "GET" }, "validators/get/failure/404")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator5), order: -1)]
        public IApiResponse GetFailureWith404StatusCode()
        {
            return ApiResponse.BadRequest(errors: new string[] { "Test Error" });
        }

        [ApiEndpointValidation(validatorType: typeof(RequestPipelineModelValidator5), continuation: ValidationContinuation.Always, order: 6)]
        public IApiResponse GetFailureWithMixedConfiguredAndAttribute()
        {
            return ApiResponse.BadRequest(errors: new string[] { "Test Error" });
        }

        public Task<IList<ApiRouteRegistration>> GetRoutes(IServiceProvider serviceProvider)
        {
            var routes = new List<ApiRouteRegistration>();

            routes.Add(new ApiRouteRegistration(
                template: "validators/get/failure/mixed/configured",
                httpMethods: new string[] { "GET" },
                controller: this.GetType(),
                endpoint: nameof(GetFailureWithMixedConfiguredAndAttribute),
                config: new DefaultApiRequestConfiguration
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

            return Task.FromResult(routes as IList<ApiRouteRegistration>);
        }
    }

    public class RequestPipelineModelValidator1 : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            var suggestedStatusCode = args?.ApiContext.Configuration.ValidationErrorConfiguration.UriBindingErrorStatusCode;

            return Task.FromResult(ApiValidationResult.Single("VALIDATOR-1", suggestedHttpStatusCode: suggestedStatusCode));
        }
    }

    public class RequestPipelineModelValidator2 : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            var suggestedStatusCode = args?.ApiContext.Configuration.ValidationErrorConfiguration.UriBindingErrorStatusCode;

            return Task.FromResult(ApiValidationResult.Single("VALIDATOR-2", suggestedHttpStatusCode: suggestedStatusCode));
        }
    }

    public class RequestPipelineModelValidator3 : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            var suggestedStatusCode = args?.ApiContext.Configuration.ValidationErrorConfiguration.UriBindingErrorStatusCode;

            return Task.FromResult(ApiValidationResult.Single("VALIDATOR-3", suggestedHttpStatusCode: suggestedStatusCode));
        }
    }

    public class RequestPipelineModelValidator4 : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {

            var suggestedStatusCode = args?.ApiContext.Configuration.ValidationErrorConfiguration.UriBindingErrorStatusCode;

            var results = new List<ApiValidationResult>();

            results.Add(ApiValidationResult.Failure("VALIDATOR-4.1", suggestedHttpStatusCode: suggestedStatusCode));
            results.Add(ApiValidationResult.Failure("VALIDATOR-4.2", suggestedHttpStatusCode: suggestedStatusCode));

            return Task.FromResult(results as IList<ApiValidationResult>);
        }
    }

    public class RequestPipelineModelValidator5 : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            return Task.FromResult(ApiValidationResult.Single("VALIDATOR-5", suggestedHttpStatusCode: 404));
        }
    }

    public class RequestPipelineModelValidatorSuccess4 : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            return Task.FromResult(null as IList<ApiValidationResult>);
        }
    }

    public class RequestPipelineModelValidatorSuccess5 : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            return Task.FromResult(ApiValidationResult.Success());
        }
    }
}
