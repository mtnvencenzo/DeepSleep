namespace Api.DeepSleep.Controllers.Cookies
{
    using global::DeepSleep;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class CookiesController
    {
        private readonly IApiRequestContextResolver contextResolver;

        /// <summary>Initializes a new instance of the <see cref="CookiesController"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public CookiesController(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver;
        }

        /// <summary>Gets the secured same site no expires maxage.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "cookies/response/cookie/secured/httponly/samesite-strict/no/maxage")]
        public CookieModel GetSecuredSameSiteStrictHttpOnlyNoMaxage()
        {
            var context = this.contextResolver.GetContext();

            var model = new CookieModel
            {
                Name = "AS",
                Value = "1",
                HttpOnly = true,
                SameSite = SameSiteCookieValue.Strict,
                Secure = true
            };

            context.AddResponseCookie(new ApiCookie
            {
                Name = model.Name,
                Value = model.Value,
                HttpOnly = model.HttpOnly,
                SameSite = model.SameSite,
                Secure = model.Secure
            });

            return model;
        }

        /// <summary>Gets the secured same site lax maxage no expires.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "cookies/response/cookie/notsecured/samesite-lax/maxage/no/value")]
        public CookieModel GetSecuredSameSiteLaxMaxageNoValue()
        {
            var context = this.contextResolver.GetContext();

            var model = new CookieModel
            {
                Name = "AS",
                Value = null,
                HttpOnly = false,
                SameSite = SameSiteCookieValue.Lax,
                Secure = false,
                MaxAgeSeconds = 10
            };

            context.AddResponseCookie(new ApiCookie
            {
                Name = model.Name,
                Value = model.Value,
                HttpOnly = model.HttpOnly,
                SameSite = model.SameSite,
                Secure = model.Secure,
                MaxAgeSeconds = model.MaxAgeSeconds
            });

            return model;
        }

        /// <summary>Gets the secured same site lax maxage no expires.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "cookies/response/cookie/notsecured/samesite-none/no/maxage")]
        public CookieModel GetSecuredBareBonesValue()
        {
            var context = this.contextResolver.GetContext();

            var model = new CookieModel
            {
                Name = "AS",
                Value = "test",
                HttpOnly = false,
                SameSite = SameSiteCookieValue.None,
                Secure = false,
                MaxAgeSeconds = 0
            };

            context.AddResponseCookie(new ApiCookie
            {
                Name = model.Name,
                Value = model.Value,
                HttpOnly = model.HttpOnly,
                SameSite = model.SameSite,
                Secure = model.Secure,
                MaxAgeSeconds = model.MaxAgeSeconds
            });

            return model;
        }

        /// <summary>Gets the secured bare bones no value.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "cookies/response/cookie/notsecured/samesite-none/no/maxage/value")]
        public CookieModel GetSecuredBareBonesNoValue()
        {
            var context = this.contextResolver.GetContext();

            var model = new CookieModel
            {
                Name = "AS",
                Value = null,
                HttpOnly = false,
                SameSite = SameSiteCookieValue.None,
                Secure = false,
                MaxAgeSeconds = 0
            };

            context.AddResponseCookie(new ApiCookie
            {
                Name = model.Name,
                Value = model.Value,
                HttpOnly = model.HttpOnly,
                SameSite = model.SameSite,
                Secure = model.Secure,
                MaxAgeSeconds = model.MaxAgeSeconds
            });

            return model;
        }

        /// <summary>Gets the secured bare bones no value.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "cookies/response/cookie/secured/samesite-dtrict/maxage/value/httponly")]
        public CookieModel GetSecuredFullValue()
        {
            var context = this.contextResolver.GetContext();

            var model = new CookieModel
            {
                Name = "AS",
                Value = "test",
                HttpOnly = true,
                SameSite = SameSiteCookieValue.Strict,
                Secure = true,
                MaxAgeSeconds = 10
            };

            context.AddResponseCookie(new ApiCookie
            {
                Name = model.Name,
                Value = model.Value,
                HttpOnly = model.HttpOnly,
                SameSite = model.SameSite,
                Secure = model.Secure,
                MaxAgeSeconds = model.MaxAgeSeconds
            });

            return model;
        }
    }
}
