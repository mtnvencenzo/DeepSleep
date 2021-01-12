namespace Api.DeepSleep.Controllers.Validation
{
    using global::DeepSleep;
    using global::DeepSleep.Validation;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataAnnotationsController
    {
        [ApiRoute(new[] { "POST" }, "validators/dataannotations/uri/and/body/{value}")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        [ApiEndpointValidation(validatorType: typeof(DataAnnotationsModelValidator), continuation: ValidationContinuation.OnlyIfValid)]
        public IApiResponse PostMixedEndpointAndDataAnnotations([UriBound] DataAnnotationsUriModel uri, [BodyBound] DataAnnotationsBodyModel body)
        {
            return ApiResponse.NoContent();
        }
    }

    public class DataAnnotationsUriModel
    {
        [Required(AllowEmptyStrings = true, ErrorMessage = "Uri value is required")]
        [RegularExpression("^[A-Za-z]{0,}$", ErrorMessage = "Uri value is not in specified format")]
        public string Value { get; set; }
    }

    public class DataAnnotationsBodyModel
    {
        [Required(AllowEmptyStrings = true, ErrorMessage = "Body value is required")]
        [RegularExpression("^[A-Za-z]{0,}$", ErrorMessage = "Body value is not in specified format")]
        public string Value { get; set; }
    }

    public class DataAnnotationsModelValidator : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            var body = args.ApiContext.Request.InvocationContext.Models<DataAnnotationsBodyModel>().FirstOrDefault();
            var uri = args.ApiContext.Request.InvocationContext.Models<DataAnnotationsUriModel>().FirstOrDefault();

            if (body.Value != uri.Value)
            {
                return Task.FromResult(ApiValidationResult.Single("Body value and Uri value must be the same"));
            }

            return Task.FromResult(ApiValidationResult.Success());
        }
    }
}
