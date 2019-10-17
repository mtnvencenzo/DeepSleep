namespace DeepSleep.Validation
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class ApiValidationArgs
    {
        /// <summary>Gets or sets the API context.</summary>
        /// <value>The API context.</value>
        public ApiRequestContext ApiContext { get; set; }

        /// <summary>Gets or sets the state of the valiation.</summary>
        /// <value>The state of the valiation.</value>
        public ApiValidationState ValiationState { get; set; }
    }
}
