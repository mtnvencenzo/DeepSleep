namespace DeepSleep.Tests.ApiRequestContextExtensionMethodsTests
{
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Moq;
    using System;
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

            var configResolver = new Mock<ICrossOriginConfigResolver>();
            configResolver
                .Setup(x => x.ResolveConfig())
                .Throws<ArgumentException>();

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing(configResolver.Object).ConfigureAwait(false);
            processed.Should().BeFalse();
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

            var configResolver = new Mock<ICrossOriginConfigResolver>();
            configResolver
                .Setup(x => x.ResolveConfig())
                .Throws<ArgumentException>();

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing(configResolver.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
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
                }
            };

            var configResolver = new Mock<ICrossOriginConfigResolver>();
            configResolver
                .Setup(x => x.ResolveConfig())
                .ReturnsAsync(() =>
                {
                    return new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" }
                    };
                });

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing(configResolver.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("true");
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
                ResourceConfig = new ApiResourceConfig
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { origin }
                    }
                }
            };

            var configResolver = new Mock<ICrossOriginConfigResolver>();
            configResolver
                .Setup(x => x.ResolveConfig())
                .ReturnsAsync(() =>
                {
                    return new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" }
                    };
                });

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing(configResolver.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("true");
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
                ResourceConfig = new ApiResourceConfig
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { origin }
                    }
                }
            };

            var configResolver = new Mock<ICrossOriginConfigResolver>();
            configResolver
                .Setup(x => x.ResolveConfig())
                .ReturnsAsync(() =>
                {
                    return new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "http://test1.vecchi.net", "http://test2.vecchi.net" }
                    };
                });

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing(configResolver.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("true");
        }

        [Fact]
        public async void ReturnsOrginWhenMultipleOrginsDefaultConfigured()
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
                ResourceConfig = new ApiResourceConfig
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = null
                    }
                }
            };

            var configResolver = new Mock<ICrossOriginConfigResolver>();
            configResolver
                .Setup(x => x.ResolveConfig())
                .ReturnsAsync(() =>
                {
                    return new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "http://test1.vecchi.net", "http://test2.vecchi.net", origin }
                    };
                });

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing(configResolver.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("true");
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
                ResourceConfig = new ApiResourceConfig
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = null
                    }
                }
            };

            var configResolver = new Mock<ICrossOriginConfigResolver>();
            configResolver
                .Setup(x => x.ResolveConfig())
                .ReturnsAsync(() =>
                {
                    return new CrossOriginConfiguration
                    {
                        AllowedOrigins = null
                    };
                });

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing(configResolver.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(string.Empty);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("true");
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
                ResourceConfig = new ApiResourceConfig
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = null
                    }
                }
            };

            var configResolver = new Mock<ICrossOriginConfigResolver>();
            configResolver
                .Setup(x => x.ResolveConfig())
                .ReturnsAsync(() =>
                {
                    return new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "http://ronnie.vecchi.net", "http://ronn.vecchi.nets" }
                    };
                });

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing(configResolver.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(string.Empty);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("true");
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
                ResourceConfig = new ApiResourceConfig
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "http://ronnie.vecchi.net", "http://ronn.vecchi.nets" }
                    }
                }
            };

            var configResolver = new Mock<ICrossOriginConfigResolver>();
            configResolver
                .Setup(x => x.ResolveConfig())
                .ReturnsAsync(() =>
                {
                    return new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" }
                    };
                });

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing(configResolver.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(string.Empty);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("true");
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
                ResourceConfig = new ApiResourceConfig
                {
                    CrossOriginConfig = new CrossOriginConfiguration
                    {
                        AllowedOrigins = new string[] { "*" }
                    }
                }
            };

            var configResolver = new Mock<ICrossOriginConfigResolver>();
            configResolver
                .Setup(x => x.ResolveConfig())
                .ReturnsAsync(() =>
                {
                    return new CrossOriginConfiguration
                    {
                        AllowedOrigins = null
                    };
                });

            var processed = await context.ProcessHttpResponseCrossOriginResourceSharing(configResolver.Object).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Headers.Should().NotBeEmpty();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Origin");
            context.ResponseInfo.Headers[0].Value.Should().Be(origin);
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Credentials");
            context.ResponseInfo.Headers[1].Value.Should().Be("true");
        }
    }
}
