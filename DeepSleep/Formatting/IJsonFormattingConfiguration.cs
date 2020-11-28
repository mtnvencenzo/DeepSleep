namespace DeepSleep.Formatting
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJsonFormattingConfiguration
    {
        /// <summary>The casing style to use when writting json
        /// </summary>
        FormatCasingStyle CasingStyle { get; set; }

        /// <summary>Whether or not null values should be ecluded from written json
        /// </summary>
        bool NullValuesExcluded { get; set; }
    }
}
