namespace DeepSleep.Api.NetCore.Tests
{
    using DeepSleep;
    using DeepSleep.Auth;
    using DeepSleep.Validation;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using System.Web;
    using System.Xml;
    using System.Xml.Serialization;

    public abstract class TestBase
    {
        protected readonly string host = "ut-test.com";
        protected readonly string applicationJson = "application/json";
        protected readonly string textJson = "text/json";
        protected readonly string textPlain = "text/plain";
        protected readonly string applicationWwwFormUrlEncoded = "application/x-www-form-urlencoded";
        protected readonly string applicationXml = "application/xml";
        protected readonly string textXml = "text/xml";
        protected readonly string applicationJsonPatch = "application/json-patch+json";
        protected static readonly string multipartBoundary = "gc0p4Jq0M2Yt08j34c0p";
        protected readonly string multipartFormData = $"multipart/form-data; boundary=\"{multipartBoundary}\"";
        protected readonly string cacheControlNoCache = "no-cache, no-store, must-revalidate, max-age=0";
        protected readonly string authSchemeToken = "Token";
        protected readonly string authSchemeBearer = "Bearer";
        protected readonly string staticToken = "T0RrMlJqWXpNVFF0UmtReFF5MDBRamN5TFVJeE5qZ3RPVGxGTlRBek5URXdNVUkz";

        protected void AssertResponse(
            ApiRequestContext apiContext,
            HttpResponse response,
            int expectedHttpStatus = 200,
            bool shouldHaveResponse = true,
            string expectedContentType = null,
            ApiValidationState expectedValidationState = ApiValidationState.Succeeded,
            NameValuePairs<string, string> extendedHeaders = null,
            bool shouldBeCancelledReuqest = false,
            string expectedProtocol = "HTTP/1.1",
            bool expectedPrettyPrint = false,
            bool expectRequestIdHeader = true,
            bool? expectedAuthenticationResult = null,
            string expectedAuthenticationScheme = null,
            string expectedAuthenticationValue = null,
            string expectedCacheControlValue = null,
            int? expectedExpiresSecondsAdd = null,
            string expectedCulture = null,
            AuthenticationType expectedAuthenticatedBy = AuthenticationType.None,
            bool? expectedAuthorizationResult = null,
            AuthorizationType expectedAuthorizedBy = AuthorizationType.None,
            bool? allowCustom500Response = false,
            long? expectedContentLength = null,
            NameValuePairs<string, string> expectedItems = null)
        {
            apiContext.Should().NotBeNull();
            response.Should().NotBeNull();

            // Check for extended messages and exceptions on expected
            // success responses.  This helps in debuging failing tests
            if (expectedHttpStatus >= 200 && expectedHttpStatus < 300)
            {
                foreach (var error in apiContext.Runtime.Exceptions)
                {
                    error.ToString().Should().BeNull();
                }

                foreach (var error in apiContext.Validation.Errors)
                {
                    error.Should().BeNull();
                }
            }


            apiContext.Response.StatusCode.Should().Be(expectedHttpStatus);
            apiContext.ValidationState().Should().Be(expectedValidationState);
            apiContext.PathBase.Should().Be(apiContext.Request.Path);
            response.StatusCode.Should().Be(expectedHttpStatus);

            var expectedHeaderCount = ((response.StatusCode >= 500 && allowCustom500Response != true) || apiContext.Request.IsCorsPreflightRequest())
                ? 3 + (extendedHeaders?.Count ?? 0) + (shouldHaveResponse ? 0 : -1)
                : 5 + (extendedHeaders?.Count ?? 0) + (shouldHaveResponse ? 0 : -1);

            if (expectRequestIdHeader)
            {
                expectedHeaderCount++;
            }


            var responseHeaderCount = response.Headers.Sum(c => c.Value.Count);

            // Check for headers
            apiContext.Response.Headers.Should().HaveCount(expectedHeaderCount);
            responseHeaderCount.Should().Be(expectedHeaderCount);

            response.Headers.Should().ContainKey("Date");
            response.Headers["Date"].Should().Equal(apiContext.Response.Date?.ToString("r"));
            apiContext.Response.Headers.HasHeader("Date").Should().BeTrue();
            apiContext.Response.Headers.GetHeader("Date").Value.Should().Be(apiContext.Response.Date?.ToString("r"));

            var assertionContentLength = expectedContentLength ?? response.Body.Length;

            response.Headers.Should().ContainKey("Content-Length");
            response.Headers["Content-Length"].Should().Equal(assertionContentLength.ToString());
            apiContext.Response.Headers.HasHeader("Content-Length").Should().BeTrue();
            apiContext.Response.Headers.GetHeader("Content-Length").Value.Should().Be(assertionContentLength.ToString());
            apiContext.Response.ContentLength.Should().Be(assertionContentLength);

            if (expectRequestIdHeader)
            {
                response.Headers.Should().ContainKey("X-RequestId");
                response.Headers["X-RequestId"].Should().Equal(apiContext.Request.RequestIdentifier);
                apiContext.Response.Headers.HasHeader("X-RequestId").Should().BeTrue();
                apiContext.Response.Headers.GetHeader("X-RequestId").Value.Should().Be(apiContext.Request.RequestIdentifier);
            }

            if (response.StatusCode < 500 && apiContext.Request.IsCorsPreflightRequest() == false)
            {
                response.Headers.Should().ContainKey("Cache-Control");
                response.Headers["Cache-Control"].Should().Equal(expectedCacheControlValue ?? this.cacheControlNoCache);
                apiContext.Response.Headers.HasHeader("Cache-Control").Should().BeTrue();
                apiContext.Response.Headers.GetHeader("Cache-Control").Value.Should().Be(expectedCacheControlValue ?? this.cacheControlNoCache);

                var expectedExpiresValue = expectedExpiresSecondsAdd.HasValue
                    ? apiContext.Response.Date?.AddSeconds(expectedExpiresSecondsAdd.Value).ToString("r")
                    : apiContext.Response.Date?.AddYears(-1).ToString("r");

                response.Headers.Should().ContainKey("Expires");
                response.Headers["Expires"].Should().Equal(expectedExpiresValue);
                apiContext.Response.Headers.HasHeader("Expires").Should().BeTrue();
                apiContext.Response.Headers.GetHeader("Expires").Value.Should().Be(expectedExpiresValue);
            }

            if (shouldHaveResponse)
            {
                response.Headers.Should().ContainKey("Content-Type");
                response.Headers["Content-Type"].Should().Equal(expectedContentType ?? this.applicationJson);
                apiContext.Response.Headers.HasHeader("Content-Type").Should().BeTrue();
                apiContext.Response.Headers.GetHeader("Content-Type").Value.Should().Be(expectedContentType ?? this.applicationJson);
                apiContext.Response.ContentType.Should().Be(expectedContentType);

                apiContext.Response.ContentLength.Should().BeGreaterThan(0);
                int.TryParse(response.Headers["Content-Length"], out var contentLength);
                contentLength.Should().BeGreaterThan(0);

                apiContext.Response.ResponseWriter.Should().NotBeNull();
                apiContext.Response.ResponseWriterOptions.Should().NotBeNull();
                apiContext.Response.ResponseWriterOptions.PrettyPrint.Should().Be(expectedPrettyPrint);

                if (apiContext.Request.IsHeadRequest())
                {
                    apiContext.Response.ResponseObject.Should().BeNull();
                }
                else
                {
                    apiContext.Response.ResponseObject.Should().NotBeNull();
                }
            }
            else
            {
                response.Headers.Should().NotContainKey("Content-Type");
                apiContext.Response.Headers.HasHeader("Content-Type").Should().BeFalse();

                apiContext.Response.ResponseObject.Should().BeNull();
                apiContext.Response.ResponseWriter.Should().BeNull();
                apiContext.Response.ResponseWriterOptions.Should().BeNull();

                apiContext.Response.ContentLength.Should().Be(0);
                int.Parse(response.Headers["Content-Length"]).Should().Be(0);
            }

            if (extendedHeaders != null)
            {
                foreach (var header in extendedHeaders)
                {
                    response.Headers.Should().ContainKey(header.Key);
                    response.Headers[header.Key].Should().Contain(header.Value);

                    apiContext.Response.Headers.HasHeader(header.Key).Should().BeTrue();

                    var matchingHeader = apiContext.Response.Headers
                        .ToList()
                        .Where(h => h.Name == header.Key)
                        .Where(h => h.Value == header.Value)
                        .FirstOrDefault();

                    matchingHeader.Should().NotBeNull();
                }
            }

            // -------------------------------------
            // Misicalleanous Api Context Validation
            apiContext.Runtime.Duration.Should().NotBeNull();
            apiContext.Runtime.Duration.Duration.Should().Be(
                (int)(apiContext.Runtime.Duration.UtcEnd.Value - apiContext.Runtime.Duration.UtcStart).TotalMilliseconds);

            apiContext.RequestAborted.IsCancellationRequested.Should().Be(shouldBeCancelledReuqest);
            apiContext.Request.Protocol.Should().Be(expectedProtocol);
            apiContext.Request.RequestIdentifier.Should().NotBeNull();

            if (!(apiContext.Configuration?.AllowAnonymous ?? false) && expectedAuthenticationResult.HasValue)
            {
                apiContext.Request.ClientAuthenticationInfo.Should().NotBeNull();
                apiContext.Request.ClientAuthenticationInfo.AuthenticatedBy.Should().Be(expectedAuthenticatedBy);
                apiContext.Request.ClientAuthenticationInfo.AuthScheme.Should().Be(expectedAuthenticationScheme);
                apiContext.Request.ClientAuthenticationInfo.AuthValue.Should().Be(expectedAuthenticationValue);
                apiContext.Request.ClientAuthenticationInfo.AuthResult.Should().NotBeNull();
                apiContext.Request.ClientAuthenticationInfo.AuthResult.IsAuthenticated.Should().Be(expectedAuthenticationResult.Value);
            }

            if (apiContext.Request.ClientAuthenticationInfo?.AuthResult?.IsAuthenticated == true)
            {
                if (!(apiContext.Configuration?.AllowAnonymous ?? false) && expectedAuthorizationResult.HasValue)
                {
                    apiContext.Request.ClientAuthorizationInfo.Should().NotBeNull();
                    apiContext.Request.ClientAuthorizationInfo.AuthorizedBy.Should().Be(expectedAuthorizedBy);
                    apiContext.Request.ClientAuthorizationInfo.AuthResult.Should().NotBeNull();
                    apiContext.Request.ClientAuthorizationInfo.AuthResult.IsAuthorized.Should().Be(expectedAuthorizationResult.Value);
                }
            }
            else
            {
                apiContext.Request.ClientAuthorizationInfo.Should().BeNull();
            }

            apiContext.Request.AcceptCulture?.Name.Should().Be(expectedCulture ?? CultureInfo.CurrentUICulture.Name);

            apiContext.Items.ContainsKey("requestHandlerCount").Should().BeTrue();
            apiContext.Items["requestHandlerCount"].Should().Be(1);

            var exceptionCount = apiContext.Runtime.Exceptions
                .Where(e => e as ApiException == null)
                .Count();

            if (apiContext.Response.StatusCode >= 500 && exceptionCount > 0)
            {
                apiContext.Items.ContainsKey("exceptionHandlerCount").Should().BeTrue();
                apiContext.Items["exceptionHandlerCount"].Should().Be(1);
            }

            if (expectedItems != null)
            {
                foreach (var item in expectedItems)
                {
                    apiContext.Items.TryGetValue(item.Key, out var val);
                    val.Should().Be(item.Value);
                }
            }

            var jsonDump = apiContext.Dump();
            jsonDump.Should().NotBeNullOrWhiteSpace();
        }

        protected Task<T> GetResponseData<T>(HttpResponse response)
        {
            if (response.Body == null || !response.Body.CanRead || response.Body.Length == 0)
            {
                return Task.FromResult(default(T));
            }

            var contentType = response.Headers["Content-Type"][0] ?? string.Empty;

            if (contentType == this.applicationJson || contentType == this.textJson)
            {
                using (var reader = new StreamReader(response.Body))
                {
                    response.Body.Seek(0, SeekOrigin.Begin);
                    var data = reader.ReadToEnd();

                    var serializerOptions = new JsonSerializerOptions
                    {
                        AllowTrailingCommas = false,
                        PropertyNameCaseInsensitive = true,
                        IgnoreReadOnlyFields = true,
                        IgnoreReadOnlyProperties = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    };

                    serializerOptions.Converters.Add(new TimeSpanConverter());
                    serializerOptions.Converters.Add(new NullableTimeSpanConverter());
                    serializerOptions.Converters.Add(new JsonStringEnumConverter());

                    var obj = JsonSerializer.Deserialize<T>(data, serializerOptions);

                    return Task.FromResult(obj);
                }
            }
            else if (contentType == this.applicationXml || contentType == this.textXml || contentType == "other/xml")
            {
                object obj;
                var serializer = new XmlSerializer(typeof(T));
                var settings = new XmlReaderSettings
                {
                    CloseInput = false,
                    ConformanceLevel = ConformanceLevel.Fragment,
                    IgnoreComments = true,
                    ValidationType = ValidationType.None
                };

                response.Body.Seek(0, SeekOrigin.Begin);

                using (XmlReader reader = XmlReader.Create(response.Body, settings))
                {
                    obj = serializer.Deserialize(reader);

                    return obj != null
                        ? Task.FromResult((T)obj)
                        : Task.FromResult(default(T));
                }
            }
            else if (contentType == this.textPlain)
            {
                response.Body.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(response.Body))
                {
                    object data = reader.ReadToEnd();

                    return Task.FromResult((T)data);
                }
            }

            throw new Exception($"Content-Type {contentType} is not supported.");
        }

        protected Task<string> GetResponseDataString(HttpResponse response)
        {
            using (var reader = new StreamReader(response.Body))
            {
                response.Body.Seek(0, SeekOrigin.Begin);
                var data = reader.ReadToEnd();
                return Task.FromResult(data);
            }
        }

        protected static string UrlEncode(string value)
        {
            return HttpUtility.UrlEncode(value);
        }
    }
}
