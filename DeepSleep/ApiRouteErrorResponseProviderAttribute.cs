namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public abstract class ApiRouteErrorResponseProviderAttribute : Attribute, IValidationErrorResponseProvider
    {
        /// <summary>Processes the specified errors.</summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public abstract Task<object> Process(IList<string> errors);
    }
}
