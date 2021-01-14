namespace DeepSleep.Validation.DataAnnotations
{
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Validation.IApiValidationProvider" />
    public class DataAnnotationsValidationProvider : IApiValidationProvider
    {
        /// <summary>Initializes a new instance of the <see cref="DataAnnotationsValidationProvider"/> class.</summary>
        /// <param name="continuation">The continuation.</param>
        /// <param name="validateAllProperties">if set to <c>true</c> [validate all properties].</param>
        public DataAnnotationsValidationProvider(ValidationContinuation continuation = ValidationContinuation.OnlyIfValid, bool validateAllProperties = true)
        {
            this.Continuation = continuation;
            this.ValidateAllProperties = validateAllProperties;
        }

        /// <summary>Gets the order.</summary>
        /// <value>The order.</value>
        public int Order => 0;

        /// <summary>Gets the continuation.</summary>
        /// <value>The continuation.</value>
        public ValidationContinuation Continuation { get; }

        /// <summary>Gets a value indicating whether [validate all properties].</summary>
        /// <value><c>true</c> if [validate all properties]; otherwise, <c>false</c>.</value>
        public bool ValidateAllProperties { get; }

        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task Validate(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            var uriModel = context?.Request?.InvocationContext?.UriModel;
            var bodyModel = context?.Request?.InvocationContext?.BodyModel;

            if (uriModel != null)
            {
                // Skip validators that are not set to run if validation != failed
                if (context.Validation.State == ApiValidationState.Failed && this.Continuation == ValidationContinuation.OnlyIfValid)
                {
                }
                else
                {

                    var validationContext = new ValidationContext(
                       uriModel,
                       serviceProvider: context.RequestServices,
                       items: null);

                    var results = new List<ValidationResult>();

                    var isValid = Validator.TryValidateObject(
                        instance: uriModel,
                        validationContext: validationContext,
                        validationResults: results,
                        validateAllProperties: this.ValidateAllProperties);

                    if (!isValid)
                    {
                        context.Validation.State = ApiValidationState.Failed;
                        context.Validation.SuggestedErrorStatusCode = context.Configuration?.ValidationErrorConfiguration?.UriBindingErrorStatusCode;

                        foreach (var result in results)
                        {
                            context.AddValidationError(result.ErrorMessage);
                        }
                    }
                }
            }

            if (bodyModel != null)
            {
                // Skip validators that are not set to run if validation != failed
                if (context.Validation.State == ApiValidationState.Failed && this.Continuation == ValidationContinuation.OnlyIfValid)
                {
                }
                else
                {

                    var validationContext = new ValidationContext(
                       bodyModel,
                       serviceProvider: context.RequestServices,
                       items: null);

                    var results = new List<ValidationResult>();

                    var isValid = Validator.TryValidateObject(
                        instance: bodyModel,
                        validationContext: validationContext,
                        validationResults: results,
                        validateAllProperties: this.ValidateAllProperties);

                    if (!isValid)
                    {
                        context.Validation.State = ApiValidationState.Failed;
                        context.Validation.SuggestedErrorStatusCode = context.Configuration?.ValidationErrorConfiguration?.BodyValidationErrorStatusCode;

                        foreach (var result in results)
                        {
                            context.AddValidationError(result.ErrorMessage);
                        }
                    }
                }
            }


            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class DataAnnotationsValidationProviderExtensionMethods
    {
        /// <summary>Uses the data annotation validations.</summary>
        /// <param name="services">The services.</param>
        /// <param name="continuation">The continuation.</param>
        /// <param name="validateAllProperties">if set to <c>true</c> [validate all properties].</param>
        /// <returns></returns>
        public static IServiceCollection UseDataAnnotationValidations(
            this IServiceCollection services,
            ValidationContinuation continuation = ValidationContinuation.OnlyIfValid,
            bool validateAllProperties = true)
        {
            services
                .AddScoped<IApiValidationProvider, DataAnnotationsValidationProvider>((p) =>
                {
                    return new DataAnnotationsValidationProvider(continuation: continuation, validateAllProperties: validateAllProperties);
                });

            return services;
        }
    }
}