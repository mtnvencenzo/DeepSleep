namespace Api.DeepSleep.Controllers.Pipeline
{
    using global::DeepSleep;

    public class MultipleHttpMethodsController
    {
        [ApiRoute(new[] { "GET", "PUT", "POST" }, "pipeline/multiple/methods")]
        [ApiRouteRequestValidation(requireContentLengthOnRequestBodyRequests: false)]
        public MultiMethodsModel MultiMethods([BodyBound] MultiMethodsModel model)
        {
            return model ?? new MultiMethodsModel { Value = "Test" };
        }

        [ApiRoute(new[] { "GET", "PUT", "POST" }, "pipeline/multiple/methods/no/auto/head")]
        [ApiRouteEnableHead(enableHeadForGetRequests: false)]
        public MultiMethodsModel MultiMethodsNoAutoHead([BodyBound] MultiMethodsModel model)
        {
            return model ?? new MultiMethodsModel { Value = "Test" };
        }
    }

    public class MultiMethodsModel
    {
        public string Value { get; set; }
    }
}
