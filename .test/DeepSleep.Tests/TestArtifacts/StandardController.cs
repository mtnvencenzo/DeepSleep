namespace DeepSleep.Tests.TestArtifacts
{
    using System.Threading.Tasks;

    public class StandardController
    {
        public void DefaultEndpoint()
        {
        }

        public Task DefaultTaskEndpoint()
        {
            return Task.CompletedTask;
        }

        public Task<int> DefaultGenericTaskEndpoint()
        {
            return Task.FromResult(100);
        }

        public Task<int> DefaultGenericTaskWithFullApiResponseEndpoint()
        {
            return Task.FromResult(100);
        }

        public int DefaultFullApiResponseEndpoint()
        {
            return 200;
        }

        public int DefaultFullApiResponseEndpointWithUriParameterAndBodyParameterNotAttributed([UriBound] StandardModel body, [BodyBound] StandardNullableModel uri)
        {
            return uri.IntProp ?? 0;
        }

        public int? DefaultFullApiResponseEndpointWithUriParameterAndBodyParameterAttributed([UriBound] StandardModel myUri, [BodyBound] StandardNullableModel myBody)
        {

            return myBody.IntProp;
        }

        public int? DefaultFullApiResponseEndpointWithUriParameterAndBodyParameterAndExtraParameters(int myInt, [UriBound] StandardModel uri, [BodyBound] StandardNullableModel body, int? myNullableInt, StandardEnum? myNullableEnum, StandardEnum myEnum)
        {
            return body.IntProp;
        }
    }
}
