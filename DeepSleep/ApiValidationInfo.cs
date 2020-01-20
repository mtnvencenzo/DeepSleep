namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiValidationInfo
    {
        /// <summary>Initializes a new instance of the <see cref="ApiValidationInfo"/> class.</summary>
        public ApiValidationInfo()
        {
            State = ApiValidationState.NotAttempted;
        }

        /// <summary>Gets or sets the state.</summary>
        /// <value>The state.</value>
        public ApiValidationState State { get; set; }
    }
}
