namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Xunit;

    public class ProcessHttpResponseCrossOriginResourceSharingTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true),
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = "https://test.org"
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void ReturnsTrueAndSkipsProcessingWhenNoOrginPresent(string origin)
        {
            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsSameRequestOrginWhenAllOrginsConfigured()
        {
            var origin = "http://www.google.com";

            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("false");
        }

        [Fact]
        public async void ReturnsEndpointConfiguredOrginWhenAllOrginsDefaultConfigured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { origin }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("false");
        }

        [Fact]
        public async void ReturnsEndpointConfiguredOrginWhenOtherOrginsDefaultConfigured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { origin }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("false");
        }

        [Fact]
        public async void ReturnsOrginWhenMultipleOrginsConfigured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "http://test.net", origin, "http://test2.net" }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("false");
        }

        [Fact]
        public async void ReturnsEmptyOrginWhenNoneConfigured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = null
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(string.Empty);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("false");
        }

        [Fact]
        public async void ReturnsEmptyOrginWhenNotInDefaultConfiguredListConfigured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = null
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(string.Empty);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("false");
        }

        [Fact]
        public async void ReturnsEmptyOrginWhenNotInEndpointListConfigured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "http://ronnie.vecchi.net", "http://ronn.vecchi.nets" }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(string.Empty);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("false");
        }

        [Fact]
        public async void ReturnsOrginWhenAllEndpointListConfigured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("false");
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async void ReturnsTrueAllowCredentialsWhenConfigured(bool? allowCredentials, bool expected)
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin,
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" },
                        AllowCredentials = allowCredentials
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be(expected.ToString().ToLowerInvariant());
        }

        [Fact]
        public async void ReturnsConfiguredAccessExposeHeaders()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" },
                        ExposeHeaders = new string[] { "X-API1", "X-API2", "Content-Type" }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(3);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("false");
            context.ResponseInfo.Headers[2].Name.Should().Be("Access-Control-Expose-Headers");
            context.ResponseInfo.Headers[2].Value.Should().Be("X-API1, X-API2, Content-Type");
        }

        [Fact]
        public async void DoesntReturnExposeHeadersWhenEmpty()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" },
                        ExposeHeaders = new string[] { }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("false");
        }
    }
}
