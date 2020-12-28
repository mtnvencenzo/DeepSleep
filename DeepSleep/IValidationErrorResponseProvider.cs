namespace DeepSleep
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IValidationErrorResponseProvider
    {
        /// <summary>Processes the specified errors.</summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        Task<object> Process(IList<string> errors);
    }
}
