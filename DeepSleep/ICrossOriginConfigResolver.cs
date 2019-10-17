namespace DeepSleep
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface ICrossOriginConfigResolver
    {
        /// <summary>Resolves the configuration.</summary>
        /// <returns></returns>
        Task<CrossOriginConfiguration> ResolveConfig();
    }
}
