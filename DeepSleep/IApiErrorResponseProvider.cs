namespace DeepSleep
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiErrorResponseProvider
    {
        /// <summary>Processes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        Task Process(ApiRequestContext context);
    }
}
