namespace DeepSleep.NetCore.Tests
{
    using DeepSleep.Discovery;
    using Xunit;

    public class ConsoleHeaderTests
    {
        [Fact]
        public void console___writes_header()
        {
            var routingTable = new DefaultApiRoutingTable();

            routingTable.AddRoute(new ApiRouteRegistration(
                template: "/myroute/{test}/other/{test2}/id",
                httpMethods: new[] { "GET" },
                controller: typeof(TestController),
                endpoint: nameof(TestController.Get)));

            ApiCoreHttpExtensionMethods.WriteDeepsleepToConsole(routingTable: routingTable);
        }

        [Fact]
        public void console___writes_may_fourth()
        {
            ApiCoreHttpExtensionMethods.MayTheFourth();
        }
    }
}