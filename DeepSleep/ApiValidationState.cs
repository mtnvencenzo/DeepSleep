namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public enum ApiValidationState
    {
        /// <summary>The not attempted</summary>
        NotAttempted = 0,

        /// <summary>The succeeded</summary>
        Succeeded = 1,

        /// <summary>The failed</summary>
        Failed = 2,

        /// <summary>The validating</summary>
        Validating = 3,

        /// <summary>An exception was thrown during validation</summary>
        Exception = 4
    }
}
