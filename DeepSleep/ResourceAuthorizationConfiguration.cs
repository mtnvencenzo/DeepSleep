namespace DeepSleep
{
    using System.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("Policy = {Policy}")]
    public class ResourceAuthorizationConfiguration
    {
        /// <summary>Gets or sets the authentication value.</summary>
        /// <value>The authentication value.</value>
        public string Policy { get; set; }
    }
}
