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



        // Helper Responses
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

        /// <summary>Bads the request.</summary>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse BadRequest(IList<ApiHeader> headers = null)
        {
            return BadRequest(
                value: null as object,
                headers: headers);
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

        /// <summary>Unauthorizeds the specified headers.</summary>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public static IApiResponse Unauthorized(IList<ApiHeader> headers = null)
        {
            return Unauthorized<object>(
                value: null as object,
                headers: headers);
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
        /// <returns></returns>
        public static IApiResponse Forbidden(IList<ApiHeader> headers = null)
        {
            return Forbidden(
                value: null as object,
                headers: headers);
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
        /// <returns></returns>
        public static IApiResponse NotFound(IList<ApiHeader> headers = null)
        {
            return NotFound(
                value: null as object,
                headers: headers);
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
        /// <returns></returns>
        public static IApiResponse Conflict(IList<ApiHeader> headers = null)
        {
            return Conflict(
                value: null as object,
                headers: headers);
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

        /// <summary>Setts the HTTP.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="statusCode">The status code.</param>
        /// <param name="value">The value.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        private static IApiResponse SettHttp<T>(int statusCode, T value = default, IList<ApiHeader> headers = null)
        {
            var response = new ApiResponse
            {
                StatusCode = statusCode,
                Response = value,
                Headers = new List<ApiHeader>()
            };

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
