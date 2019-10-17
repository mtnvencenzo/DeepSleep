namespace DeepSleep
{
    /// <summary></summary>
    public class ClientAuthentication
    {
        /// <summary>Gets or sets the authentication value.</summary>
        /// <value>The authentication value.</value>
        public string AuthValue { get; set; }

        /// <summary>Gets or sets the signature placement.</summary>
        /// <value>The signature placement.</value>
        public RequestMetaDataPlacement AuthValuePlacement { get; set; }

        /// <summary>Gets or sets the authentication scheme.</summary>
        /// <value>The authentication scheme.</value>
        public string AuthScheme { get; set; }
    }
}