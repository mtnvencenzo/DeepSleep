namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Xunit;

    public class ProcessHttpResponseCrossOriginResourceSharingTests
    {
        [Fact]
        public async void pipeline_cors___returns_false_for_cancelled_request()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true),
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = "https://test.org"
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void pipeline_cors___returns_true_and_skips_processing_when_no_orgin_present(string origin)
        {
            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void pipeline_cors___returns_same_request_orgin_when_all_orgins_configured()
        {
            var origin = "http://www.google.com";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Headers.Should().NotBeEmpty();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.Response.Headers[0].Value.Should().Be(origin);
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.Response.Headers[1].Value.Should().Be("false");
            context.Response.Headers[2].Name.Should().Be("Vary");
            context.Response.Headers[2].Value.Should().Be("Origin");
        }

        [Fact]
        public async void pipeline_cors___returns_endpoint_configured_orgin_when_all_orgins_default_configured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { origin }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Headers.Should().NotBeEmpty();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.Response.Headers[0].Value.Should().Be(origin);
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.Response.Headers[1].Value.Should().Be("false");
            context.Response.Headers[2].Name.Should().Be("Vary");
            context.Response.Headers[2].Value.Should().Be("Origin");
        }

        [Fact]
        public async void pipeline_cors___returns_endpoint_configured_orgin_when_other_orgins_default_configured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { origin }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Headers.Should().NotBeEmpty();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.Response.Headers[0].Value.Should().Be(origin);
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.Response.Headers[1].Value.Should().Be("false");
            context.Response.Headers[2].Name.Should().Be("Vary");
            context.Response.Headers[2].Value.Should().Be("Origin");
        }

        [Fact]
        public async void pipeline_cors___returns_orgin_when_multiple_orgins_configured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "http://test.net", origin, "http://test2.net" }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Headers.Should().NotBeEmpty();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.Response.Headers[0].Value.Should().Be(origin);
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.Response.Headers[1].Value.Should().Be("false");
            context.Response.Headers[2].Name.Should().Be("Vary");
            context.Response.Headers[2].Value.Should().Be("Origin");
        }

        [Fact]
        public async void pipeline_cors___returns_empty_orgin_when_none_configured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedOrigins = null
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Headers.Should().NotBeEmpty();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.Response.Headers[0].Value.Should().Be(string.Empty);
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.Response.Headers[1].Value.Should().Be("false");
            context.Response.Headers[2].Name.Should().Be("Vary");
            context.Response.Headers[2].Value.Should().Be("Origin");
        }

        [Fact]
        public async void pipeline_cors___returns_empty_orgin_when_not_in_default_configured_list_configured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedOrigins = null
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Headers.Should().NotBeEmpty();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.Response.Headers[0].Value.Should().Be(string.Empty);
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.Response.Headers[1].Value.Should().Be("false");
            context.Response.Headers[2].Name.Should().Be("Vary");
            context.Response.Headers[2].Value.Should().Be("Origin");
        }

        [Fact]
        public async void pipeline_cors___returns_empty_orgin_when_not_in_endpoint_list_configured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "http://ronnie.vecchi.net", "http://ronn.vecchi.nets" }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Headers.Should().NotBeEmpty();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.Response.Headers[0].Value.Should().Be(string.Empty);
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.Response.Headers[1].Value.Should().Be("false");
            context.Response.Headers[2].Name.Should().Be("Vary");
            context.Response.Headers[2].Value.Should().Be("Origin");
        }

        [Fact]
        public async void pipeline_cors___returns_orgin_when_all_endpoint_list_configured()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Headers.Should().NotBeEmpty();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.Response.Headers[0].Value.Should().Be(origin);
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.Response.Headers[1].Value.Should().Be("false");
            context.Response.Headers[2].Name.Should().Be("Vary");
            context.Response.Headers[2].Value.Should().Be("Origin");
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public async void pipeline_cors___returns_true_allow_credentials_when_configured(bool? allowCredentials, bool expected)
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin,
                    }
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" },
                        AllowCredentials = allowCredentials
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Headers.Should().NotBeEmpty();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.Response.Headers[0].Value.Should().Be(origin);
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.Response.Headers[1].Value.Should().Be(expected.ToString().ToLowerInvariant());
            context.Response.Headers[2].Name.Should().Be("Vary");
            context.Response.Headers[2].Value.Should().Be("Origin");
        }

        [Fact]
        public async void pipeline_cors___returns_configured_access_expose_headers()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" },
                        ExposeHeaders = new string[] { "X-API1", "X-API2", "Content-Type" }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Headers.Should().NotBeEmpty();
            context.Response.Headers.Should().HaveCount(4);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.Response.Headers[0].Value.Should().Be(origin);
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.Response.Headers[1].Value.Should().Be("false");
            context.Response.Headers[2].Name.Should().Be("Access-Control-Expose-Headers");
            context.Response.Headers[2].Value.Should().Be("X-API1, X-API2, Content-Type");
            context.Response.Headers[3].Name.Should().Be("Vary");
            context.Response.Headers[3].Value.Should().Be("Origin");
        }

        [Fact]
        public async void pipeline_cors___doesnt_return_expose_headers_when_empty()
        {
            var origin = "http://ron.vecchi.net";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" },
                        ExposeHeaders = new string[] { }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Headers.Should().NotBeEmpty();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.Response.Headers[0].Value.Should().Be(origin);
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.Response.Headers[1].Value.Should().Be("false");
            context.Response.Headers[2].Name.Should().Be("Vary");
            context.Response.Headers[2].Value.Should().Be("Origin");
        }
    }
}
