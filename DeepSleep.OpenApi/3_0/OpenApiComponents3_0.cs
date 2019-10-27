namespace DeepSleep.OpenApi.v3_0
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiComponents3_0
    {
        /// <summary>
        /// Gets or sets the schemas.
        /// </summary>
        /// <value>
        /// The schemas.
        /// </value>
        public Dictionary<string, OpenApiSchema3_0> schemas { get; set; } = new Dictionary<string, OpenApiSchema3_0>();

        /// <summary>
        /// Gets or sets the responses.
        /// </summary>
        /// <value>
        /// The responses.
        /// </value>
        public Dictionary<string, OpenApiResponse3_0> responses { get; set; } = new Dictionary<string, OpenApiResponse3_0>();

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public Dictionary<string, OpenApiInlineOrReferenceParameter3_0> parameters { get; set; } = new Dictionary<string, OpenApiInlineOrReferenceParameter3_0>();

        /// <summary>
        /// Gets or sets the examples.
        /// </summary>
        /// <value>
        /// The examples.
        /// </value>
        public Dictionary<string, object> examples { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the request bodies.
        /// </summary>
        /// <value>
        /// The request bodies.
        /// </value>
        public Dictionary<string, OpenApiInlineOrReferenceRequestBody3_0> requestBodies { get; set; } = new Dictionary<string, OpenApiInlineOrReferenceRequestBody3_0>();

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public Dictionary<string, OpenApiInlineOrReferenceHeader3_0> headers { get; set; } = new Dictionary<string, OpenApiInlineOrReferenceHeader3_0>();

        /// <summary>
        /// Gets or sets the security schemes.
        /// </summary>
        /// <value>
        /// The security schemes.
        /// </value>
        public Dictionary<string, object> securitySchemes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public Dictionary<string, OpenApiInlineOrReferenceLink3_0> links { get; set; } = new Dictionary<string, OpenApiInlineOrReferenceLink3_0>();

        /// <summary>
        /// Gets or sets the callbacks.
        /// </summary>
        /// <value>
        /// The callbacks.
        /// </value>
        public Dictionary<string, OpenApiInlineOrReferenceCallback3_0> callbacks { get; set; } = new Dictionary<string, OpenApiInlineOrReferenceCallback3_0>();
    }
}
