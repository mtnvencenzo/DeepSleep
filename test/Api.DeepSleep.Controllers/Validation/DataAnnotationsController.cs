namespace Api.DeepSleep.Controllers.Validation
{
    using global::DeepSleep;
    using global::DeepSleep.Validation;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class DataAnnotationsController
    {
        /// <summary>Posts the mixed endpoint and data annotations.</summary>
        /// <param name="uri">The URI.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "validators/dataannotations/uri/and/body/{value}")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        [ApiEndpointValidation(validatorType: typeof(DataAnnotationsModelValidator), continuation: ValidationContinuation.OnlyIfValid)]
        public IApiResponse PostMixedEndpointAndDataAnnotations([InUri] DataAnnotationsUriModel uri, [InBody] DataAnnotationsBodyModel body)
        {
            return ApiResponse.NoContent();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataAnnotationsUriModel
    {
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        [Required(AllowEmptyStrings = true, ErrorMessage = "Uri value is required")]
        [RegularExpression("^[A-Za-z]{0,}$", ErrorMessage = "Uri value is not in specified format")]
        public string Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DataAnnotationsBodyModel
    {
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        [Required(AllowEmptyStrings = true, ErrorMessage = "Body value is required")]
        [RegularExpression("^[A-Za-z]{0,}$", ErrorMessage = "Body value is not in specified format")]
        public string Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Validation.IEndpointValidator" />
    public class DataAnnotationsModelValidator : IEndpointValidator
    {
        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();
            var body = context.Request.InvocationContext.Models<DataAnnotationsBodyModel>().FirstOrDefault();
            var uri = context.Request.InvocationContext.Models<DataAnnotationsUriModel>().FirstOrDefault();

            if (body.Value != uri.Value)
            {
                return Task.FromResult(ApiValidationResult.Single("Body value and Uri value must be the same"));
            }

            return Task.FromResult(ApiValidationResult.Success());
        }
    }
}
