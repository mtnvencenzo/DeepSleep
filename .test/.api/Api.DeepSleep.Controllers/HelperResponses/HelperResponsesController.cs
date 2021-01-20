namespace Api.DeepSleep.Controllers.HelperResponses
{
    using global::DeepSleep;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class HelperResponsesController
    {
        // Ok Responses
        // ------------

        /// <summary>Oks this instance.</summary>
        /// <returns></returns>
        public IApiResponse Ok()
        {
            var response = ApiResponse.Ok(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Oks the null.</summary>
        /// <returns></returns>
        public Task<IApiResponse> Ok_Null()
        {
            var response = ApiResponse.Ok();

            return Task.FromResult(response);
        }

        /// <summary>Oks the headers.</summary>
        /// <returns></returns>
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

        /// <summary>Createds this instance.</summary>
        /// <returns></returns>
        public IApiResponse Created()
        {
            var response = ApiResponse.Created(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Createds the null.</summary>
        /// <returns></returns>
        public Task<IApiResponse> Created_Null()
        {
            var response = ApiResponse.Created();

            return Task.FromResult(response);
        }

        /// <summary>Createds the headers.</summary>
        /// <returns></returns>
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

        /// <summary>Accepteds this instance.</summary>
        /// <returns></returns>
        public IApiResponse Accepted()
        {
            var response = ApiResponse.Accepted(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Accepteds the null.</summary>
        /// <returns></returns>
        public Task<IApiResponse> Accepted_Null()
        {
            var response = ApiResponse.Accepted(location: "/test/location");

            return Task.FromResult(response);
        }

        /// <summary>Accepteds the headers.</summary>
        /// <returns></returns>
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

        /// <summary>Noes the content.</summary>
        /// <returns></returns>
        public IApiResponse NoContent()
        {
            var response = ApiResponse.NoContent();

            return response;
        }

        /// <summary>Noes the content headers.</summary>
        /// <returns></returns>
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

        /// <summary>Bads the request.</summary>
        /// <returns></returns>
        public IApiResponse BadRequest()
        {
            var response = ApiResponse.BadRequest(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Bads the request null.</summary>
        /// <returns></returns>
        public Task<IApiResponse> BadRequest_Null()
        {
            var response = ApiResponse.BadRequest();

            return Task.FromResult(response);
        }

        /// <summary>Bads the request null with errors.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/badrequest/null/with/errors")]
        public Task<IApiResponse> BadRequest_Null_WithErrors()
        {
            var response = ApiResponse.BadRequest(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        /// <summary>Bads the request headers.</summary>
        /// <returns></returns>
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

        /// <summary>Unauthorizeds this instance.</summary>
        /// <returns></returns>
        public IApiResponse Unauthorized()
        {
            var response = ApiResponse.Unauthorized(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Unauthorizeds the null.</summary>
        /// <returns></returns>
        public Task<IApiResponse> Unauthorized_Null()
        {
            var response = ApiResponse.Unauthorized();

            return Task.FromResult(response);
        }

        /// <summary>Unauthorizeds the null with errors.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/unauthorized/null/with/errors")]
        public Task<IApiResponse> Unauthorized_Null_WithErrors()
        {
            var response = ApiResponse.Unauthorized(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        /// <summary>Unauthorizeds the headers.</summary>
        /// <returns></returns>
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

        /// <summary>Forbiddens this instance.</summary>
        /// <returns></returns>
        public IApiResponse Forbidden()
        {
            var response = ApiResponse.Forbidden(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Forbiddens the null.</summary>
        /// <returns></returns>
        public Task<IApiResponse> Forbidden_Null()
        {
            var response = ApiResponse.Forbidden();

            return Task.FromResult(response);
        }

        /// <summary>Forbiddens the null with errors.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/forbidden/null/with/errors")]
        public Task<IApiResponse> Forbidden_Null_WithErrors()
        {
            var response = ApiResponse.Forbidden(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        /// <summary>Forbiddens the headers.</summary>
        /// <returns></returns>
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

        /// <summary>Nots the found.</summary>
        /// <returns></returns>
        public IApiResponse NotFound()
        {
            var response = ApiResponse.NotFound(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Nots the found null.</summary>
        /// <returns></returns>
        public Task<IApiResponse> NotFound_Null()
        {
            var response = ApiResponse.NotFound();

            return Task.FromResult(response);
        }

        /// <summary>Nots the found null with errors.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/notfound/null/with/errors")]
        public Task<IApiResponse> NotFound_Null_WithErrors()
        {
            var response = ApiResponse.NotFound(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        /// <summary>Nots the found headers.</summary>
        /// <returns></returns>
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

        /// <summary>Conflicts this instance.</summary>
        /// <returns></returns>
        public IApiResponse Conflict()
        {
            var response = ApiResponse.Conflict(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Conflicts the null.</summary>
        /// <returns></returns>
        public Task<IApiResponse> Conflict_Null()
        {
            var response = ApiResponse.Conflict();

            return Task.FromResult(response);
        }

        /// <summary>Conflicts the null with errors.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/conflict/null/with/errors")]
        public Task<IApiResponse> Conflict_Null_WithErrors()
        {
            var response = ApiResponse.Conflict(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        /// <summary>Conflicts the headers.</summary>
        /// <returns></returns>
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

        /// <summary>Moveds the permanently.</summary>
        /// <returns></returns>
        public IApiResponse MovedPermanently()
        {
            var response = ApiResponse.MovedPermanently(location: "/test/location");

            return response;
        }

        /// <summary>Moveds the permanently headers.</summary>
        /// <returns></returns>
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

        /// <summary>Founds this instance.</summary>
        /// <returns></returns>
        public IApiResponse Found()
        {
            var response = ApiResponse.Found(location: "/test/location");

            return response;
        }

        /// <summary>Founds the headers.</summary>
        /// <returns></returns>
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

        /// <summary>Permanents the redirect.</summary>
        /// <returns></returns>
        public IApiResponse PermanentRedirect()
        {
            var response = ApiResponse.PermanentRedirect(location: "/test/location");

            return response;
        }

        /// <summary>Permanents the redirect headers.</summary>
        /// <returns></returns>
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

        /// <summary>Temporaries the redirect.</summary>
        /// <returns></returns>
        public IApiResponse TemporaryRedirect()
        {
            var response = ApiResponse.TemporaryRedirect(location: "/test/location");

            return response;
        }

        /// <summary>Temporaries the redirect headers.</summary>
        /// <returns></returns>
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

        /// <summary>Unhandleds the exception.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/unhandledexception")]
        public IApiResponse UnhandledException()
        {
            var response = ApiResponse.UnhandledException(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Unhandleds the exception null.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/unhandledexception/null")]
        public Task<IApiResponse> UnhandledException_Null()
        {
            var response = ApiResponse.UnhandledException();

            return Task.FromResult(response);
        }

        /// <summary>Unhandleds the exception null with errors.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/unhandledexception/null/with/errors")]
        public Task<IApiResponse> UnhandledException_Null_WithErrors()
        {
            var response = ApiResponse.UnhandledException(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        /// <summary>Unhandleds the exception headers.</summary>
        /// <returns></returns>
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

        /// <summary>Nots the implemented.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/notimplemented")]
        public IApiResponse NotImplemented()
        {
            var response = ApiResponse.NotImplemented(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Nots the implemented null.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/notimplemented/null")]
        public Task<IApiResponse> NotImplemented_Null()
        {
            var response = ApiResponse.NotImplemented();

            return Task.FromResult(response);
        }

        /// <summary>Nots the implemented null with errors.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/notimplemented/null/with/errors")]
        public Task<IApiResponse> NotImplemented_Null_WithErrors()
        {
            var response = ApiResponse.NotImplemented(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        /// <summary>Nots the implemented headers.</summary>
        /// <returns></returns>
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

        /// <summary>Bads the gateway.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/badgateway")]
        public IApiResponse BadGateway()
        {
            var response = ApiResponse.BadGateway(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Bads the gateway null.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/badgateway/null")]
        public Task<IApiResponse> BadGateway_Null()
        {
            var response = ApiResponse.BadGateway();

            return Task.FromResult(response);
        }

        /// <summary>Bads the gateway null with errors.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/badgateway/null/with/errors")]
        public Task<IApiResponse> BadGateway_Null_WithErrors()
        {
            var response = ApiResponse.BadGateway(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        /// <summary>Bads the gateway headers.</summary>
        /// <returns></returns>
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

        /// <summary>Services the unavailable.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/serviceunavailable")]
        public IApiResponse ServiceUnavailable()
        {
            var response = ApiResponse.ServiceUnavailable(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Services the unavailable null.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/serviceunavailable/null")]
        public Task<IApiResponse> ServiceUnavailable_Null()
        {
            var response = ApiResponse.ServiceUnavailable();

            return Task.FromResult(response);
        }

        /// <summary>Services the unavailable null with errors.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/serviceunavailable/null/with/errors")]
        public Task<IApiResponse> ServiceUnavailable_Null_WithErrors()
        {
            var response = ApiResponse.ServiceUnavailable(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        /// <summary>Services the unavailable headers.</summary>
        /// <returns></returns>
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

        /// <summary>Services the timeout.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/gatewaytimeout")]
        public IApiResponse ServiceTimeout()
        {
            var response = ApiResponse.GatewayTimeout(value: new HelperResponseModel());

            return response;
        }

        /// <summary>Gateways the timeout null.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/gatewaytimeout/null")]
        public Task<IApiResponse> GatewayTimeout_Null()
        {
            var response = ApiResponse.GatewayTimeout();

            return Task.FromResult(response);
        }

        /// <summary>Gateways the timeout null with errors.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "helper/responses/gatewaytimeout/null/with/errors")]
        public Task<IApiResponse> GatewayTimeout_Null_WithErrors()
        {
            var response = ApiResponse.GatewayTimeout(errors: new string[] { "Test-Error" });

            return Task.FromResult(response);
        }

        /// <summary>Gateways the timeout headers.</summary>
        /// <returns></returns>
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

        /// <summary>Awaiters this instance.</summary>
        /// <returns></returns>
        private Task Awaiter()
        {
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class HelperResponseModel
    {
        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public string Value => "Test";
    }
}
