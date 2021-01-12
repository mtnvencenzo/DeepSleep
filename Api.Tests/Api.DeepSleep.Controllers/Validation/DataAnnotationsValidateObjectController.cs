namespace Api.DeepSleep.Controllers.Validation
{
    using global::DeepSleep;
    using global::DeepSleep.Validation;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class DataAnnotationsValidateObjectController
    {
        [ApiRoute(new[] { "POST" }, "validators/validateobject/uri/and/body/{value}")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        [ApiEndpointValidation(validatorType: typeof(ValidateObjectModelValidator), continuation: ValidationContinuation.OnlyIfValid)]
        public IApiResponse PostMixedEndpointObjectValidations([UriBound] ValidateObjectUriModel uri, [BodyBound] ValidateObjectBodyModel body)
        {
            return ApiResponse.NoContent();
        }
    }

    public class ValidateObjectUriModel : IValidatableObject
    {
        public string Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (Value == null)
            {
                validationResults.Add(new ValidationResult(errorMessage: "Uri value is required"));
            }
            else
            {
                if (!Regex.IsMatch(Value, "^[A-Za-z]{0,}$"))
                {
                    validationResults.Add(new ValidationResult(errorMessage: "Uri value is not in specified format"));
                }
            }

            return validationResults;
        }
    }

    public class ValidateObjectBodyModel : IValidatableObject
    {
        public string Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (Value == null)
            {
                validationResults.Add(new ValidationResult(errorMessage: "Body value is required"));
            }
            else
            {
                if (!Regex.IsMatch(Value, "^[A-Za-z]{0,}$"))
                {
                    validationResults.Add(new ValidationResult(errorMessage: "Body value is not in specified format"));
                }
            }

            return validationResults;
        }
    }

    public class ValidateObjectModelValidator : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            var body = args.ApiContext.Request.InvocationContext.Models<ValidateObjectBodyModel>().FirstOrDefault();
            var uri = args.ApiContext.Request.InvocationContext.Models<ValidateObjectUriModel>().FirstOrDefault();

            if (body.Value != uri.Value)
            {
                return Task.FromResult(ApiValidationResult.Single("Body value and Uri value must be the same"));
            }

            return Task.FromResult(ApiValidationResult.Success());
        }
    }
}
