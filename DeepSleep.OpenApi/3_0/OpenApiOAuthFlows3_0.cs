namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiOAuthFlows3_0
    {
        /// <summary>Gets or sets the implicit.</summary>
        /// <value>The implicit.</value>
        public OpenApiOAuthFlow3_0 @implicit { get; set; }

        /// <summary>Gets or sets the password.</summary>
        /// <value>The password.</value>
        public OpenApiOAuthFlow3_0 password { get; set; }

        /// <summary>Gets or sets the client credentials.</summary>
        /// <value>The client credentials.</value>
        public OpenApiOAuthFlow3_0 clientCredentials { get; set; }

        /// <summary>Gets or sets the authorization code.</summary>
        /// <value>The authorization code.</value>
        public OpenApiOAuthFlow3_0 authorizationCode { get; set; }

        /// <summary>The extensions</summary>
        [JsonExtensionData]
        public Dictionary<string, object> extensions = new Dictionary<string, object>();
    }
}
