namespace Api.DeepSleep.Controllers.Cookies
{
    using global::DeepSleep;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class CookieModel
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
    }
}
