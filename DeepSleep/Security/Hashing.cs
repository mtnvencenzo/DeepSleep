using System;
using System.Text;
using System.Security.Cryptography;

namespace DeepSleep.Security
{
    /// <summary>
    /// 
    /// </summary>
    public static class Hashing
    {
        /// <summary>Generates the hmacsh a512.</summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="stringToSign">The string to sign.</param>
        /// <returns></returns>
        public static byte[] GenerateHMACSHA512(string privateKey, string stringToSign)
        {
            Byte[] toSign = Encoding.UTF8.GetBytes(stringToSign);

            return GenerateHMACSHA512(privateKey, toSign);
        }

        /// <summary>Generates the hmacsh a512.</summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="toSign">To sign.</param>
        /// <returns></returns>
        public static byte[] GenerateHMACSHA512(string privateKey, byte[] toSign)
        {
            Byte[] key = Encoding.UTF8.GetBytes(privateKey);

            // Initialize the keyed hash object. 
            using (HMACSHA512 hmac = new HMACSHA512(key))
            {
                // Compute the hash of the input string 
                return hmac.ComputeHash(toSign);
            }
        }

        /// <summary>Generates the hmacsh a384.</summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="stringToSign">The string to sign.</param>
        /// <returns></returns>
        public static byte[] GenerateHMACSHA384(string privateKey, string stringToSign)
        {
            Byte[] toSign = Encoding.UTF8.GetBytes(stringToSign);

            return GenerateHMACSHA384(privateKey, toSign);
        }

        /// <summary>Generates the hmacsh a384.</summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="toSign">To sign.</param>
        /// <returns></returns>
        public static byte[] GenerateHMACSHA384(string privateKey, byte[] toSign)
        {
            Byte[] key = Encoding.UTF8.GetBytes(privateKey);

            // Initialize the keyed hash object. 
            using (HMACSHA384 hmac = new HMACSHA384(key))
            {
                // Compute the hash of the input string 
                return hmac.ComputeHash(toSign);
            }
        }

        /// <summary>
        /// Generates the HMACSH a256.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="stringToSign">The string to sign.</param>
        /// <returns></returns>
        public static byte[] GenerateHMACSHA256(string privateKey, string stringToSign)
        {
            Byte[] toSign = Encoding.UTF8.GetBytes(stringToSign);

            return GenerateHMACSHA256(privateKey, toSign);
        }

        /// <summary>
        /// Generates the HMACSH a256.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="toSign">To sign.</param>
        /// <returns></returns>
        public static byte[] GenerateHMACSHA256(string privateKey, byte[] toSign)
        {
            Byte[] key = Encoding.UTF8.GetBytes(privateKey);

            // Initialize the keyed hash object. 
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                // Compute the hash of the input string 
                return hmac.ComputeHash(toSign);
            }
        }

        /// <summary>
        /// Generates the sh a256.
        /// </summary>
        /// <param name="stringToSign">The string automatic sign.</param>
        /// <returns></returns>
        public static byte[] GenerateSHA256(string stringToSign)
        {
            var bytes = new ASCIIEncoding().GetBytes(stringToSign);

            return GenerateSHA256(bytes);
        }

        /// <summary>
        /// Generates the sh a256 query string.
        /// </summary>
        /// <param name="toSign">The automatic sign.</param>
        /// <returns></returns>
        public static byte[] GenerateSHA256(byte[] toSign)
        {
            using (var provider = SHA256.Create())
            {
                return provider.ComputeHash(toSign);
            }
        }
    }
}
