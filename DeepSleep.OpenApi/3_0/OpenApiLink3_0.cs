namespace DeepSleep.OpenApi.v3_0
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiLink3_0
    {
        /// <summary>Gets or sets the operation reference.</summary>
        /// <value>The operation reference.</value>
        public string operationRef { get; set; }

        /// <summary>Gets or sets the operation identifier.</summary>
        /// <value>The operation identifier.</value>
        public string operationId { get; set; }

        /// <summary>Gets or sets the parameters.</summary>
        /// <value>The parameters.</value>
        public object parameters { get; set; }

        /// <summary>Gets or sets the request body.</summary>
        /// <value>The request body.</value>
        public object requestBody { get; set; }

        /// <summary>Gets or sets the description.</summary>
        /// <value>The description.</value>
        public string description { get; set; }

        /// <summary>Gets or sets the server.</summary>
        /// <value>The server.</value>
        public OpenApiServer3_0 server { get; set; }

        /// <summary>The extensions</summary>
        [JsonExtensionData]
        public Dictionary<string, object> extensions = new Dictionary<string, object>();
    }
}
