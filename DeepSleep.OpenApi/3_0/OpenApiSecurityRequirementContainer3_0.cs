namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiSecurityRequirementContainer3_0
    {
        /// <summary>Gets or sets the security.</summary>
        /// <value>The security.</value>
        [JsonExtensionData]
        public Dictionary<string, object> security { get; set; } = new Dictionary<string, object>();
    }
}