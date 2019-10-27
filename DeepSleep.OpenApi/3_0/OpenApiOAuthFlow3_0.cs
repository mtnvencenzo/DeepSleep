namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiOAuthFlow3_0
    {
        /// <summary>Gets or sets the authorization URL.</summary>
        /// <value>The authorization URL.</value>
        public string authorizationUrl { get; set; }

        /// <summary>Gets or sets the token URL.</summary>
        /// <value>The token URL.</value>
        public string tokenUrl { get; set; }

        /// <summary>Gets or sets the refresh URL.</summary>
        /// <value>The refresh URL.</value>
        public string refreshUrl { get; set; }

        /// <summary>Gets or sets the scopes.</summary>
        /// <value>The scopes.</value>
        public Dictionary<string, string> scopes { get; set; } = new Dictionary<string, string>();

        /// <summary>The extensions</summary>
        [JsonExtensionData]
        public Dictionary<string, object> extensions = new Dictionary<string, object>();
    }
}
