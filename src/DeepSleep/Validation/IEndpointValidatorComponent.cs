namespace DeepSleep.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEndpointValidatorComponent : IEndpointValidator
    {
        /// <summary>Gets the order.</summary>
        /// <value>The order.</value>
        int Order { get; }

        /// <summary>Gets the continuation.</summary>
        /// <value>The continuation.</value>
        ValidationContinuation Continuation { get; }
    }
}
