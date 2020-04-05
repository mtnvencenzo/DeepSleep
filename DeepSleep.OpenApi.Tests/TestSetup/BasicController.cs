namespace DeepSleep.OpenApi.Tests.TestSetup
{
    using DeepSleep.OpenApi.Decorators;
    using System;
    using System.Threading.Tasks;

    public class BasicController
    {
        [OpenApiResponse("204", typeof(void))]
        [OpenApiResponse("200", typeof(ComplexClassModel))]
        public Task<ApiResponse> EndpointNoParams()
        {
            throw new NotImplementedException();
        }

        [OpenApiResponse("204", typeof(void))]
        [OpenApiResponse("200", typeof(ComplexClassModel))]
        public Task<ApiResponse> EndpointNoParamsPatch()
        {
            throw new NotImplementedException();
        }

        [OpenApiResponse("200", typeof(ComplexClassModel))]
        public Task<ApiResponse> EndpointWithRouteParam([UriBound] ComplexRequest request)
        {
            throw new NotImplementedException();
        }

        [OpenApiResponse("200", typeof(ComplexClassModel))]
        public Task<ApiResponse> EndpointWithBodyParam([BodyBound] ComplexResponse request)
        {
            throw new NotImplementedException();
        }
    }
}
