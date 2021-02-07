namespace DeepSleep.Tests
{
    using System;
    using Xunit;

    public class ApiRouteAttributeTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("", null, " ")]
        public void apiRouteAttribute___throws_for_missing_httpmethods(params string[] httpMethods)
        {
            Assert.Throws<ArgumentException>("httpMethods", () => new ApiRouteAttribute(httpMethods, "test"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void apiRouteAttribute___throws_for_missing_template(string template)
        {
            Assert.Throws<ArgumentException>("template", () => new ApiRouteAttribute(new[] { "GET" }, template));
        }
    }
}
