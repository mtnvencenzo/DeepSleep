namespace DeepSleep.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public enum AuthSchemeType
    {
        /// <summary>Unspecified or unsuppported
        /// </summary>
        None = 0,

        /// <summary>Represents a public/private key security scheme where both the client and server have
        /// knowledge of the private key for use in a SHA256 HMAC hash signature
        /// </summary>
        Shared = 1,

        /// <summary>Represents a dynamically generated login token based authentication type for use in a SHA256 HMAC hash signature
        /// </summary>
        Token = 2
    }
}
