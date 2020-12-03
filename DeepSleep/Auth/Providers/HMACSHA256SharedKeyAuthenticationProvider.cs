namespace DeepSleep.Auth.Providers
{
    using System;
    using System.Threading.Tasks;
    using System.Text;
    using System.Security.Cryptography;
    using System.Net;
    using System.Globalization;

    /// <summary></summary>
    public class HMACSHA256SharedKeyAuthenticationProvider : IAuthenticationProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HMACSHA256SharedKeyAuthenticationProvider"/> class.
        /// </summary>
        /// <param name="realm">The realm.</param>
        public HMACSHA256SharedKeyAuthenticationProvider(string realm)
        {
            Realm = realm;
        }

        /// <summary>Authenticates the specified identity.</summary>
        /// <param name="context">The context.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public virtual async Task Authenticate(ApiRequestContext context, IApiResponseMessageConverter responseMessageConverter)
        {
            if (context.RequestInfo.ClientAuthenticationInfo == null)
            {
                context.RequestInfo.ClientAuthenticationInfo = new ClientAuthentication();
            }

            var authValue = context.RequestInfo.ClientAuthenticationInfo.AuthValue ?? string.Empty;

            if (string.IsNullOrWhiteSpace(authValue))
            {
                context.RequestInfo.ClientAuthenticationInfo.AuthResult = new AuthenticationResult(false, AuthenticationErrors.EmptyAuthField, responseMessageConverter);
                return;
            }

            var authPairs = authValue.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (authPairs.Length != 2)
            {
                context.RequestInfo.ClientAuthenticationInfo.AuthResult = new AuthenticationResult(false, AuthenticationErrors.InvalidAuthValuePairing, responseMessageConverter);
                return;
            }

            var publicKey = authPairs[0];
            if (string.IsNullOrWhiteSpace(publicKey))
            {
                context.RequestInfo.ClientAuthenticationInfo.AuthResult = new AuthenticationResult(false, AuthenticationErrors.InvalidPublicKey, responseMessageConverter);
                return;
            }

            var signature = authPairs[1];
            if (string.IsNullOrWhiteSpace(signature))
            {
                context.RequestInfo.ClientAuthenticationInfo.AuthResult = new AuthenticationResult(false, AuthenticationErrors.EmptySignature, responseMessageConverter);
                return;
            }

            var privateKey = (await KeyInfoProvider.GetKeyInfo(publicKey).ConfigureAwait(false))?.PrivateKey ?? string.Empty;
            if (string.IsNullOrWhiteSpace(privateKey))
            {
                context.RequestInfo.ClientAuthenticationInfo.AuthResult = new AuthenticationResult(false, AuthenticationErrors.InvalidPrivateKey, responseMessageConverter);
                return;
            }

            // GENERATE THE SIGNAURE BASED ON PROPERTIES OF THE REQUEST.  IF THIS MATCHES 
            // THE SIGNAUTE THAT THE CLIENT SUPPLIED THEN AUTHENTICATION IS SUCCESSFUL
            var builtSignature = BuildSignature(
                context.RequestInfo.RequestUri,
                context.RequestInfo.Method,
                publicKey,
                privateKey,
                context.RequestInfo.RequestDate);

            if (CompareSignatures(builtSignature, signature))
            {
                context.RequestInfo.ClientAuthenticationInfo.AuthResult = new AuthenticationResult(true);
                return;
            }

            context.RequestInfo.ClientAuthenticationInfo.AuthResult = new AuthenticationResult(false, AuthenticationErrors.InvalidSignature, responseMessageConverter);
        }

        /// <summary>Determines whether this instance [can handle authentication type] the specified type.</summary>
        /// <param name="scheme">The authorization scheme.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public virtual bool CanHandleAuthScheme(string scheme)
        {
            if (string.IsNullOrWhiteSpace(scheme))
                return false;

            if (scheme.Equals(Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary> Gets or sets the key info provider
        /// </summary>
        public IApiKeyInfoProvider KeyInfoProvider { get; set; }

        /// <summary>Gets the authentication scheme.</summary>
        /// <value>The authentication scheme.</value>
        public string Scheme => AuthSchemeType.Shared.ToString();

        /// <summary>Gets the realm.</summary>
        /// <value>The realm.</value>
        public string Realm { get; }

        #region Helpers

        /// <summary>Builds the signature.</summary>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="method">The method.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="privateKey">The private key.</param>
        /// <param name="requestDate">The request date.</param>
        /// <returns></returns>
        private string BuildSignature(string requestUri, string method, string publicKey, string privateKey, DateTime? requestDate)
        {
            string uriToHash = GetRelativePath(requestUri);
            string encodedUri = WebUtility.UrlEncode(uriToHash);

            string verb = (!string.IsNullOrWhiteSpace(method))
               ? method.ToUpper()
               : string.Empty;


            string toSign = string.Format("{0}\n{1}\n{2}\n{3}",
                publicKey ?? string.Empty,
                verb ?? string.Empty,
                requestDate.GetValueOrDefault().ToUnixTime().ToString(CultureInfo.InvariantCulture),
                encodedUri);



            // get the byte[] representation of the private key
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] key = encoding.GetBytes(privateKey);
            Byte[] bytesToSign = encoding.GetBytes(toSign);

            byte[] data = null;
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                data = hmac.ComputeHash(bytesToSign);
            }

            string base64 = Convert.ToBase64String(data);
            return base64;
        }

        /// <summary>Compares the HMAC signatures.</summary>
        /// <param name="serviceSignature">The service signature.</param>
        /// <param name="clientSignature">The client signature.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool CompareSignatures(string serviceSignature, string clientSignature)
        {
            if (!string.IsNullOrWhiteSpace(serviceSignature) && !string.IsNullOrWhiteSpace(clientSignature))
                return serviceSignature.ToLower() == clientSignature.ToLower();

            return serviceSignature == clientSignature;
        }

        /// <summary>Gets the relative path.</summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        private static string GetRelativePath(string uri)
        {
            // GET RID OF QUERY STRING
            string returnUri = (uri.Contains("?"))
                ? uri.Substring(0, uri.IndexOf("?"))
                : uri;

            // GET RID OF PROTOCOL/HOST/PORT
            var firstSlash = (returnUri.Contains("://"))
                ? returnUri.IndexOf('/', returnUri.IndexOf("://") + 3)
                : returnUri.IndexOf('/');

            returnUri = returnUri.Substring(firstSlash, returnUri.Length - firstSlash);
            return returnUri;
        }

        #endregion
    }
}