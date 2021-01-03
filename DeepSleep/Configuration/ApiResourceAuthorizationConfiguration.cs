namespace DeepSleep.Configuration
{
    using System.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("Policy = {Policy}")]
    public class ApiResourceAuthorizationConfiguration
    {
        /// <summary>Gets or sets the authentication value.</summary>
        /// <value>The authentication value.</value>
        public string Policy { get; set; }
    }
}
