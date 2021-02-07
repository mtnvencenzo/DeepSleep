namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.ApiRouteErrorResponseProviderAttribute" />
    public class CustomApiRouteErrorResponseProviderAttribute : ApiRouteErrorResponseProviderAttribute
    {
        /// <summary>Processes the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public override Task<object> Process(IApiRequestContextResolver contextResolver, IList<string> errors)
        {
            return Task.FromResult(new CustomResponseErrorObject(errors) as object);
        }

        /// <summary>Gets the type of the error.</summary>
        /// <returns></returns>
        public override Type GetErrorType() => typeof(CustomResponseErrorObject);
    }
}
