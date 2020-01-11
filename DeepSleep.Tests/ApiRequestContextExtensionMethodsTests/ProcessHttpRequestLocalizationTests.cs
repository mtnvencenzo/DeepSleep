namespace DeepSleep.Tests.ApiRequestContextExtensionMethodsTests
{
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using System;
    using System.Globalization;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpRequestLocalizationTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var set = new CultureInfo("es");
            CultureInfo.CurrentCulture = set;
            CultureInfo.CurrentUICulture = set;

            var currentCultureCode = CultureInfo.CurrentCulture.Name;
            var currentUiCultureCode = CultureInfo.CurrentUICulture.Name;

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestLocalization().ConfigureAwait(false);
            processed.Should().BeFalse();

            CultureInfo.CurrentCulture.Name.Should().Be(currentCultureCode);
            CultureInfo.CurrentUICulture.Name.Should().Be(currentUiCultureCode);
        }

        [Fact]
        public async void ReturnsEnUsForUnConfiguredFallbackLanguage()
        {
            var set = new CultureInfo("es");
            CultureInfo.CurrentCulture = set;
            CultureInfo.CurrentUICulture = set;

            var context = new ApiRequestContext();
            var processed = await context.ProcessHttpRequestLocalization().ConfigureAwait(false);

            processed.Should().BeTrue();
            context.RequestInfo.AcceptCulture.Name.Should().Be("es");
        }

        [Theory]
        [InlineData("en-GB", "en-GB", "en,de,es,es-ES,en-IE,", "ru;q=1.0,fr-FR;q=0.9,ka-GE;q=0.1")]
        [InlineData("en-IE", "en-GB", "en,de,es,es-ES,en-IE,", "ru;q=1.0,fr-FR;q=0.9,ka-GE;q=0.1,en-IE;q=0.1")]
        [InlineData("ka-GE", "en-GB", "en,de,es,es-ES,en-IE,ka-GE", "ru;q=1.0,es;q=0.1,fr-FR;q=0.9,ka-GE;q=0.2,en-IE;q=0.1")]
        [InlineData("en-GB", "en-GB", "en,de,es,es-ES,en-IE,ka-GE", "")]
        [InlineData("en-GB", "en-GB", "en,de,es,es-ES,en-IE,ka-GE", " ")]
        [InlineData("en-GB", "en-GB", "en,de,es,es-ES,en-IE,ka-GE", null)]
        [InlineData("en-GB", "en-GB", "", null)]
        [InlineData("en-GB", "en-GB", " ", null)]
        [InlineData("en-GB", "en-GB", null, null)]
        [InlineData("en", "en-US", "en", "en;q=1.0,en-US;q=0.9")]
        [InlineData("en", "en-US", "en", "en;q=1.0,en-US;q=1.0")]
        [InlineData("en", "en-US", "en", "en;q=0.1,en-US;q=1.0")]
        [InlineData("en", "en-US", "en,en-US", "en;q=1.0,en-US;q=1.0")]
        [InlineData("en", "zh-ZH", "en,de,de-DE", "en-GB;q=1.0,en-US")]
        public async void ReturnsCorrectSupportedForUnConfiguredFallbackLanguage(string expected, string fallBackLanguage, string supported, string acceptable)
        {
            var context = new ApiRequestContext
            {
                RouteInfo = new ApiRoutingInfo
                {
                    RoutingItem = new ApiRoutingItem
                    {
                        Config = new ApiResourceConfig
                        {
                            FallBackLanguage = fallBackLanguage,
                            SupportedLanguages = supported?.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        }
                    }
                },
                RequestInfo = new ApiRequestInfo
                {
                    AcceptCulture = new CultureInfo("ru"),
                    AcceptLanguage = acceptable
                }
            };


            var processed = await context.ProcessHttpRequestLocalization().ConfigureAwait(false);

            processed.Should().BeTrue();
            context.RequestInfo.AcceptCulture.Name.Should().Be(expected);
        }
    }
}
