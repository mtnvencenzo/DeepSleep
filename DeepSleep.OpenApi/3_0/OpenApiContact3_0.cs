﻿namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiContact3_0
    {
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string name { get; set; }

        /// <summary>Gets or sets the URL.</summary>
        /// <value>The URL.</value>
        public string url { get; set; }

        /// <summary>Gets or sets the email.</summary>
        /// <value>The email.</value>
        public string email { get; set; }

        /// <summary>The extensions</summary>
        [JsonExtensionData]
        public Dictionary<string, object> extensions = new Dictionary<string, object>();
    }
}
