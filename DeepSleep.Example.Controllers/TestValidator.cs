using DeepSleep.Example.Controllers.Resources;
using DeepSleep.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeepSleep.Example.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Validation.IApiValidator" />
    public class TestValidator : IApiValidator
    {
        /// <summary>Initializes a new instance of the <see cref="TestValidator"/> class.</summary>
        public TestValidator()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="TestValidator"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public TestValidator(IApiRequestContextResolver contextResolver)
        {
            ContextResolver = contextResolver;
        }

        /// <summary>Gets or sets the context resolver.</summary>
        /// <value>The context resolver.</value>
        public IApiRequestContextResolver ContextResolver { get; set; }

        /// <summary>Validates the specified arguments.</summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public Task<IEnumerable<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            var result = new List<ApiValidationResult>();
            var source = new TaskCompletionSource<IEnumerable<ApiValidationResult>>();

            var message = ContextResolver.GetContext().GetAcceptLanguageResource(() => Messages.Error1);

            result.Add(new ApiValidationResult
            {
                IsValid = false,
                Message = new ApiResponseMessage
                {
                    Code = "222",
                    Message = message
                }
            });

            source.SetResult(result);
            return source.Task;
        }
    }
}
