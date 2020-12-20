namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
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
        public DateTime? Expires { get; set; }

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

            string expires = string.Empty;

            if (!string.IsNullOrWhiteSpace(maxAge) && this.Expires.HasValue)
            {
                expires = $"; Expires={this.Expires.Value.ToString("r")}";
            }

            string samesite = $"; SameSite={this.SameSite}";

            var cookie = $"{cookieNameValue}{secure}{httpOnly}{maxAge}{expires}{samesite}";
            return cookie;
        }
    }
}
