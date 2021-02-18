namespace Api.DeepSleep.Controllers.Validation
{
    using global::DeepSleep;
    using global::DeepSleep.Validation;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class DataAnnotationsValidateObjectController
    {
        /// <summary>Posts the mixed endpoint object validations.</summary>
        /// <param name="uri">The URI.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "validators/validateobject/uri/and/body/{value}")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        [ApiEndpointValidation(validatorType: typeof(ValidateObjectModelValidator), continuation: ValidationContinuation.OnlyIfValid)]
        public IApiResponse PostMixedEndpointObjectValidations([InUri] ValidateObjectUriModel uri, [InBody] ValidateObjectBodyModel body)
        {
            return ApiResponse.NoContent();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.IValidatableObject" />
    public class ValidateObjectUriModel : IValidatableObject
    {
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>Determines whether the specified object is valid.</summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A collection that holds failed-validation information.</returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.IValidatableObject" />
    public class ValidateObjectBodyModel : IValidatableObject
    {
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>Determines whether the specified object is valid.</summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A collection that holds failed-validation information.</returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Validation.IEndpointValidator" />
    public class ValidateObjectModelValidator : IEndpointValidator
    {
        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();
            var body = context.Request.InvocationContext.Models<ValidateObjectBodyModel>().FirstOrDefault();
            var uri = context.Request.InvocationContext.Models<ValidateObjectUriModel>().FirstOrDefault();

            if (body.Value != uri.Value)
            {
                return Task.FromResult(ApiValidationResult.Single("Body value and Uri value must be the same"));
            }

            return Task.FromResult(ApiValidationResult.Success());
        }
    }
}
