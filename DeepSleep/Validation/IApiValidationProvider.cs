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

        /// <summary>Validates the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        Task Validate(ApiRequestContext context);
    }
}
