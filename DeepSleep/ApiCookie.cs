namespace DeepSleep
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{ToCookie()}")]
    public class ApiCookie
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SameSiteCookieValue SameSite { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HttpOnly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int MaxAgeSeconds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToCookie()
        {
            string name = (this.Secure)
                ? $"__Secure-{this.Name}"
                : this.Name;

            string cookieNameValue = $"{name}={this.Value}";

            string secure = this.Secure
                ? "; Secure"
                : string.Empty;

            string httpOnly = this.HttpOnly
                ? "; HttpOnly"
                : string.Empty;

            string maxAge = this.MaxAgeSeconds > 0
                ? $"; Max-Age={this.MaxAgeSeconds}"
                : string.Empty;

            string samesite = $"; SameSite={this.SameSite}";

            var cookie = $"{cookieNameValue}{secure}{httpOnly}{maxAge}{samesite}";
            return cookie;
        }
    }
}
