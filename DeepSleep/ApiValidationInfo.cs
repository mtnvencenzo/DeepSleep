namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiValidationInfo
    {
        /// <summary>Gets or sets the state.</summary>
        /// <value>The state.</value>
        public ApiValidationState State { get; set; } = ApiValidationState.NotAttempted;

        /// <summary>The suggested error status code</summary>
        public int SuggestedErrorStatusCode = 400;
    }
}
