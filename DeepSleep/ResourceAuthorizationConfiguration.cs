namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class ResourceAuthorizationConfiguration
    {
        /// <summary>Gets or sets the authentication value.</summary>
        /// <value>The authentication value.</value>
        public string Policy { get; set; }

        /// <summary>Gets or sets the authentication scheme.</summary>
        /// <value>The authentication scheme.</value>
        public IList<string> Roles { get; set; }
    }
}
