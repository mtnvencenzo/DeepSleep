namespace DeepSleep.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class EndpointValidatorComponent<T> : IEndpointValidatorComponent where T : IEndpointValidator
    {
        /// <summary>Initializes a new instance of the <see cref="EndpointValidatorComponent{T}"/> class.</summary>
        /// <param name="continuation">The continuation.</param>
        /// <param name="order">The order.</param>
        public EndpointValidatorComponent(ValidationContinuation continuation = ValidationContinuation.OnlyIfValid, int order = 0)
        {
            this.Order = order < 0
                ? 0
                : order;

            this.Continuation = continuation;
        }

        /// <summary>Gets the order.</summary>
        /// <value>The order.</value>
        public int Order { get; }

        /// <summary>Gets the continuation.</summary>
        /// <value>The continuation.</value>
        public ValidationContinuation Continuation { get; }

        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public async Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context != null)
            {
                IEndpointValidator validator = null;

                try
                {
                    if (context.RequestServices != null)
                    {
                        validator = context.RequestServices.GetService<T>();
                    }
                }
                catch { }

                if (validator == null)
                {
                    try
                    {
                        validator = Activator.CreateInstance<T>();
                    }
                    catch { }
                }

                if (validator != null)
                {
                    return await validator.Validate(contextResolver).ConfigureAwait(false);
                }
            }

            return null;
        }
    }
}
