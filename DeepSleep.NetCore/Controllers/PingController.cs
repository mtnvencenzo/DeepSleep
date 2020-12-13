namespace DeepSleep.NetCore.Controllers
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    internal class PingController
    {
        /// <summary>Pings this instance.</summary>
        /// <returns></returns>
        internal Task<string> Ping()
        {
            return Task.FromResult("Pong");
        }
    }
}
