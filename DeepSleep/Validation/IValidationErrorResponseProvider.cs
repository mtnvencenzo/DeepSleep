namespace DeepSleep.Validation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IValidationErrorResponseProvider
    {
        /// <summary>Processes the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        Task<object> Process(IApiRequestContextResolver contextResolver, IList<string> errors);
    }
}
