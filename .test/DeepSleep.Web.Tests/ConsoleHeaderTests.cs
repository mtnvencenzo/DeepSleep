namespace DeepSleep.Web.Tests
{
    using DeepSleep.Discovery;
    using Xunit;

    public class ConsoleHeaderTests
    {
        [Fact]
        public void console___writes_header()
        {
            var routingTable = new ApiRoutingTable();

            routingTable.AddRoute(new DeepSleepRouteRegistration(
                template: "/myroute/{test}/other/{test2}/id",
                httpMethods: new[] { "GET" },
                controller: typeof(TestController),
                endpoint: nameof(TestController.Get)));

            ApiCoreHttpExtensionMethods.WriteDeepSleepToConsole(routingTable: routingTable, null);
        }

        [Fact]
        public void console___writes_may_fourth()
        {
            ApiCoreHttpExtensionMethods.MayTheFourth();
        }
    }
}
