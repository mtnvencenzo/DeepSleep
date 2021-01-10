namespace DeepSleep.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="DeepSleep.Validation.IEndpointValidatorComponent" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ApiEndpointValidationAttribute : Attribute, IEndpointValidatorComponent
    {
        /// <summary>Initializes a new instance of the <see cref="ApiEndpointValidationAttribute"/> class.</summary>
        /// <param name="validatorType">Type of the validator.</param>
        /// <param name="continuation">The continuation.</param>
        /// <param name="order">The order.</param>
        /// <exception cref="ArgumentNullException">validatorType</exception>
        /// <exception cref="ArgumentException"></exception>
        public ApiEndpointValidationAttribute(
            Type validatorType, 
            ValidationContinuation continuation = ValidationContinuation.OnlyIfValid, 
            int order = 0)
        {
            if (validatorType == null)
            {
                throw new ArgumentNullException(nameof(validatorType));
            }

            if (validatorType.GetInterface(nameof(IEndpointValidator), false) == null)
            {
                throw new ArgumentException($"{nameof(validatorType)} must implement interface {typeof(IEndpointValidator).AssemblyQualifiedName}");
            }

            this.ValidatorType = validatorType;
            this.Continuation = continuation;

            this.Order = order < 0
                ? 0
                : order;
        }

        /// <summary>Gets the type of the validator.</summary>
        /// <value>The type of the validator.</value>
        public Type ValidatorType { get; }

        /// <summary>Gets the order.</summary>
        /// <value>The order.</value>
        public int Order { get; }

        /// <summary>Gets the continuation.</summary>
        /// <value>The continuation.</value>
        public ValidationContinuation Continuation { get; }

        /// <summary>Validates the specified arguments.</summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public async Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            var context = args.ApiContext;

            if (context != null)
            {
                IEndpointValidator validator = null;

                try
                {
                    if (context.RequestServices != null)
                    {
                        validator = context.RequestServices.GetService(Type.GetType(ValidatorType.AssemblyQualifiedName)) as IEndpointValidator;
                    }
                }
                catch { }

                if (validator == null)
                {
                    try
                    {
                        validator = Activator.CreateInstance(Type.GetType(ValidatorType.AssemblyQualifiedName)) as IEndpointValidator;
                    }
                    catch { }
                }

                if (validator != null)
                {
                    return await validator.Validate(args).ConfigureAwait(false);
                }
            }

            return null;
        }
    }
}
