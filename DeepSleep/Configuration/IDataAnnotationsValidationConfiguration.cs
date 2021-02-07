namespace DeepSleep.Configuration
{
    using DeepSleep.Validation;

    /// <summary>
    /// 
    /// </summary>
    public interface IDataAnnotationsValidationConfiguration
    {
        /// <summary>Gets or sets the continuation.</summary>
        /// <value>The continuation.</value>
        ValidationContinuation Continuation { get; set; }

        /// <summary>Gets or sets a value indicating whether [validate all properties].</summary>
        /// <value><c>true</c> if [validate all properties]; otherwise, <c>false</c>.</value>
        bool ValidateAllProperties { get; set; }
    }
}
