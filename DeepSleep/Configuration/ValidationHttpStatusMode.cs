namespace DeepSleep.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public enum ValidationHttpStatusMode
    {
        /// <summary>The common HTTP specification</summary>
        CommonHttpSpecification = 1,

        /// <summary>The strict HTTP specification</summary>
        StrictHttpSpecification = 2,

        /// <summary>The common HTTP specification with custom deserialization status</summary>
        CommonHttpSpecificationWithCustomDeserializationStatus = 3
    }
}
