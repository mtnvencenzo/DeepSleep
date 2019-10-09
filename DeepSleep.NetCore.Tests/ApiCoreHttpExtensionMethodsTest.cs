using Microsoft.Extensions.DependencyInjection;
using DeepSleep.NetCore;
using Xunit;
using System.Linq;

namespace DeepSleep.NetCore.Tests
{
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
        public void ApiCoreHttpExtensionMethods_AddsEnvironmentRoute_ByDefault()
        {
            IServiceCollection services = new ServiceCollection();

            var config = new DefaultApiServiceConfiguration();
            services.UseApiCoreServices(config);

            var provider = services.BuildServiceProvider();
            Assert.NotNull(provider);

            var table = provider.GetService<IApiRoutingTable>();
            Assert.NotNull(table);

            var routes = table.GetRoutes().ToList();
            var route = routes.FirstOrDefault(r => r.Template == "env");

            Assert.NotNull(route);
        }

        [Fact]
        public void ApiCoreHttpExtensionMethods_DoesNotAddPingRoute()
        {
            IServiceCollection services = new ServiceCollection();

            var config = new DefaultApiServiceConfiguration();
            config.UsePingEndpoint = false;

            services.UseApiCoreServices(config);

            var provider = services.BuildServiceProvider();
            Assert.NotNull(provider);

            var table = provider.GetService<IApiRoutingTable>();
            Assert.NotNull(table);

            var routes = table.GetRoutes().ToList();
            var route = routes.FirstOrDefault(r => r.Template == "ping");

            Assert.Null(route);
        }

        [Fact]
        public void ApiCoreHttpExtensionMethods_DoesNotAddEnvironmentRoute()
        {
            IServiceCollection services = new ServiceCollection();

            var config = new DefaultApiServiceConfiguration();
            config.UseEnvironmentEndpoint = false;

            services.UseApiCoreServices(config);

            var provider = services.BuildServiceProvider();
            Assert.NotNull(provider);

            var table = provider.GetService<IApiRoutingTable>();
            Assert.NotNull(table);

            var routes = table.GetRoutes().ToList();
            var route = routes.FirstOrDefault(r => r.Template == "env");

            Assert.Null(route);
        }
    }
}
