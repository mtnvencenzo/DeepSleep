namespace Api.DeepSleep.Controllers.HelperResponses
{
    using global::DeepSleep;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class HelperResponsesController
    {
        // Ok Responses
        // ------------

        public IApiResponse Ok()
        {
            var response = ApiResponse.Ok(value: new HelperResponseModel());

            return response;
        }

        public Task<IApiResponse> Ok_Null()
        {
            var response = ApiResponse.Ok();

            return Task.FromResult(response);
        }

        public async Task<IApiResponse> Ok_Headers()
        {
            await Awaiter();

            var response = ApiResponse.Ok(value: new HelperResponseModel(), headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // Created Responses
        // ------------

        public IApiResponse Created()
        {
            var response = ApiResponse.Created(value: new HelperResponseModel());

            return response;
        }

        public Task<IApiResponse> Created_Null()
        {
            var response = ApiResponse.Created();

            return Task.FromResult(response);
        }

        public async Task<IApiResponse> Created_Headers()
        {
            await Awaiter();

            var response = ApiResponse.Created(value: new HelperResponseModel(), headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // Accepted Responses
        // ------------

        public IApiResponse Accepted()
        {
            var response = ApiResponse.Accepted(value: new HelperResponseModel());

            return response;
        }

        public Task<IApiResponse> Accepted_Null()
        {
            var response = ApiResponse.Accepted(location: "/test/location");

            return Task.FromResult(response);
        }

        public async Task<IApiResponse> Accepted_Headers()
        {
            await Awaiter();

            var response = ApiResponse.Accepted(location: "/test/location", headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // No Content Responses
        // ------------

        public IApiResponse NoContent()
        {
            var response = ApiResponse.NoContent();

            return response;
        }

        public async Task<IApiResponse> NoContent_Headers()
        {
            await Awaiter();

            var response = ApiResponse.NoContent(headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }


        // Bad Request Responses
        // ------------

        public IApiResponse BadRequest()
        {
            var response = ApiResponse.BadRequest(value: new HelperResponseModel());

            return response;
        }

        public Task<IApiResponse> BadRequest_Null()
        {
            var response = ApiResponse.BadRequest();

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/badrequest/null/with/errors")]
        public Task<IApiResponse> BadRequest_Null_WithErrors()
        {
            var response = ApiResponse.BadRequest(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        public async Task<IApiResponse> BadRequest_Headers()
        {
            await Awaiter();

            var response = ApiResponse.BadRequest(value: new HelperResponseModel(), headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // Unauthorized Responses
        // ------------

        public IApiResponse Unauthorized()
        {
            var response = ApiResponse.Unauthorized(value: new HelperResponseModel());

            return response;
        }

        public Task<IApiResponse> Unauthorized_Null()
        {
            var response = ApiResponse.Unauthorized();

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/unauthorized/null/with/errors")]
        public Task<IApiResponse> Unauthorized_Null_WithErrors()
        {
            var response = ApiResponse.Unauthorized(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        public async Task<IApiResponse> Unauthorized_Headers()
        {
            await Awaiter();

            var response = ApiResponse.Unauthorized(value: new HelperResponseModel(), headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // Forbidden Responses
        // ------------

        public IApiResponse Forbidden()
        {
            var response = ApiResponse.Forbidden(value: new HelperResponseModel());

            return response;
        }

        public Task<IApiResponse> Forbidden_Null()
        {
            var response = ApiResponse.Forbidden();

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/forbidden/null/with/errors")]
        public Task<IApiResponse> Forbidden_Null_WithErrors()
        {
            var response = ApiResponse.Forbidden(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        public async Task<IApiResponse> Forbidden_Headers()
        {
            await Awaiter();

            var response = ApiResponse.Forbidden(value: new HelperResponseModel(), headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // Not Found Responses
        // ------------

        public IApiResponse NotFound()
        {
            var response = ApiResponse.NotFound(value: new HelperResponseModel());

            return response;
        }

        public Task<IApiResponse> NotFound_Null()
        {
            var response = ApiResponse.NotFound();

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/notfound/null/with/errors")]
        public Task<IApiResponse> NotFound_Null_WithErrors()
        {
            var response = ApiResponse.NotFound(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        public async Task<IApiResponse> NotFound_Headers()
        {
            await Awaiter();

            var response = ApiResponse.NotFound(value: new HelperResponseModel(), headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // Conflict Responses
        // ------------

        public IApiResponse Conflict()
        {
            var response = ApiResponse.Conflict(value: new HelperResponseModel());

            return response;
        }

        public Task<IApiResponse> Conflict_Null()
        {
            var response = ApiResponse.Conflict();

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/conflict/null/with/errors")]
        public Task<IApiResponse> Conflict_Null_WithErrors()
        {
            var response = ApiResponse.Conflict(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        public async Task<IApiResponse> Conflict_Headers()
        {
            await Awaiter();

            var response = ApiResponse.Conflict(value: new HelperResponseModel(), headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // Moved Permanently Responses
        // ------------

        public IApiResponse MovedPermanently()
        {
            var response = ApiResponse.MovedPermanently(location: "/test/location");

            return response;
        }

        public async Task<IApiResponse> MovedPermanently_Headers()
        {
            await Awaiter();

            var response = ApiResponse.MovedPermanently(location: "/test/location", headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // Found Responses
        // ------------

        public IApiResponse Found()
        {
            var response = ApiResponse.Found(location: "/test/location");

            return response;
        }

        public async Task<IApiResponse> Found_Headers()
        {
            await Awaiter();

            var response = ApiResponse.Found(location: "/test/location", headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // Permanent Redirect Responses
        // ------------

        public IApiResponse PermanentRedirect()
        {
            var response = ApiResponse.PermanentRedirect(location: "/test/location");

            return response;
        }

        public async Task<IApiResponse> PermanentRedirect_Headers()
        {
            await Awaiter();

            var response = ApiResponse.PermanentRedirect(location: "/test/location", headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // Temporary Redirect Responses
        // ------------

        public IApiResponse TemporaryRedirect()
        {
            var response = ApiResponse.TemporaryRedirect(location: "/test/location");

            return response;
        }

        public async Task<IApiResponse> TemporaryRedirect_Headers()
        {
            await Awaiter();

            var response = ApiResponse.TemporaryRedirect(location: "/test/location", headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // UnhandledException Responses
        // ------------

        [ApiRoute(new[] { "GET" }, "helper/responses/unhandledexception")]
        public IApiResponse UnhandledException()
        {
            var response = ApiResponse.UnhandledException(value: new HelperResponseModel());

            return response;
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/unhandledexception/null")]
        public Task<IApiResponse> UnhandledException_Null()
        {
            var response = ApiResponse.UnhandledException();

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/unhandledexception/null/with/errors")]
        public Task<IApiResponse> UnhandledException_Null_WithErrors()
        {
            var response = ApiResponse.UnhandledException(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/unhandledexception/headers")]
        public async Task<IApiResponse> UnhandledException_Headers()
        {
            await Awaiter();

            var response = ApiResponse.UnhandledException(value: new HelperResponseModel(), headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // NotImplemented Responses
        // ------------

        [ApiRoute(new[] { "GET" }, "helper/responses/notimplemented")]
        public IApiResponse NotImplemented()
        {
            var response = ApiResponse.NotImplemented(value: new HelperResponseModel());

            return response;
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/notimplemented/null")]
        public Task<IApiResponse> NotImplemented_Null()
        {
            var response = ApiResponse.NotImplemented();

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/notimplemented/null/with/errors")]
        public Task<IApiResponse> NotImplemented_Null_WithErrors()
        {
            var response = ApiResponse.NotImplemented(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/notimplemented/headers")]
        public async Task<IApiResponse> NotImplemented_Headers()
        {
            await Awaiter();

            var response = ApiResponse.NotImplemented(value: new HelperResponseModel(), headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // BadGateway Responses
        // ------------

        [ApiRoute(new[] { "GET" }, "helper/responses/badgateway")]
        public IApiResponse BadGateway()
        {
            var response = ApiResponse.BadGateway(value: new HelperResponseModel());

            return response;
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/badgateway/null")]
        public Task<IApiResponse> BadGateway_Null()
        {
            var response = ApiResponse.BadGateway();

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/badgateway/null/with/errors")]
        public Task<IApiResponse> BadGateway_Null_WithErrors()
        {
            var response = ApiResponse.BadGateway(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/badgateway/headers")]
        public async Task<IApiResponse> BadGateway_Headers()
        {
            await Awaiter();

            var response = ApiResponse.BadGateway(value: new HelperResponseModel(), headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // ServiceUnavailable Responses
        // ------------

        [ApiRoute(new[] { "GET" }, "helper/responses/serviceunavailable")]
        public IApiResponse ServiceUnavailable()
        {
            var response = ApiResponse.ServiceUnavailable(value: new HelperResponseModel());

            return response;
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/serviceunavailable/null")]
        public Task<IApiResponse> ServiceUnavailable_Null()
        {
            var response = ApiResponse.ServiceUnavailable();

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/serviceunavailable/null/with/errors")]
        public Task<IApiResponse> ServiceUnavailable_Null_WithErrors()
        {
            var response = ApiResponse.ServiceUnavailable(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/serviceunavailable/headers")]
        public async Task<IApiResponse> ServiceUnavailable_Headers()
        {
            await Awaiter();

            var response = ApiResponse.ServiceUnavailable(value: new HelperResponseModel(), headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        // GatewayTimeout Responses
        // ------------

        [ApiRoute(new[] { "GET" }, "helper/responses/gatewaytimeout")]
        public IApiResponse ServiceTimeout()
        {
            var response = ApiResponse.GatewayTimeout(value: new HelperResponseModel());

            return response;
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/gatewaytimeout/null")]
        public Task<IApiResponse> GatewayTimeout_Null()
        {
            var response = ApiResponse.GatewayTimeout();

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/gatewaytimeout/null/with/errors")]
        public Task<IApiResponse> GatewayTimeout_Null_WithErrors()
        {
            var response = ApiResponse.GatewayTimeout(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        [ApiRoute(new[] { "GET" }, "helper/responses/gatewaytimeout/headers")]
        public async Task<IApiResponse> GatewayTimeout_Headers()
        {
            await Awaiter();

            var response = ApiResponse.GatewayTimeout(value: new HelperResponseModel(), headers: new List<ApiHeader>
            {
                new ApiHeader("Test", "Value")
            });

            return response;
        }

        private Task Awaiter()
        {
            return Task.CompletedTask;
        }
    }

    public class HelperResponseModel
    {
        public string Value => "Test";
    }
}
