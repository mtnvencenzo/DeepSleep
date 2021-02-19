namespace DeepSleep.Tests.Auth
{
    using DeepSleep.Auth;
    using FluentAssertions;
    using System;
    using Xunit;

    public class ApiAuthorizationAttributeTests
    {
        [Fact]
        public void apiauthorizationattribute___ctor_thorws_on_null_provider_type()
        {
            var exception = Assert.Throws<ArgumentNullException>("authorizationProviderType", () =>
            {
                var attribute = new ApiAuthorizationAttribute(null);
            });
        }

        [Fact]
        public void apiauthorizationattribute___ctor_thorws_on_provider_type_not_implmenting_correct_interface()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var attribute = new ApiAuthorizationAttribute(typeof(int));
            });

            exception.Message.Should().StartWith("authorizationProviderType must implement interface");
        }
    }
}
