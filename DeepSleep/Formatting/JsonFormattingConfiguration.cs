namespace DeepSleep.Formatting
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonFormattingConfiguration : IJsonFormattingConfiguration
    {
        /// <summary>The casing style to use when writting json
        /// </summary>
        public virtual FormatCasingStyle CasingStyle { get; set; }

        /// <summary>Whether or not null values should be ecluded from written json
        /// </summary>
        public virtual bool NullValuesExcluded { get; set; } = true; 
    }
}
