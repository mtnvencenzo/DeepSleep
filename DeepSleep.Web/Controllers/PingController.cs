namespace DeepSleep.Web.Controllers
{
    using DeepSleep.OpenApi.Decorators;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    internal class PingController
    {
        /// <summary>Pings this instance.</summary>
        /// <returns></returns>
        [OasSummary(summary: "Simple endpoint to test service up status.")]
        [OasDescription(description: "Returns a simple response to test server up status.  A valid 200 reponse indicates the service is up.")]
        [return: OasDescription(description: "The ping response")]
        internal Task<string> Ping()
        {
            return Task.FromResult("Pong");
        }
    }
}
