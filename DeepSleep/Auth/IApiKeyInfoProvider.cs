using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep.Auth
{
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
