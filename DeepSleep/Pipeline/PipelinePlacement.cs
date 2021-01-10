namespace DeepSleep.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    public enum PipelinePlacement
    {
        /// <summary>The before endpoint validation</summary>
        BeforeEndpointValidation = 1,

        /// <summary>The before endpoint invocation</summary>
        BeforeEndpointInvocation = 2,

        /// <summary>The after endpoint invocation</summary>
        AfterEndpointInvocation = 3
    }
}
