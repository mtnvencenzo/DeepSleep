namespace DeepSleep.Configuration
{
    using DeepSleep.Validation;

    /// <summary>
    /// 
    /// </summary>
    public class DataAnnotationsValidationConfiguration : IDataAnnotationsValidationConfiguration
    {
        /// <summary>Gets or sets the continuation.</summary>
        /// <value>The continuation.</value>
        public ValidationContinuation Continuation { get; set; }

        /// <summary>Gets or sets a value indicating whether [validate all properties].</summary>
        /// <value><c>true</c> if [validate all properties]; otherwise, <c>false</c>.</value>
        public bool ValidateAllProperties { get; set; }
    }
}
