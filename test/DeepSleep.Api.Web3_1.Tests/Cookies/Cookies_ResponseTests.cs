namespace DeepSleep.Api.Web.Tests.Cookies
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Auth;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Cookies;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class Cookies_ResponseTests : PipelineTestBase
    {
        [Fact]
        public async Task cookies___secured_samesite_strict_httponly_no_expires_maxage()
        {
            base.SetupEnvironment();

            var utcNow = DateTimeOffset.UtcNow;
            var request = @$"
GET https://{host}/cookies/response/cookie/secured/httponly/samesite-strict/no/maxage HTTP/1.1
Date: {utcNow.ToString("r")}
Host: {host}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedAuthenticatedBy: AuthenticationType.Anonymous,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Set-Cookie", "__Secure-AS=1; Secure; HttpOnly; SameSite=Strict" }
                });

            var data = await base.GetResponseData<CookieModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.HttpOnly.Should().Be(true);
            data.Name.Should().Be("AS");
            data.SameSite.Should().Be(SameSiteCookieValue.Strict);
            data.Secure.Should().Be(true);
            data.Value.Should().Be("1");
            data.MaxAgeSeconds.Should().Be(0);
        }

        [Fact]
        public async Task cookies___notsecured_samesite_lax_maxage_no_value()
        {
            base.SetupEnvironment();

            var utcNow = DateTimeOffset.UtcNow;
            var request = @$"
GET https://{host}/cookies/response/cookie/notsecured/samesite-lax/maxage/no/value HTTP/1.1
Date: {utcNow.ToString("r")}
Host: {host}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;


            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedAuthenticatedBy: AuthenticationType.Anonymous,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Set-Cookie", "AS=; Max-Age=10; SameSite=Lax" }
                });

            var data = await base.GetResponseData<CookieModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.HttpOnly.Should().Be(false);
            data.Name.Should().Be("AS");
            data.SameSite.Should().Be(SameSiteCookieValue.Lax);
            data.Secure.Should().Be(false);
            data.Value.Should().BeNull();
            data.MaxAgeSeconds.Should().Be(10);
        }

        [Fact]
        public async Task cookies___notsecured_samesite_none_no_maxage()
        {
            base.SetupEnvironment();

            var utcNow = DateTimeOffset.UtcNow;
            var request = @$"
GET https://{host}/cookies/response/cookie/notsecured/samesite-none/no/maxage HTTP/1.1
Date: {utcNow.ToString("r")}
Host: {host}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;


            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedAuthenticatedBy: AuthenticationType.Anonymous,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Set-Cookie", "AS=test; SameSite=None" }
                });

            var data = await base.GetResponseData<CookieModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.HttpOnly.Should().Be(false);
            data.Name.Should().Be("AS");
            data.SameSite.Should().Be(SameSiteCookieValue.None);
            data.Secure.Should().Be(false);
            data.Value.Should().Be("test");
            data.MaxAgeSeconds.Should().Be(0);
        }

        [Fact]
        public async Task cookies___notsecured_samesite_none_no_maxage_value()
        {
            base.SetupEnvironment();

            var utcNow = DateTimeOffset.UtcNow;
            var request = @$"
GET https://{host}/cookies/response/cookie/notsecured/samesite-none/no/maxage/value HTTP/1.1
Date: {utcNow.ToString("r")}
Host: {host}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;


            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedAuthenticatedBy: AuthenticationType.Anonymous,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Set-Cookie", "AS=; SameSite=None" }
                });

            var data = await base.GetResponseData<CookieModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.HttpOnly.Should().Be(false);
            data.Name.Should().Be("AS");
            data.SameSite.Should().Be(SameSiteCookieValue.None);
            data.Secure.Should().Be(false);
            data.Value.Should().BeNull();
            data.MaxAgeSeconds.Should().Be(0);
        }

        [Fact]
        public async Task cookies___notsecured_samesite_strict_maxage_value_httponly()
        {
            base.SetupEnvironment();

            var utcNow = DateTimeOffset.UtcNow;
            var request = @$"
GET https://{host}/cookies/response/cookie/secured/samesite-dtrict/maxage/value/httponly HTTP/1.1
Date: {utcNow.ToString("r")}
Host: {host}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;


            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedAuthenticatedBy: AuthenticationType.Anonymous,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Set-Cookie", "__Secure-AS=test; Secure; HttpOnly; Max-Age=10; SameSite=Strict" }
                });

            var data = await base.GetResponseData<CookieModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.HttpOnly.Should().Be(true);
            data.Name.Should().Be("AS");
            data.SameSite.Should().Be(SameSiteCookieValue.Strict);
            data.Secure.Should().Be(true);
            data.Value.Should().Be("test");
            data.MaxAgeSeconds.Should().Be(10);
        }
    }
}
