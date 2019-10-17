namespace DeepSleep.Auth
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiKeyInfoProvider
    {
        /// <summary>Gets the key information.</summary>
        /// <param name="publicKey">The public key.</param>
        /// <returns></returns>
        Task<ApiKeyInfo> GetKeyInfo(string publicKey);
    }
}
