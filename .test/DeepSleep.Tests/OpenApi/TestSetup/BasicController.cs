namespace DeepSleep.Tests.OpenApi.TestSetup
{
    using DeepSleep.OpenApi.Decorators;
    using System;
    using System.Threading.Tasks;

    public class BasicController
    {
        [OasResponse("204", typeof(void))]
        [OasResponse("200", typeof(ComplexClassModel))]
        public Task<ComplexClassModel> EndpointNoParams()
        {
            throw new NotImplementedException();
        }

        [OasResponse("204", typeof(void))]
        [OasResponse("200", typeof(ComplexClassModel))]
        public Task<ComplexClassModel> EndpointNoParamsPatch()
        {
            throw new NotImplementedException();
        }

        [OasResponse("200", typeof(ComplexClassModel))]
        public Task<ComplexClassModel> EndpointWithRouteParam([UriBound] ComplexRequest request)
        {
            throw new NotImplementedException();
        }

        [OasResponse("200", typeof(ComplexClassModel))]
        public Task<ComplexClassModel> EndpointWithBodyParam([BodyBound] ComplexResponse request)
        {
            throw new NotImplementedException();
        }
    }
}
