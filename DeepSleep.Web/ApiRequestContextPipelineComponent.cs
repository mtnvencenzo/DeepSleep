namespace DeepSleep.Web
{
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestContextPipelineComponent
    {
        private readonly RequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestContextPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestContextPipelineComponent(RequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified httpcontext.</summary>
        /// <param name="httpcontext">The httpcontext.</param>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="requestPipeline">The request pipeline.</param>
        /// <param name="config">The config.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpcontext, IApiRequestContextResolver contextResolver, IApiRequestPipeline requestPipeline, IApiServiceConfiguration config)
        {
            var path = httpcontext?.Request?.Path.ToString() ?? string.Empty;

            if (config?.ExcludePaths != null)
            {
                foreach (var excludedPath in config.ExcludePaths)
                {
                    var match = Regex.IsMatch(path, excludedPath);
                    if (match)
                    {
                        await apinext.Invoke(httpcontext);
                        return;
                    }
                }
            }

            contextResolver.SetContext(await BuildApiRequestContext(httpcontext));
            var context = contextResolver.GetContext();

#if DEBUG
            var previousForeColor = Console.ForegroundColor;
            Console.Write($"{context.Runtime.Duration.UtcStart.ToString("yyyy-MM-ddT HH:mm:ss.fffzzz", CultureInfo.CurrentCulture)} ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{context.Request.Method.ToUpper()} {context.Request.RequestUri} {context.Request.Protocol}");
            Console.ForegroundColor = previousForeColor;
#endif
            var defaultRequestConfiguration = context.RequestServices.GetService(typeof(IApiRequestConfiguration)) as IApiRequestConfiguration;

            await context.ProcessApiRequest(httpcontext, contextResolver, requestPipeline, defaultRequestConfiguration);
        }

        /// <summary>Builds the API request context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private Task<ApiRequestContext> BuildApiRequestContext(HttpContext context)
        {
            var serverTime = DateTimeOffset.UtcNow;

            var apiContext = new ApiRequestContext
            {
                PathBase = context.Request.PathBase,
                RequestAborted = context.RequestAborted,
                RequestServices = context.RequestServices,
                RegisterForDispose = (disposable) => context.Response.RegisterForDispose(disposable),
                ConfigureMaxRequestLength = (length) => SetMaxRequestLength(length, context),
                Request = new ApiRequestInfo
                {
                    Path = context.Request.Path,
                    Protocol = context.Request.Protocol,
                    RequestIdentifier = context.TraceIdentifier ?? Guid.NewGuid().ToString(),
                    Method = context.Request.Method.ToUpper(),
                    Accept = GetAcceptTypes(context.Request),
                    AcceptCharset = GetAcceptCharset(context.Request),
                    AcceptLanguage = GetAcceptLanguage(context.Request),
                    AcceptCulture = CultureInfo.CurrentUICulture,
                    AcceptEncoding = GetAcceptEncoding(context.Request),
                    ContentType = context.Request.ContentType,
                    ContentLength = context.Request.ContentLength,
                    RequestDate = GetRequestDate(context.Request, serverTime),
                    CorrelationId = GetCorrelationId(context.Request),
                    RemoteUser = GetRemoteUserFromServerVariables(context.Request),
                    ClientAuthenticationInfo = GetClientAuthInfo(context.Request),
                    CrossOriginRequest = GetCrossOriginRequestValues(context.Request),
                    QueryVariables = GetQueryStringVariables(context),
                    PrettyPrint = GetPrettyPrint(context.Request),
                    Cookies = GetRequestCookies(context.Request),
                    // GETTING FULL URI BASED ON HEADER HOST AND NOT DIRECTLY FROM RequestURI.
                    // THIS CAN BE CHANGED VIA PROXY SERVERS BUT CLIENT APPS USE THE HOST HEADER
                    RequestUri = context.Request.GetEncodedUrl(),
                    Headers = GetRequestHeaders(context.Request),
                    IfMatch = GetIfMatch(context.Request),
                    IfNoneMatch = GetIfNoneMatch(context.Request),
                    IfModifiedSince = GetIfModifiedSince(context.Request),
                    IfUnmodifiedSince = GetIfUnmodifiedSince(context.Request),
                    Body = context.Request.Body
                }
            };

            apiContext.Runtime.Duration.UtcStart = serverTime;

            return Task.FromResult(apiContext);
        }

        /// <summary>Gets the request date.</summary>
        /// <param name="request">The request.</param>
        /// <param name="serverTime">The server time.</param>
        /// <returns></returns>
        private DateTime? GetRequestDate(HttpRequest request, DateTimeOffset serverTime)
        {
            if (request == null)
            {
                return null;
            }


            DateTime? requestDate = null;
            string rawEpoch = string.Empty;

            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xdate");
            if (queryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(queryString.Value))
                {
                    rawEpoch = queryString.Value;
                }
            }

            // QUERY STRING DATES OVERRIDE ALL OTHER DATES
            // INCLUDED STANDARD 'Date' AND CUSTOM 'X-Date' HEADERS
            if (!string.IsNullOrWhiteSpace(rawEpoch) && long.TryParse(rawEpoch, NumberStyles.Any, CultureInfo.InvariantCulture, out var epoch))
            {
                requestDate = epoch.ToUtcDate();
            }
            else
            {
                // CERTAIN DATE FORMATTS DO NOT GET PICKED UP
                // BY .NET AND APPLIED IN THE TYPE'D DATE HEADER
                // USE THE .NET DATE HEADER VALUE BUT IF NO VALUE
                // TRY AND SEE IF DATE HEADER EXISTS AND PARSE
                if (request.Headers.ContainsKey("Date"))
                {
                    requestDate = (ProcessHeaderDateValue(request.Headers["Date"], serverTime)) ?? requestDate;
                }

                // THE X-DATE HEADER WILL OVERRIDE THE STANDARD DATE HEADER
                if (request.Headers.ContainsKey("X-Date"))
                {
                    requestDate = (ProcessHeaderDateValue(request.Headers["X-Date"], serverTime)) ?? requestDate;
                }
            }


            return requestDate;
        }

        /// <summary>Gets the request headers.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private List<ApiHeader> GetRequestHeaders(HttpRequest request)
        {
            List<ApiHeader> headers = new List<ApiHeader>();

            foreach (var httpHeader in request.Headers)
            {
                headers.Add(new ApiHeader(httpHeader.Key, httpHeader.Value.ToString()));
            }

            return headers;
        }

        /// <summary>Processes the header date value.</summary>
        /// <param name="val">The value.</param>
        /// <param name="serverTime">The server time (UTC) when the service started processing the request.</param>
        /// <returns></returns>
        private DateTime? ProcessHeaderDateValue(string val, DateTimeOffset serverTime)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return null;
            }

            string[] ascTimeFormats = new string[]
            {
                "ddd MMM  d HH:mm:ss yyyy",
                "dddd MMM  d HH:mm:ss yyyy",

                "ddd MMM d HH:mm:ss yyyy",
                "dddd MMM d HH:mm:ss yyyy",

                "ddd MMM dd HH:mm:ss yyyy",
                "dddd MMM dd HH:mm:ss yyyy",

                "ddd, dd‐MMM‐yy HH:mm:ss GMT",
                "dddd, dd‐MMM‐yy HH:mm:ss GMT",

                "ddd, dd‐MMM‐yy HH:mm:ss",
                "dddd, dd‐MMM‐yy HH:mm:ss",

                "ddd, dd MMM yyyy HH:mm:ss zzz",
                "dddd, dd MMM yyyy HH:mm:ss zzz",

                "ddd, dd‐MMM‐yy HH:mm:ss zzz",
                "dddd, dd‐MMM‐yy HH:mm:ss zzz"
            };

            var processedVal = Regex.Replace(val, "UTC", "GMT", RegexOptions.IgnoreCase);
            processedVal = Regex.Replace(processedVal, "UT", "GMT", RegexOptions.IgnoreCase);

            bool isInUtc = false;
            bool hasLocalOffSet = false;


            if (processedVal.ToLowerInvariant().Contains("gmt") || processedVal.EndsWith("00:00") || processedVal.EndsWith("0000"))
            {
                isInUtc = true;
            }

            if (processedVal.Length > 6)
            {
                string fifthChar = processedVal.Substring(processedVal.Length - 5, 1);
                string sixthChar = processedVal.Substring(processedVal.Length - 6, 1);

                if (fifthChar == "-" || fifthChar == "+" || sixthChar == "-" || sixthChar == "+")
                {
                    hasLocalOffSet = true;
                }
            }

            if (DateTimeOffset.TryParse(processedVal, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedOffset))
            {
                return ProcessDateHeaderValueKind(parsedOffset.DateTime, serverTime, isInUtc, hasLocalOffSet);
            }
            else if (DateTime.TryParse(processedVal, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
            {
                return ProcessDateHeaderValueKind(parsedDateTime, serverTime, isInUtc, hasLocalOffSet);
            }
            else if (DateTimeOffset.TryParseExact(processedVal, ascTimeFormats, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedOffset))
            {
                return ProcessDateHeaderValueKind(parsedOffset.DateTime, serverTime, isInUtc, hasLocalOffSet);
            }
            else if (DateTime.TryParseExact(processedVal, ascTimeFormats, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDateTime))
            {
                return ProcessDateHeaderValueKind(parsedDateTime, serverTime, isInUtc, hasLocalOffSet);
            }

            return null;
        }

        /// <summary>Processes the kind of the date header value.</summary>
        /// <param name="parsed">The parsed.</param>
        /// <param name="serverTime">The serverTime when the service started processing the request.</param>
        /// <param name="isInUtc">if set to <c>true</c> [is in UTC].</param>
        /// <param name="hasLocalOffSet">if set to <c>true</c> [has local off set].</param>
        /// <returns></returns>
        private DateTime ProcessDateHeaderValueKind(DateTime parsed, DateTimeOffset serverTime, bool isInUtc, bool hasLocalOffSet)
        {
            if (parsed.Kind != DateTimeKind.Utc && isInUtc)
                return parsed.ChangeKind(DateTimeKind.Utc);


            if (parsed.Kind != DateTimeKind.Utc)
            {
                return parsed.ChangeKind(DateTimeKind.Utc);
            }
            else
            {
                return parsed;
            }
        }

        /// <summary>Gets the client specified correlation id from the reuqest.  A check is first made on the request headers for a
        ///     header named 'X-CorrelationID'.
        ///     If non-existent the the check is made against the query string for the xCorrelationID parameter.</summary>
        /// <param name="request">The request.</param>
        /// <returns>The correlation id from the request or null.</returns>
        private string GetCorrelationId(HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }


            string correlationID = null;



            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-correlationid");
            if (header.Key != null)
            {
                foreach (string val in header.Value)
                {
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        correlationID = val;
                    }
                }
            }


            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xcorrelationid");
            if (queryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(queryString.Value))
                {
                    correlationID = queryString.Value;
                }
            }

            return correlationID;
        }

        /// <summary>Gets the client specified correlation id from the reuqest.  A check is first made on the request headers for a
        ///     header named 'X-CorrelationID'.
        ///     If non-existent the the check is made against the query string for the xCorrelationID parameter.</summary>
        /// <param name="request">The request.</param>
        /// <returns>The correlation id from the request or null.</returns>
        private string GetAcceptTypes(HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }


            string accept = null;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "accept");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    accept = header.Value;
                }
            }

            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-accept");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    accept = header.Value;
                }
            }


            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xaccept");
            if (queryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(queryString.Value))
                {
                    accept = queryString.Value;
                }
            }

            return accept;
        }

        /// <summary>Gets the accept language.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private string GetAcceptLanguage(HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }


            string accept = null;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "accept-language");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    accept = header.Value;
                }
            }

            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-accept-language");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    accept = header.Value;
                }
            }


            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xacceptlanguage");
            if (queryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(queryString.Value))
                {
                    accept = queryString.Value;
                }
            }

            return accept;
        }

        /// <summary>Gets the accept encoding.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private string GetAcceptEncoding(HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }


            string accept = null;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "accept-encoding");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    accept = header.Value;
                }
            }

            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-accept-encoding");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    accept = header.Value;
                }
            }


            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xacceptencoding");
            if (queryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(queryString.Value))
                {
                    accept = queryString.Value;
                }
            }

            return accept;
        }

        /// <summary>Gets the accept charset.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private string GetAcceptCharset(HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }


            string charset = null;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "accept-charset");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    charset = header.Value;
                }
            }

            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-accept-charset");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    charset = header.Value;
                }
            }

            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xacceptcharset");
            if (queryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(queryString.Value))
                {
                    charset = queryString.Value;
                }
            }

            return charset;
        }

        /// <summary>Gets the client authentication information.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private ClientAuthentication GetClientAuthInfo(HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }


            string authScheme = string.Empty;
            string authValue = string.Empty;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "authorization");
            if (header.Key != null)
            {
                var authParts = header.Value.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (authParts.Length == 1)
                {
                    authScheme = authParts[0];
                    authValue = null;
                }
                else if (authParts.Length == 2)
                {
                    authScheme = authParts[0];
                    authValue = authParts[1];
                }
            }


            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-authorization");
            if (header.Key != null)
            {
                var authParts = header.Value.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (authParts.Length == 1)
                {
                    authScheme = authParts[0];
                    authValue = null;
                }
                else if (authParts.Length == 2)
                {
                    authScheme = authParts[0];
                    authValue = authParts[1];
                }
            }


            var authSchemeQueryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xscheme");
            var authValueQueryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xauth");
            if (authSchemeQueryString.Key != null && authValueQueryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(authSchemeQueryString.Value))
                {
                    authScheme = authSchemeQueryString.Value.ToString();

                    if (!string.IsNullOrWhiteSpace(authValueQueryString.Value))
                    {
                        authValue = authValueQueryString.Value.ToString();
                    }
                }
            }


            if (!string.IsNullOrWhiteSpace(authScheme))
            {
                return new ClientAuthentication
                {
                    AuthScheme = authScheme,
                    AuthValue = authValue
                };
            }


            return null;
        }

        /// <summary>Gets the cross origin request values.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private CrossOriginRequestValues GetCrossOriginRequestValues(HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }

            string origin = null;
            string requestMethod = null;
            string requestHeaders = null;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "origin");
            if (header.Key != null)
            {
                origin = header.Value;
            }


            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "access-control-request-method");
            if (header.Key != null)
            {
                requestMethod = header.Value;
            }

            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "access-control-request-headers");
            if (header.Key != null)
            {
                requestHeaders = header.Value;
            }

            return new CrossOriginRequestValues
            {
                Origin = origin,
                AccessControlRequestMethod = requestMethod,
                AccessControlRequestHeaders = requestHeaders
            };
        }

        /// <summary>Gets the remote user from server variables.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private ApiRemoteUser GetRemoteUserFromServerVariables(HttpRequest request)
        {
            var remoteUser = new ApiRemoteUser
            {
                Addr = GetRequestIP(request.HttpContext, true),
                Host = GetRequestIP(request.HttpContext, true),
                Port = GetRequestPort(request.HttpContext, true),
                User = GetRequestIP(request.HttpContext, true)
            };

            return (!string.IsNullOrWhiteSpace(remoteUser.Addr) || !string.IsNullOrWhiteSpace(remoteUser.Host) || !string.IsNullOrWhiteSpace(remoteUser.Port) || !string.IsNullOrWhiteSpace(remoteUser.User))
                ? remoteUser
                : null;
        }

        /// <summary>Gets the request ip.</summary>
        /// <param name="context">The context.</param>
        /// <param name="tryUseXForwardHeader">if set to <c>true</c> [try use x forward header].</param>
        /// <returns></returns>
        private string GetRequestIP(HttpContext context, bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // TODO: support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = SplitCsv(GetHeaderValueAs<string>(context, "X-Forwarded-For")).FirstOrDefault();

            if (tryUseXForwardHeader && string.IsNullOrWhiteSpace(ip))
                ip = SplitCsv(GetHeaderValueAs<string>(context, "X-Original-For")).FirstOrDefault();


            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(ip) && context?.Connection?.RemoteIpAddress != null)
                ip = context.Connection.RemoteIpAddress.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                ip = GetHeaderValueAs<string>(context, "REMOTE_ADDR");

            return ip;
        }

        /// <summary>Gets the request port.</summary>
        /// <param name="context">The context.</param>
        /// <param name="tryUseXForwardHeader">if set to <c>true</c> [try use x forward header].</param>
        /// <returns></returns>
        private string GetRequestPort(HttpContext context, bool tryUseXForwardHeader = true)
        {
            string port = null;

            // TODO: support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-Port

            // X-Forwarded-Port (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                port = SplitCsv(GetHeaderValueAs<string>(context, "X-Forwarded-Port")).FirstOrDefault();

            if (tryUseXForwardHeader && string.IsNullOrWhiteSpace(port))
                port = SplitCsv(GetHeaderValueAs<string>(context, "X-Original-Port")).FirstOrDefault();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (string.IsNullOrWhiteSpace(port) && context?.Connection?.RemotePort != null)
                port = context.Connection.RemotePort.ToString(CultureInfo.InvariantCulture);

            if (string.IsNullOrWhiteSpace(port))
                port = GetHeaderValueAs<string>(context, "REMOTE_PORT");

            return port;
        }

        /// <summary>Gets the header value as.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="headerName">Name of the header.</param>
        /// <returns></returns>
        private T GetHeaderValueAs<T>(HttpContext context, string headerName)
        {
            StringValues values = default;

            if (context?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }

            return default;
        }

        /// <summary>Splits the CSV.</summary>
        /// <param name="csvList">The CSV list.</param>
        /// <param name="nullOrWhitespaceInputReturnsNull">if set to <c>true</c> [null or whitespace input returns null].</param>
        /// <returns></returns>
        private List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }

        /// <summary>Gets the pretty print.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private bool GetPrettyPrint(HttpRequest request)
        {
            if (request == null)
            {
                return false;
            }

            bool prettyPrint = false;
            bool returnPrettyPrint = false;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-prettyprint");
            if (header.Key != null)
            {
                foreach (string val in header.Value)
                {
                    if (bool.TryParse(val, out prettyPrint) && prettyPrint)
                    {
                        returnPrettyPrint = true;
                    }
                }
            }


            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xprettyprint");
            if (queryString.Key != null)
            {
                if (bool.TryParse(queryString.Value, out prettyPrint) && prettyPrint)
                {
                    returnPrettyPrint = true;
                }
            }

            return returnPrettyPrint;
        }

        /// <summary>Gets the query string variables.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private Dictionary<string, string> GetQueryStringVariables(HttpContext context)
        {
            var qvars = new Dictionary<string, string>();

            if (context.Request.Query != null)
            {
                foreach (var q in context.Request.Query)
                {
                    qvars.Add(q.Key, q.Value);
                }
            }

            return qvars;
        }

        /// <summary>Gets the request cookies.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private Dictionary<string, string> GetRequestCookies(HttpRequest request)
        {
            var cookies = new Dictionary<string, string>();

            if (request.Cookies != null)
            {
                foreach (var requestCookie in request.Cookies)
                {
                    if (!cookies.ContainsKey(requestCookie.Key))
                    {
                        cookies.Add(requestCookie.Key, requestCookie.Value);
                    }
                }
            }

            return cookies;
        }

        /// <summary>Gets if match.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private string GetIfMatch(HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }

            string returnValue = null;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "if-match");
            if (header.Key != null)
            {
                foreach (string val in header.Value)
                {
                    if (val != null)
                    {
                        returnValue = val;
                        break;
                    }
                }
            }

            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-if-match");
            if (header.Key != null)
            {
                foreach (string val in header.Value)
                {
                    if (val != null)
                    {
                        returnValue = val;
                        break;
                    }
                }
            }


            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xifmatch");
            if (queryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(queryString.Value))
                {
                    returnValue = queryString.Value;
                }
            }

            return returnValue;
        }

        /// <summary>Gets if none match.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private string GetIfNoneMatch(HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }

            string returnValue = null;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "if-none-match");
            if (header.Key != null)
            {
                foreach (string val in header.Value)
                {
                    if (val != null)
                    {
                        returnValue = val;
                        break;
                    }
                }
            }

            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-if-none-match");
            if (header.Key != null)
            {
                foreach (string val in header.Value)
                {
                    if (val != null)
                    {
                        returnValue = val;
                        break;
                    }
                }
            }


            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xifnonematch");
            if (queryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(queryString.Value))
                {
                    returnValue = queryString.Value;
                }
            }

            return returnValue;
        }

        /// <summary>Gets if modified since.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private DateTimeOffset? GetIfModifiedSince(HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }

            DateTimeOffset parsed;
            DateTimeOffset? returnValue = null;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "if-modified-since");
            if (header.Key != null)
            {
                foreach (string val in header.Value)
                {
                    if (val != null)
                    {
                        if (DateTimeOffset.TryParse(val, out parsed))
                        {
                            returnValue = parsed;
                        }
                        break;
                    }
                }
            }

            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-if-modified-since");
            if (header.Key != null)
            {
                foreach (string val in header.Value)
                {
                    if (val != null)
                    {
                        if (DateTimeOffset.TryParse(val, out parsed))
                        {
                            returnValue = parsed;
                        }
                        break;
                    }
                }
            }


            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xifmodifiedsince");
            if (queryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(queryString.Value))
                {
                    if (DateTimeOffset.TryParse(queryString.Value, out parsed))
                    {
                        returnValue = parsed;
                    }
                }
            }

            return returnValue;
        }

        /// <summary>Gets if unmodified since.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private DateTimeOffset? GetIfUnmodifiedSince(HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }

            DateTimeOffset parsed;
            DateTimeOffset? returnValue = null;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "if-unmodified-since");
            if (header.Key != null)
            {
                foreach (string val in header.Value)
                {
                    if (val != null)
                    {
                        if (DateTimeOffset.TryParse(val, out parsed))
                        {
                            returnValue = parsed;
                        }
                        break;
                    }
                }
            }

            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-if-unmodified-since");
            if (header.Key != null)
            {
                foreach (string val in header.Value)
                {
                    if (val != null)
                    {
                        if (DateTimeOffset.TryParse(val, out parsed))
                        {
                            returnValue = parsed;
                        }
                        break;
                    }
                }
            }


            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xifunmodifiedsince");
            if (queryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(queryString.Value))
                {
                    if (DateTimeOffset.TryParse(queryString.Value, out parsed))
                    {
                        returnValue = parsed;
                    }
                }
            }

            return returnValue;
        }

        /// <summary>Sets the maximum length of the request.</summary>
        /// <param name="length">The length.</param>
        /// <param name="context">The context.</param>
        private void SetMaxRequestLength(long length, HttpContext context)
        {
            var feature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();

            if (feature != null && !feature.IsReadOnly)
            {
                feature.MaxRequestBodySize = length <= 0
                    ? null as long?
                    : length;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestContextPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request context.</summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiRequestContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiRequestContextPipelineComponent>();
        }

        /// <summary>Processes the API request.</summary>
        /// <param name="context">The context.</param>
        /// <param name="httpcontext">The httpcontext.</param>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="requestPipeline">The request pipeline.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessApiRequest(
            this ApiRequestContext context, 
            HttpContext httpcontext, 
            IApiRequestContextResolver contextResolver, 
            IApiRequestPipeline requestPipeline,
            IApiRequestConfiguration defaultRequestConfiguration)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                await requestPipeline.Run(contextResolver);
                context.SetThreadCulure();

                var responseDate = DateTimeOffset.UtcNow;
                context.Response.Date = responseDate;

                context.Response.AddHeader(
                    name: "Date",
                    value: responseDate.ToString("r"),
                    append: false,
                    allowMultiple: false);

                httpcontext.Response.Headers.Add("Date", responseDate.ToString("r"));

                // Sync up the expire header for nocache requests with the date header being used
                var contextExpiresHeader = context.Response.Headers.FirstOrDefault(h => h.Name == "Expires");

                var expirationSeconds = context.Configuration?.CacheDirective?.ExpirationSeconds
                    ?? defaultRequestConfiguration?.CacheDirective?.ExpirationSeconds
                    ?? ApiRequestContext.GetDefaultRequestConfiguration().CacheDirective.ExpirationSeconds.Value;

                var cacheability = context.Configuration?.CacheDirective?.Cacheability
                    ?? defaultRequestConfiguration?.CacheDirective?.Cacheability
                    ?? ApiRequestContext.GetDefaultRequestConfiguration().CacheDirective.Cacheability.Value;

                if (contextExpiresHeader != null)
                {
                    if (cacheability == HttpCacheType.NoCache && expirationSeconds > 0)
                    {
                        contextExpiresHeader.Value = responseDate.AddSeconds(-1).ToString("r");
                    }
                    else
                    {
                        contextExpiresHeader.Value = responseDate.AddSeconds(expirationSeconds).ToString("r");
                    }
                }

                // Merge status code to the http response
                httpcontext.Response.StatusCode = context.Response.StatusCode;

                if (context.Response.StatusCode == 450)
                {
                    try
                    {
                        var feature = httpcontext?.Response?.HttpContext?.Features?.Get<IHttpResponseFeature>();
                        if (feature != null)
                        {
                            feature.ReasonPhrase = "Bad Request Format";
                        }
                    }
                    catch { }
                }

                // Add any headers to the http context
                void addHeadersToResponse() => context.Response.Headers.ForEach(h =>
                {
                    if (!httpcontext.Response.Headers.ContainsKey(h.Name))
                    {
                        httpcontext.Response.Headers.Add(h.Name, context.Response.GetHeaderValues(h.Name).ToArray());
                    }
                });

                if (context.Response.ResponseWriter != null && context.Response.ResponseWriterOptions != null)
                {
                    context.Response.AddHeader(
                        name: "Content-Type",
                        value: context.Response.ContentType.ToString(),
                        append: false,
                        allowMultiple: false);

                    if (!string.IsNullOrWhiteSpace(context.Response.ContentLanguage))
                    {
                        context.Response.AddHeader(
                            name: "Content-Language",
                            value: context.Response.ContentLanguage,
                            append: false,
                            allowMultiple: false);
                    }


                    if (!context.Request.IsHeadRequest())
                    {
                        var contentLength = await context.Response.ResponseWriter.WriteType(
                            stream: httpcontext.Response.Body,
                            obj: context.Response.ResponseObject,
                            context.Response.ResponseWriterOptions,
                            (l) =>
                            {
                                context.Response.ContentLength = l;

                                context.Response.AddHeader(
                                    name: "Content-Length",
                                    value: l.ToString(CultureInfo.InvariantCulture),
                                    append: false,
                                    allowMultiple: false);

                                addHeadersToResponse();
                            }).ConfigureAwait(false);

                        context.Response.ContentLength = contentLength;
                    }
                    else
                    {
                        using (var ms = new MemoryStream())
                        {
                            await context.Response.ResponseWriter.WriteType(
                                ms,
                                context.Response.ResponseObject,
                                context.Response.ResponseWriterOptions).ConfigureAwait(false);

                            context.Response.ResponseObject = null;
                            context.Response.ContentLength = ms.Length;

                            context.Response.AddHeader(
                                name: "Content-Length",
                                value: ms.Length.ToString(CultureInfo.InvariantCulture),
                                append: false,
                                allowMultiple: false);

                            addHeadersToResponse();
                        }
                    }
                }
                else
                {
                    context.Response.ContentLength = 0;
                    context.Response.AddHeader("Content-Length", "0");
                    addHeadersToResponse();
                }

                context.Runtime.Duration.UtcEnd = DateTime.UtcNow;
            }

            return true;
        }
    }
}
