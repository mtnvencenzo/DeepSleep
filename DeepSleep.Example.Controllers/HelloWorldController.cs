using DeepSleep.Example.Controllers.Models;
using DeepSleep.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DeepSleep.Example.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    public class HelloWorldController
    {
        /// <summary>Initializes a new instance of the <see cref="HelloWorldController"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public HelloWorldController(IApiRequestContextResolver contextResolver)
        {
            ContextResolver = contextResolver;
        }

        /// <summary>Gets or sets the context resolver.</summary>
        /// <value>The context resolver.</value>
        public IApiRequestContextResolver ContextResolver { get; set; }

        /// <summary>Gets the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [TypeBasedValidator(typeof(TestValidator))]
        public Task<ApiResponse> Get(HelloWorldGetUriRq request)
        {
            var source = new TaskCompletionSource<ApiResponse>();
            source.SetResult(new ApiResponse
            {
                StatusCode = 200,
                Body = new HelloWorldRs
                {
                    Message = "Hello World"
                }
            });
            return source.Task;
        }

        /// <summary>Creates something.</summary>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        [ApiRoute("api/helloworld", "POST")]
        public Task<ApiResponse> CreateSomething(HelloWorldPostBodyRq body)
        {
            var source = new TaskCompletionSource<ApiResponse>();
            source.SetResult(new ApiResponse
            {
                StatusCode = 200,
                Body = new HelloWorldRs
                {
                    Message = body?.Message
                }
            });
            return source.Task;
        }

        /// <summary>Posts the specified URI.</summary>
        /// <param name="uri">The URI.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        [TypeBasedValidator(typeof(BodyNotNullValidator))]
        public Task<ApiResponse> Post(HelloWorldPostUriRq uri, HelloWorldPostBodyRq body)
        {
            ContextResolver.GetContext().ProcessingInfo.ExtendedMessages.Add(new ApiResponseMessage
            {
                Code = "100.0001",
                Message = "It Works!"
            });

            var source = new TaskCompletionSource<ApiResponse>();
            source.SetResult(new ApiResponse
            {
                StatusCode = 201,
                Body = new HelloWorldRs
                {
                    Message = $"POST-{body?.Message ?? string.Empty}-{uri?.From ?? string.Empty}"
                }
            });
            return source.Task;
        }

        /// <summary>Puts something.</summary>
        /// <param name="uri">The URI.</param>
        /// <param name="body">The body.</param>
        /// <returns></returns>
        public Task<ApiResponse> PutSomething(HelloWorldPutUriRq uri, HelloWorldPutBodyRq body)
        {
            var source = new TaskCompletionSource<ApiResponse>();
            source.SetResult(new ApiResponse
            {
                StatusCode = 200,
                Body = new HelloWorldRs
                {
                    Message = $"PUT-{body?.Message ?? string.Empty}-{uri?.From ?? string.Empty}"
                }
            });
            return source.Task;
        }

        /// <summary>Deletes this instance.</summary>
        /// <returns></returns>
        public Task<ApiResponse> Delete()
        {
            var source = new TaskCompletionSource<ApiResponse>();
            source.SetResult(new ApiResponse
            {
                StatusCode = 204
            });
            return source.Task;
        }
    }
}
