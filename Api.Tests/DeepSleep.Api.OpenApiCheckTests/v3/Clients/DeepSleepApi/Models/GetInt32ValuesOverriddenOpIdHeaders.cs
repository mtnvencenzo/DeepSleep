// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace DeepSleep.Api.OpenApiCheckTests.v3.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Defines headers for GetInt32ValuesOverriddenOpId operation.
    /// </summary>
    public partial class GetInt32ValuesOverriddenOpIdHeaders
    {
        /// <summary>
        /// Initializes a new instance of the
        /// GetInt32ValuesOverriddenOpIdHeaders class.
        /// </summary>
        public GetInt32ValuesOverriddenOpIdHeaders()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// GetInt32ValuesOverriddenOpIdHeaders class.
        /// </summary>
        public GetInt32ValuesOverriddenOpIdHeaders(string xRequestId = default(string), string xCorrelationId = default(string))
        {
            XRequestId = xRequestId;
            XCorrelationId = xCorrelationId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "X-RequestId")]
        public string XRequestId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "X-CorrelationId")]
        public string XCorrelationId { get; set; }

    }
}
