namespace DeepSleep.OpenApi
{
    using Microsoft.OpenApi.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IDeepSleepOasGenerator
    {
        /// <summary>Generates the specified information.</summary>
        /// <returns></returns>
        Task<OpenApiDocument> Generate();
    }
}
