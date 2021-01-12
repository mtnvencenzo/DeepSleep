namespace DeepSleep
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponse : IApiResponse
    {
        /// <summary>Gets or sets the response.</summary>
        /// <value>The response.</value>
        public object Response { get; set; }

        /// <summary>Gets or sets the status code.</summary>
        /// <value>The status code.</value>
        public int StatusCode { get; set; }

        /// <summary>Gets or sets the headers.</summary>
        /// <value>The headers.</value>
        public IList<ApiHeader> Headers { get; set; }


        // Success Responses
        // ----------------


        /// <summary>Oks the specified headers.</summary>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse Ok(IList<ApiHeader> headers = null)
        {
            return Ok(
                value: null as object,
                headers: headers);
        }

        /// <summary>Oks the specified value.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse Ok<T>(T value, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 200,
                value: value,
                headers: headers);
        }

        /// <summary>Createds the specified headers.</summary>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse Created(IList<ApiHeader> headers = null)
        {
            return Created(
                value: null as object,
                headers: headers);
        }

        /// <summary>Createds the specified value.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse Created<T>(T value, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 201,
                value: value,
                headers: headers);
        }

        /// <summary>Accepteds the specified location.</summary>
        /// <param name="location">The location.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse Accepted(string location = null, IList<ApiHeader> headers = null)
        {
            return Accepted(
                value: null as object,
                location: location,
                headers: headers);
        }

        /// <summary>Accepteds the specified value.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="location">The location.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse Accepted<T>(T value, string location = null, IList<ApiHeader> headers = null)
        {
            var headersToUse = new List<ApiHeader>();

            if (headers != null)
            {
                headers.ForEach(h => headersToUse.Add(new ApiHeader(h.Name, h.Value)));
            }

            if (!string.IsNullOrWhiteSpace(location) && !headersToUse.HasHeader("Location"))
            {
                headersToUse.Add(new ApiHeader("Location", location));
            }

            return SettHttp(
                statusCode: 202,
                value: value,
                headers: headersToUse);
        }

        /// <summary>Noes the content.</summary>
        /// <returns></returns>
        public static IApiResponse NoContent()
        {
            return NoContent(
                headers: null);
        }

        /// <summary>Noes the content.</summary>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse NoContent(IList<ApiHeader> headers = null)
        {
            return SettHttp<object>(
                statusCode: 204,
                headers: headers);
        }

        // Bad Responses
        // ----------------

        /// <summary>Bads the request.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static IApiResponse BadRequest(IList<ApiHeader> headers = null, IList<string> errors = null)
        {
            return SettHttp(
                statusCode: 400,
                value: null as object,
                headers: headers,
                errors: errors);
        }

        /// <summary>Bads the request.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse BadRequest<T>(T value, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 400,
                value: value,
                headers: headers);
        }

        /// <summary>Unprocessables the entity.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static IApiResponse UnprocessableEntity(IList<ApiHeader> headers = null, IList<string> errors = null)
        {
            return SettHttp(
                statusCode: 422,
                value: null as object,
                headers: headers,
                errors: errors);
        }

        /// <summary>Unprocessables the entity.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse UnprocessableEntity<T>(T value, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 422,
                value: value,
                headers: headers);
        }

        /// <summary>Unauthorizeds the specified headers.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static IApiResponse Unauthorized(IList<ApiHeader> headers = null, IList<string> errors = null)
        {
            return SettHttp(
                statusCode: 401,
                value: null as object,
                headers: headers,
                errors: errors);
        }

        /// <summary>Unauthorizeds the specified value.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse Unauthorized<T>(T value, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 401,
                value: value,
                headers: headers);
        }

        /// <summary>Forbiddens the specified headers.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static IApiResponse Forbidden(IList<ApiHeader> headers = null, IList<string> errors = null)
        {
            return SettHttp(
                statusCode: 403,
                value: null as object,
                headers: headers,
                errors: errors);
        }

        /// <summary>Forbiddens the specified value.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse Forbidden<T>(T value, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 403,
                value: value,
                headers: headers);
        }

        /// <summary>Nots the found.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static IApiResponse NotFound(IList<ApiHeader> headers = null, IList<string> errors = null)
        {
            return SettHttp(
                statusCode: 404,
                value: null as object,
                headers: headers,
                errors: errors);
        }

        /// <summary>Nots the found.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse NotFound<T>(T value, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 404,
                value: value,
                headers: headers);
        }

        /// <summary>Conflicts the specified headers.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static IApiResponse Conflict(IList<ApiHeader> headers = null, IList<string> errors = null)
        {
            return SettHttp(
                statusCode: 409,
                value: null as object,
                headers: headers,
                errors: errors);
        }

        /// <summary>Conflicts the specified value.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse Conflict<T>(T value = default, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 409,
                value: value,
                headers: headers);
        }


        // Redirect Responses
        // ----------------


        /// <summary>Moveds the permanently.</summary>
        /// <param name="location">The location.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse MovedPermanently(string location, IList<ApiHeader> headers = null)
        {
            var headersToUse = new List<ApiHeader>();

            if (headers != null)
            {
                headers.ForEach(h => headersToUse.Add(new ApiHeader(h.Name, h.Value)));
            }

            if (!string.IsNullOrWhiteSpace(location) && !headersToUse.HasHeader("Location"))
            {
                headersToUse.Add(new ApiHeader("Location", location));
            }

            return SettHttp<object>(
                statusCode: 301,
                headers: headersToUse);
        }

        /// <summary>Founds the specified location.</summary>
        /// <param name="location">The location.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse Found(string location, IList<ApiHeader> headers = null)
        {
            var headersToUse = new List<ApiHeader>();

            if (headers != null)
            {
                headers.ForEach(h => headersToUse.Add(new ApiHeader(h.Name, h.Value)));
            }

            if (!string.IsNullOrWhiteSpace(location) && !headersToUse.HasHeader("Location"))
            {
                headersToUse.Add(new ApiHeader("Location", location));
            }

            return SettHttp<object>(
                statusCode: 302,
                headers: headersToUse);
        }

        /// <summary>Temporaries the redirect.</summary>
        /// <param name="location">The location.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse TemporaryRedirect(string location, IList<ApiHeader> headers = null)
        {
            var headersToUse = new List<ApiHeader>();

            if (headers != null)
            {
                headers.ForEach(h => headersToUse.Add(new ApiHeader(h.Name, h.Value)));
            }

            if (!string.IsNullOrWhiteSpace(location) && !headersToUse.HasHeader("Location"))
            {
                headersToUse.Add(new ApiHeader("Location", location));
            }

            return SettHttp<object>(
                statusCode: 307,
                headers: headersToUse);
        }

        /// <summary>Permanents the redirect.</summary>
        /// <param name="location">The location.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse PermanentRedirect(string location, IList<ApiHeader> headers = null)
        {
            var headersToUse = new List<ApiHeader>();

            if (headers != null)
            {
                headers.ForEach(h => headersToUse.Add(new ApiHeader(h.Name, h.Value)));
            }

            if (!string.IsNullOrWhiteSpace(location) && !headersToUse.HasHeader("Location"))
            {
                headersToUse.Add(new ApiHeader("Location", location));
            }

            return SettHttp<object>(
                statusCode: 308,
                headers: headersToUse);
        }


        // Error Responses
        // ----------------


        /// <summary>Unhandleds the exception.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static IApiResponse UnhandledException(IList<ApiHeader> headers = null, IList<string> errors = null)
        {
            return SettHttp(
                statusCode: 500,
                value: null as object,
                headers: headers,
                errors: errors);
        }

        /// <summary>Unhandleds the exception.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse UnhandledException<T>(T value = default, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 500,
                value: value,
                headers: headers);
        }

        /// <summary>Nots the implemented.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static IApiResponse NotImplemented(IList<ApiHeader> headers = null, IList<string> errors = null)
        {
            return SettHttp(
                statusCode: 501,
                value: null as object,
                headers: headers,
                errors: errors);
        }

        /// <summary>Nots the implemented.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse NotImplemented<T>(T value = default, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 501,
                value: value,
                headers: headers);
        }

        /// <summary>Bads the gateway.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static IApiResponse BadGateway(IList<ApiHeader> headers = null, IList<string> errors = null)
        {
            return SettHttp(
                statusCode: 502,
                value: null as object,
                headers: headers,
                errors: errors);
        }

        /// <summary>Bads the gateway.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse BadGateway<T>(T value = default, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 502,
                value: value,
                headers: headers);
        }

        /// <summary>Services the unavailable.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static IApiResponse ServiceUnavailable(IList<ApiHeader> headers = null, IList<string> errors = null)
        {
            return SettHttp(
                statusCode: 503,
                value: null as object,
                headers: headers,
                errors: errors);
        }

        /// <summary>Services the unavailable.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse ServiceUnavailable<T>(T value = default, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 503,
                value: value,
                headers: headers);
        }

        /// <summary>Gateways the timeout.</summary>
        /// <param name="headers">The headers.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public static IApiResponse GatewayTimeout(IList<ApiHeader> headers = null, IList<string> errors = null)
        {
            return SettHttp(
                statusCode: 504,
                value: null as object,
                headers: headers,
                errors: errors);
        }

        /// <summary>Gateways the timeout.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse GatewayTimeout<T>(T value = default, IList<ApiHeader> headers = null)
        {
            return SettHttp(
                statusCode: 504,
                value: value,
                headers: headers);
        }


        // Internal response
        // -----------------


        /// <summary>Setts the HTTP.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statusCode">The status code.</param>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        private static IApiResponse SettHttp<T>(int statusCode, T value = default, IList<ApiHeader> headers = null, IList<string> errors = null)
        {
            ApiResponse response = null;

            if (errors != null && errors.Count > 0)
            {
                response = new ApiErrorResponse
                {
                    StatusCode = statusCode,
                    Response = value,
                    Headers = new List<ApiHeader>(),
                    Errors = errors
                };
            }
            else
            {
                response = new ApiResponse
                {
                    StatusCode = statusCode,
                    Response = value,
                    Headers = new List<ApiHeader>()
                };
            }

            if (headers != null)
            {
                headers.Where(h => h != null).ToList().ForEach(h =>
                {
                    response.Headers.Add(new ApiHeader(h.Name, h.Value));
                });
            }

            return response;
        }
    }
}
