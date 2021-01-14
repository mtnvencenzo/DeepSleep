namespace DeepSleep.Validation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary></summary>
    public interface IEndpointValidator
    {
        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver);
    }
}