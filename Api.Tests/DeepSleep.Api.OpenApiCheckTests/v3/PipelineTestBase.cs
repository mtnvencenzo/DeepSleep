namespace DeepSleep.Api.OpenApiCheckTests.v3
{
    using DeepSleep.Api.OpenApiCheckTests.Mocks;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;

    public abstract class PipelineTestBase : TestBase
    {
        protected DeepSleepApi GetClient()
        {
            SetupEnvironment();

            var messageHandler = new MockHttpMessageHandler(GetResponseMessageTask);

            var httpClient = new HttpClient(messageHandler);

            var client = typeof(DeepSleepApi)
                .GetConstructors(bindingAttr: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(c => c.GetParameters().Length == 2)
                .Where(c => c.GetParameters().First().ParameterType == typeof(HttpClient))
                .FirstOrDefault()
                ?.Invoke(new object[] { httpClient, true }) as DeepSleepApi;

            return client;
        }
    }
}
