namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Moq;
    using System;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpResponseCachingTests
    {
        [Fact]
        public async void ReturnsTrueForCancelledRequest()
        {
            var mockResponseInfo = new Mock<ApiResponseInfo>();
            mockResponseInfo.Setup(m => m.ResponseObject).Throws(new Exception("test"));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true),
                Response = mockResponseInfo.Object
            };

            var processed = await context.ProcessHttpResponseCaching().ConfigureAwait(false);
            processed.Should().BeTrue();
        }

        [Fact]
        public async void ReturnsTrueForNullResponseObject()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = null,
                Response = new ApiResponseInfo
                {
                    ResponseObject = null
                }
            };

            var processed = await context.ProcessHttpResponseCaching().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.Headers[0].Name.Should().Be("Cache-Control");
            context.Response.Headers[0].Value.Should().Be("no-cache, no-store, must-revalidate, max-age=0");
            context.Response.Headers[1].Name.Should().Be("Expires");

            var now = DateTime.UtcNow;
            var expires = DateTimeOffset.Parse(context.Response.Headers[1].Value);
            expires.Year.Should().Be(now.Year - 1);
        }

        [Theory]
        [InlineData(199)]
        [InlineData(300)]
        [InlineData(204)]
        [InlineData(401)]
        [InlineData(403)]
        [InlineData(404)]
        [InlineData(405)]
        [InlineData(413)]
        [InlineData(414)]
        [InlineData(415)]
        [InlineData(500)]
        [InlineData(501)]
        public async void ReturnsTrueForNonSuccessOrNoContentResponse(int statusCode)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = null,
                Response = new ApiResponseInfo
                {
                    StatusCode = statusCode,
                    ResponseObject = null
                }
            };

            var processed = await context.ProcessHttpResponseCaching().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.Headers[0].Name.Should().Be("Cache-Control");
            context.Response.Headers[0].Value.Should().Be("no-cache, no-store, must-revalidate, max-age=0");
            context.Response.Headers[1].Name.Should().Be("Expires");

            var now = DateTime.UtcNow;
            var expires = DateTimeOffset.Parse(context.Response.Headers[1].Value);
            expires.Year.Should().Be(now.Year - 1);
        }

        [Theory]
        [InlineData("POST")]
        [InlineData("TRACE")]
        [InlineData("PATCH")]
        [InlineData("DELETE")]
        public async void ReturnsTrueAndCorrectHeadersForNonCachableRequestMethod(string method)
        {
            var mockRouteInfo = new Mock<ApiRoutingInfo>();
            mockRouteInfo.Setup(m => m.Route).Throws(new Exception("test"));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = method
                },
                Response = new ApiResponseInfo
                {
                    StatusCode = 200
                },
                Routing = mockRouteInfo.Object
            };

            var processed = await context.ProcessHttpResponseCaching().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.Headers[0].Name.Should().Be("Cache-Control");
            context.Response.Headers[0].Value.Should().Be("no-cache, no-store, must-revalidate, max-age=0");
            context.Response.Headers[1].Name.Should().Be("Expires");

            var now = DateTime.UtcNow;
            var expires = DateTimeOffset.Parse(context.Response.Headers[1].Value);
            expires.Year.Should().Be(now.Year - 1);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("PUT")]
        [InlineData("OPTIONS")]
        [InlineData("HEAD")]
        public async void ReturnsTrueAndCorrectHeadersForCachableRequestMethodButNoRouteInfo(string method)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = method
                },
                Response = new ApiResponseInfo
                {
                    StatusCode = 200,
                    ResponseObject = null
                },
                Routing = null
            };

            var processed = await context.ProcessHttpResponseCaching().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.Headers[0].Name.Should().Be("Cache-Control");
            context.Response.Headers[0].Value.Should().Be("no-cache, no-store, must-revalidate, max-age=0");
            context.Response.Headers[1].Name.Should().Be("Expires");

            var now = DateTime.UtcNow;
            var expires = DateTimeOffset.Parse(context.Response.Headers[1].Value);
            expires.Year.Should().Be(now.Year - 1);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("PUT")]
        [InlineData("OPTIONS")]
        [InlineData("HEAD")]
        public async void ReturnsTrueAndCorrectHeadersForCachableRequestMethodButNoRoutingItem(string method)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = method
                },
                Response = new ApiResponseInfo
                {
                    StatusCode = 200,
                    ResponseObject = null
                },
                Routing = new ApiRoutingInfo
                {
                    Route = null
                }
            };

            var processed = await context.ProcessHttpResponseCaching().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.Headers[0].Name.Should().Be("Cache-Control");
            context.Response.Headers[0].Value.Should().Be("no-cache, no-store, must-revalidate, max-age=0");
            context.Response.Headers[1].Name.Should().Be("Expires");

            var now = DateTime.UtcNow;
            var expires = DateTimeOffset.Parse(context.Response.Headers[1].Value);
            expires.Year.Should().Be(now.Year - 1);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("PUT")]
        [InlineData("OPTIONS")]
        [InlineData("HEAD")]
        public async void ReturnsTrueAndCorrectHeadersForCachableRequestMethodButNoRoutingItemConfig(string method)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = method
                },
                Response = new ApiResponseInfo
                {
                    StatusCode = 200,
                    ResponseObject = null
                },
                Configuration = null
            };

            var processed = await context.ProcessHttpResponseCaching().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.Headers[0].Name.Should().Be("Cache-Control");
            context.Response.Headers[0].Value.Should().Be("no-cache, no-store, must-revalidate, max-age=0");
            context.Response.Headers[1].Name.Should().Be("Expires");

            var now = DateTime.UtcNow;
            var expires = DateTimeOffset.Parse(context.Response.Headers[1].Value);
            expires.Year.Should().Be(now.Year - 1);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("PUT")]
        [InlineData("OPTIONS")]
        [InlineData("HEAD")]
        public async void ReturnsTrueAndCorrectHeadersForCachableRequestMethodButNoRoutingItemConfigCacheDirective(string method)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = method
                },
                Response = new ApiResponseInfo
                {
                    StatusCode = 200,
                    ResponseObject = null
                },
                Configuration = new DefaultApiRequestConfiguration
                {
                    CacheDirective = null
                }
            };

            var processed = await context.ProcessHttpResponseCaching().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.Headers[0].Name.Should().Be("Cache-Control");
            context.Response.Headers[0].Value.Should().Be("no-cache, no-store, must-revalidate, max-age=0");
            context.Response.Headers[1].Name.Should().Be("Expires");

            var now = DateTime.UtcNow;
            var expires = DateTimeOffset.Parse(context.Response.Headers[1].Value);
            expires.Year.Should().Be(now.Year - 1);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("PUT")]
        [InlineData("OPTIONS")]
        [InlineData("HEAD")]
        public async void ReturnsTrueAndCorrectHeadersForCachableRequestMethodButCacheDirectiveIsNotCacheable(string method)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = method
                },
                Response = new ApiResponseInfo
                {
                    StatusCode = 200
                },
                Configuration = new DefaultApiRequestConfiguration
                {
                    CacheDirective = new ApiCacheDirectiveConfiguration
                    {
                        Cacheability = HttpCacheType.NoCache,
                        ExpirationSeconds = 100
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCaching().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.Headers[0].Name.Should().Be("Cache-Control");
            context.Response.Headers[0].Value.Should().Be("no-cache, no-store, must-revalidate, max-age=0");
            context.Response.Headers[1].Name.Should().Be("Expires");

            var now = DateTime.UtcNow;
            var expires = DateTimeOffset.Parse(context.Response.Headers[1].Value);
            expires.Year.Should().Be(now.Year - 1);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("PUT")]
        [InlineData("OPTIONS")]
        [InlineData("HEAD")]
        public async void ReturnsTrueAndCorrectHeadersForCachableRequestMethodButCacheDirectiveSecondsIsZero(string method)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = method
                },
                Response = new ApiResponseInfo
                {
                    StatusCode = 200,
                    ResponseObject = null
                },
                Configuration = new DefaultApiRequestConfiguration
                {
                    CacheDirective = new ApiCacheDirectiveConfiguration
                    {
                        Cacheability = HttpCacheType.Cacheable,
                        ExpirationSeconds = 0
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCaching().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.Headers[0].Name.Should().Be("Cache-Control");
            context.Response.Headers[0].Value.Should().Be("no-cache, no-store, must-revalidate, max-age=0");
            context.Response.Headers[1].Name.Should().Be("Expires");

            var now = DateTime.UtcNow;
            var expires = DateTimeOffset.Parse(context.Response.Headers[1].Value);
            expires.Year.Should().Be(now.Year - 1);
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("PUT")]
        [InlineData("OPTIONS")]
        [InlineData("HEAD")]
        public async void ReturnsTrueAndCorrectHeadersForCachableRequestMethodAndPrivateCacheLocation(string method)
        {
            var cacheDirective = new ApiCacheDirectiveConfiguration
            {
                Cacheability = HttpCacheType.Cacheable,
                CacheLocation = HttpCacheLocation.Private,
                ExpirationSeconds = 1
            };

            var expiresDate = DateTime.Parse(DateTime.UtcNow.AddSeconds(cacheDirective.ExpirationSeconds.Value).ToString("r"));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = method
                },
                Response = new ApiResponseInfo
                {
                    StatusCode = 200,
                    ResponseObject = null
                },
                Configuration = new DefaultApiRequestConfiguration
                {
                    CacheDirective = cacheDirective
                }
            };

            var processed = await context.ProcessHttpResponseCaching().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Cache-Control");
            context.Response.Headers[0].Value.Should().Be($"{cacheDirective.CacheLocation.ToString().ToLower()}, max-age={cacheDirective.ExpirationSeconds}");
            context.Response.Headers[1].Name.Should().Be("Expires");

            var expires = DateTime.Parse(context.Response.Headers[1].Value);
            expires.Should().BeOnOrAfter(expiresDate);

            context.Response.Headers[2].Name.Should().Be("Vary");
            context.Response.Headers[2].Value.Should().Be("Accept, Accept-Encoding, Accept-Language");
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("PUT")]
        [InlineData("OPTIONS")]
        [InlineData("HEAD")]
        public async void ReturnsTrueAndCorrectHeadersForCachableRequestMethodAndPublicCacheLocation(string method)
        {
            var cacheDirective = new ApiCacheDirectiveConfiguration
            {
                Cacheability = HttpCacheType.Cacheable,
                CacheLocation = HttpCacheLocation.Public,
                ExpirationSeconds = 1
            };

            var expiresDate = DateTime.Parse(DateTime.UtcNow.AddSeconds(cacheDirective.ExpirationSeconds.Value).ToString("r"));

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = method
                },
                Response = new ApiResponseInfo
                {
                    StatusCode = 200
                },
                Configuration = new DefaultApiRequestConfiguration
                {
                    CacheDirective = cacheDirective
                }
            };

            var processed = await context.ProcessHttpResponseCaching().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Cache-Control");
            context.Response.Headers[0].Value.Should().Be($"{cacheDirective.CacheLocation.ToString().ToLower()}, max-age={cacheDirective.ExpirationSeconds}");
            context.Response.Headers[1].Name.Should().Be("Expires");

            var expires = DateTime.Parse(context.Response.Headers[1].Value);
            expires.Should().BeOnOrAfter(expiresDate);

            context.Response.Headers[2].Name.Should().Be("Vary");
            context.Response.Headers[2].Value.Should().Be("Accept, Accept-Encoding, Accept-Language");
        }
    }
}
