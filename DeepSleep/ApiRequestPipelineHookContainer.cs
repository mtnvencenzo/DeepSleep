namespace DeepSleep
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestPipelineHookContainer
    {
        /// <summary>Gets or sets the pipeline types.</summary>
        /// <value>The pipeline types.</value>
        public ApiRequestPipelineComponentTypes PipelineTypes { get; set; }

        /// <summary>Gets or sets the placements.</summary>
        /// <value>The placements.</value>
        public ApiRequestPipelineHookPlacements Placements { get; set; }

        /// <summary>Gets or sets the hook.</summary>
        /// <value>The hook.</value>
        public Func<ApiRequestContext, ApiRequestPipelineComponentTypes, ApiRequestPipelineHookPlacements, Task<ApiRequestPipelineHookResult>> Hook { get; set; }
    }
}
