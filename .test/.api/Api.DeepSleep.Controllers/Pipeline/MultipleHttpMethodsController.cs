namespace Api.DeepSleep.Controllers.Pipeline
{
    using global::DeepSleep;

    /// <summary>
    /// 
    /// </summary>
    public class MultipleHttpMethodsController
    {
        /// <summary>Multis the methods.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "GET", "PUT", "POST" }, "pipeline/multiple/methods")]
        [ApiRouteRequestValidation(requireContentLengthOnRequestBodyRequests: false)]
        public MultiMethodsModel MultiMethods([InBody] MultiMethodsModel model)
        {
            return model ?? new MultiMethodsModel { Value = "Test" };
        }

        /// <summary>Multis the methods no automatic head.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "GET", "PUT", "POST" }, "pipeline/multiple/methods/no/auto/head")]
        [ApiRouteEnableHead(enableHeadForGetRequests: false)]
        public MultiMethodsModel MultiMethodsNoAutoHead([InBody] MultiMethodsModel model)
        {
            return model ?? new MultiMethodsModel { Value = "Test" };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MultiMethodsModel
    {
        /// <summary>Gets or sets the value.</summary>
        /// <value>The value.</value>
        public string Value { get; set; }
    }
}
