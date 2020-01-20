namespace DeepSleep.Tests.TestArtifacts
{
    using System.Collections.Generic;
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

        public Task<ApiResponse> DefaultGenericTaskWithFullApiResponseEndpoint()
        {
            return Task.FromResult(new ApiResponse
            {
                StatusCode = 201,
                Body = 100,
                Headers = new List<ApiHeader>
                {
                    new ApiHeader{ Name = "X-MyHeader1", Value = "MyHeaderValue1" }
                }
            });
        }

        public ApiResponse DefaultFullApiResponseEndpoint()
        {
            return new ApiResponse
            {
                StatusCode = 201,
                Body = 200,
                Headers = new List<ApiHeader>
                {
                    new ApiHeader{ Name = "X-MyHeader2", Value = "MyHeaderValue2" }
                }
            };
        }

        public ApiResponse DefaultFullApiResponseEndpointWithUriParameterAndBodyParameterNotAttributed([UriBound] StandardModel body, [BodyBound] StandardNullableModel uri)
        {
            return new ApiResponse
            {
                StatusCode = body.IntProp,
                Body = uri.IntProp ?? 0,
                Headers = new List<ApiHeader>
                {
                    new ApiHeader{ Name = "X-MyHeader2", Value = "MyHeaderValue2" }
                }
            };
        }

        public ApiResponse DefaultFullApiResponseEndpointWithUriParameterAndBodyParameterAttributed([UriBound] StandardModel myUri, [BodyBound] StandardNullableModel myBody)
        {
            return new ApiResponse
            {
                StatusCode = myUri.IntProp,
                Body = myBody.IntProp,
                Headers = new List<ApiHeader>
                {
                    new ApiHeader{ Name = "X-MyHeader2", Value = "MyHeaderValue2" }
                }
            };
        }

        public ApiResponse DefaultFullApiResponseEndpointWithUriParameterAndBodyParameterAndExtraParameters(int myInt, [UriBound] StandardModel uri, [BodyBound] StandardNullableModel body, int? myNullableInt, StandardEnum? myNullableEnum, StandardEnum myEnum)
        {
            return new ApiResponse
            {
                StatusCode = uri.IntProp,
                Body = body.IntProp,
                Headers = new List<ApiHeader>
                {
                    new ApiHeader{ Name = "X-MyHeader2", Value = "MyHeaderValue2" }
                }
            };
        }
    }
}
