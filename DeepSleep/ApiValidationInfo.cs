namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiValidationInfo
    {
        #region Constructors & Initialization

        /// <summary>Initializes a new instance of the <see cref="ApiValidationInfo"/> class.</summary>
        public ApiValidationInfo()
        {
            State = ApiValidationState.NotAttempted;
        }

        #endregion

        /// <summary>Gets or sets the state.</summary>
        /// <value>The state.</value>
        public ApiValidationState State { get; set; }
    }
}
