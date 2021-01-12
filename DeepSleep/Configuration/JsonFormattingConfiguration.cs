namespace DeepSleep.Configuration
{
    using System.Text.Json;

    /// <summary>
    /// 
    /// </summary>
    public class JsonFormattingConfiguration
    {
        /// <summary>Gets or sets a value indicating whether [disable formatter].</summary>
        /// <value><c>true</c> if [disable formatter]; otherwise, <c>false</c>.</value>
        public bool DisableFormatter { get; set; } = false;

        /// <summary>Gets or sets the read serializer options.</summary>
        /// <value>The read serializer options.</value>
        public virtual JsonSerializerOptions ReadSerializerOptions { get; set; }

        /// <summary>Gets or sets the write serializer options.</summary>
        /// <value>The write serializer options.</value>
        public virtual JsonSerializerOptions WriteSerializerOptions { get; set; }
    }
}
