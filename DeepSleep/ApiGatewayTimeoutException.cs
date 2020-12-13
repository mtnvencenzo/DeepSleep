namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ApiGatewayTimeoutException : ApiException
    {
        /// <summary>Gets the HTTP status.</summary>
        /// <value>The HTTP status.</value>
        public override int HttpStatus { get; } = 504;
    }
}
