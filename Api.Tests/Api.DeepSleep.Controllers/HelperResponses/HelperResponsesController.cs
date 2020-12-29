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
