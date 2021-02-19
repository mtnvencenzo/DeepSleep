namespace DeepSleep.Tests.Auth
{
    using DeepSleep.Auth;
    using FluentAssertions;
    using System;
    using Xunit;

    public class ApiAuthenticationAttributeTests
    {
        [Fact]
        public void apiauthenticationattribute___ctor_thorws_on_null_provider_type()
        {
            var exception = Assert.Throws<ArgumentNullException>("authenticationProviderType", () =>
            {
                var attribute = new ApiAuthenticationAttribute(null);
            });
        }

        [Fact]
        public void apiauthenticationattribute___ctor_thorws_on_provider_type_not_implmenting_correct_interface()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var attribute = new ApiAuthenticationAttribute(typeof(int));
            });

            exception.Message.Should().StartWith("authenticationProviderType must implement interface");
        }
    }
}
