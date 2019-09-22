using DeepSleep.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep.Example.Controllers
{
    public class BodyNotNullValidator : IApiValidator
    {
        /// <summary>Initializes a new instance of the <see cref="HelloWorldController"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public BodyNotNullValidator()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="HelloWorldController"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public BodyNotNullValidator(IApiRequestContextResolver contextResolver)
        {
            ContextResolver = contextResolver;
        }

        /// <summary>Gets or sets the context resolver.</summary>
        /// <value>The context resolver.</value>
        public IApiRequestContextResolver ContextResolver { get; set; }

        public Task<IEnumerable<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            var result = new List<ApiValidationResult>();
            var source = new TaskCompletionSource<IEnumerable<ApiValidationResult>>();

            if (args?.ApiContext?.RequestInfo?.InvocationContext?.BodyModel == null)
            {
                result.Add(new ApiValidationResult
                {
                    IsValid = false,
                    Message = new ApiResponseMessage
                    {
                        Code = "111",
                        Message = "Body is null"
                    }
                });
            }

            source.SetResult(result);
            return source.Task;
        }
    }
}
