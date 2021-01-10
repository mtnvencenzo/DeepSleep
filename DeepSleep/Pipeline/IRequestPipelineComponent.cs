namespace DeepSleep.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Pipeline.IPipelineComponent" />
    public interface IRequestPipelineComponent : IPipelineComponent
    {
        /// <summary>Gets the placement.</summary>
        /// <value>The placement.</value>
        PipelinePlacement Placement { get; }

        /// <summary>Gets the order.</summary>
        /// <value>The order.</value>
        int Order { get; }
    }
}
