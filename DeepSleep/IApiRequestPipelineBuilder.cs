namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiRequestPipelineBuilder
    {
        /// <summary>Builds this instance.</summary>
        /// <returns></returns>
        IApiRequestPipeline Build();
    }
}
