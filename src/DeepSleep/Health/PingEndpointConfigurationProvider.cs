namespace DeepSleep.Health
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Health.IPingEndpointConfigurationProvider" />
    public class PingEndpointConfigurationProvider : IPingEndpointConfigurationProvider
    {
        /// <summary>Gets or sets the template.</summary>
        /// <value>The template.</value>
        public string Template { get; set; }
    }
}
