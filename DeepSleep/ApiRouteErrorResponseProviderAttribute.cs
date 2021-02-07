namespace DeepSleep
{
    using DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public abstract class ApiRouteErrorResponseProviderAttribute : Attribute, IValidationErrorResponseProvider
    {
        /// <summary>Processes the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public abstract Task<object> Process(IApiRequestContextResolver contextResolver, IList<string> errors);

        /// <summary>Gets the type of the error.</summary>
        /// <returns></returns>
        public abstract Type GetErrorType();
    }
}
