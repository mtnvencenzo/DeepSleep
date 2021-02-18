namespace DeepSleep.Validation
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiValidationProvider
    {
        /// <summary>Gets the order.</summary>
        /// <value>The order.</value>
        int Order { get; }

        /// <summary>Validates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        Task Validate(IApiRequestContextResolver contextResolver);
    }
}
