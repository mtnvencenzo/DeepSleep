namespace DeepSleep.Tests
{
    using FluentAssertions;
    using Xunit;

    public class ApiRoutingTemplateTests
    {
        [Theory]
        [InlineData("/test/{MyVar1}/{myvar2}/test/some/{myVar3}/test/{myVar4}")]
        [InlineData("/{MyVar1}/{my var2}/test/some/{myVar3}/test/{myVar4}/")]
        [InlineData("{MyVar1}/{myvar2}/test/some/{myVar3}/test/{myVar4}/")]
        [InlineData("{MyVar1}/{myvar2}/{myVar3}/{myVar4}")]
        public void api_routing_template___ctor_builds_up_variables(string template)
        {
            var t = new ApiRoutingTemplate(template);

            t.Variables.Should().NotBeNull();
            t.Variables.Should().HaveCount(4);
            t.Variables[0].Should().Be("MyVar1");
            t.Variables[1].Should().Be("myvar2");
            t.Variables[2].Should().Be("myVar3");
            t.Variables[3].Should().Be("myVar4");
        }

        [Theory]
        [InlineData("/test/{MyVar1}s/myvar2}/test/some/s{myVar3}/test{myVar4}")]
        [InlineData("/test/{MyVar1{/myvar2}/test/some/{myV{ar3}/test{myV}ar4}")]
        [InlineData("/test/}{MyVar1{/myvar2}/test/some/{my}Var3}/test{}myVar4}")]
        [InlineData("{")]
        [InlineData("}")]
        [InlineData("/test/{My}Var1/test")]
        [InlineData("/test/|MyV{ar1/test")]
        [InlineData("/{}/{}/")]
        public void api_routing_template___ctor_doesnt_fail_on_weird_templates(string template)
        {
            var t = new ApiRoutingTemplate(template);
            t.Variables.Should().NotBeNull();
            t.Variables.Should().BeEmpty();
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public void api_routing_template___ctor_empty_template_gives_empty_variables(string template)
        {
            var t = new ApiRoutingTemplate(template);

            t.Variables.Should().NotBeNull();
            t.Variables.Should().HaveCount(0);
        }
    }
}
