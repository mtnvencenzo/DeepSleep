namespace DeepSleep.Validation
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("Statue = {State}, Error Count = Errors?.Count")]
    public class ApiValidationInfo
    {
        /// <summary>Gets or sets the state.</summary>
        /// <value>The state.</value>
        public ApiValidationState State { get; set; } = ApiValidationState.NotAttempted;

        /// <summary>The suggested error status code</summary>
        public int? SuggestedErrorStatusCode { get; set; }

        /// <summary>Gets or sets the extended messages.</summary>
        /// <value>The extended messages.</value>
        public virtual IList<string> Errors { get; internal set; } = new List<string>();
    }
}
