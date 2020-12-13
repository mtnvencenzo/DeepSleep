namespace DeepSleep.NetCore.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using DeepSleep.NetCore;
    using Xunit;
    using System.Linq;
    using DeepSleep.Configuration;

    public class ApiCoreHttpExtensionMethodsTest
    {
        [Fact]
        public void ApiCoreHttpExtensionMethods_AddsPingRoute_ByDefault()
        {
            IServiceCollection services = new ServiceCollection();

            var config = new DefaultApiServiceConfiguration();
            services.UseApiCoreServices(config);

            var provider = services.BuildServiceProvider();
            Assert.NotNull(provider);

            var table = provider.GetService<IApiRoutingTable>();
            Assert.NotNull(table);

            var routes = table.GetRoutes().ToList();
            var route = routes.FirstOrDefault(r => r.Template == "ping");

            Assert.NotNull(route);
        }

        [Fact]
        public void ApiCoreHttpExtensionMethods_DoesNotAddPingRoute()
        {
            IServiceCollection services = new ServiceCollection();

            var config = new DefaultApiServiceConfiguration
            {
                UsePingEndpoint = false
            };

            services.UseApiCoreServices(config);

            var provider = services.BuildServiceProvider();
            Assert.NotNull(provider);

            var table = provider.GetService<IApiRoutingTable>();
            Assert.NotNull(table);

            var routes = table.GetRoutes().ToList();
            var route = routes.FirstOrDefault(r => r.Template == "ping");

            Assert.Null(route);
        }
    }
}
