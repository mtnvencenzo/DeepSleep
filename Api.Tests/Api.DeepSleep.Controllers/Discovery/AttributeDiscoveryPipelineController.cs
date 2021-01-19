namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Pipeline;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryPipelineController
    {
        /// <summary>Gets the pipeline before validation.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/pipeline/multi")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent), PipelinePlacement.BeforeEndpointInvocation, 1)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent), PipelinePlacement.BeforeEndpointInvocation, 0)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent), PipelinePlacement.AfterEndpointInvocation, 1)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent), PipelinePlacement.AfterEndpointInvocation, 0)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent), PipelinePlacement.BeforeEndpointValidation, 1)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent), PipelinePlacement.BeforeEndpointValidation, 0)]
        public AttributeDiscoveryModel GetPipelineBeforeValidation()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
