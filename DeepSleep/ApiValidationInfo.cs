namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class ApiValidationInfo
    {
        /// <summary>Gets or sets the state.</summary>
        /// <value>The state.</value>
        public ApiValidationState State { get; set; } = ApiValidationState.NotAttempted;

        /// <summary>The suggested error status code</summary>
        public int SuggestedErrorStatusCode { get; set; } = 400;

        /// <summary>Gets or sets the extended messages.</summary>
        /// <value>The extended messages.</value>
        public virtual IList<string> Errors { get; set; } = new List<string>();
    }
}
