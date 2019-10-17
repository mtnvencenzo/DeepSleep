﻿namespace DeepSleep.NetCore
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestContextPipelineComponent
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestContextPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestContextPipelineComponent(RequestDelegate next)
        {
            _next = next;
        }

        private readonly RequestDelegate _next;

        #endregion

        #region Helper Methods and Fields

        /// <summary>Builds the API request context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private Task<ApiRequestContext> BuildApiRequestContext(HttpContext context)
        {
            DateTime serverTime = DateTime.UtcNow;
            
            var apiContext = new ApiRequestContext
            {
                PathBase = context.Request.PathBase,
                RequestAborted = context.RequestAborted,
                RequestServices = context.RequestServices,
                ProcessingInfo = new ApiProcessingInfo
                {
                    UTCRequestDuration = new ApiRequestDuration
                    {
                        ServerTime = serverTime
                    }
                },
                RequestInfo = new ApiRequestInfo
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
                    SecuredTransportMode = GetTransportSecurityMode(context.Request),
                    RemoteUser = GetRemoteUserFromServerVariables(context.Request),
                    ForceDownload = GetForceDownloadFlag(context.Request),
                    ClientAuthenticationInfo = GetClientAuthInfo(context.Request),
                    CrossOriginRequest = GetCrossOriginRequestValues(context.Request),
                    QueryVariables = GetQueryStringVariables(context),
                    PrettyPrint = GetPrettyPrint(context.Request),
                    // GETTING FULL URI BASED ON HEADER HOST AND NOT DIRECTLY FROM RequestURI.
                    // THIS CAN BE CHANGED VIA PROXY SERVERS BUT CLIENT APPS USE THE HOST HEADER
                    RequestUri = WebUtility.UrlDecode(context.Request.Scheme + "://" + context.Request.Host + context.Request.Path + context.Request.QueryString),
                    Headers = GetRequestHeaders(context.Request),
                    Body = context.Request.Body
                }
            };

            TaskCompletionSource<ApiRequestContext> source = new TaskCompletionSource<ApiRequestContext>();
            source.SetResult(apiContext);
            return source.Task;
        }

        /// <summary>Gets the request date.</summary>
        /// <param name="request">The request.</param>
        /// <param name="serverTime">The server time.</param>
        /// <returns></returns>
        private DateTime? GetRequestDate(HttpRequest request, DateTime serverTime)
        {
            if (request == null)
            {
                return null;
            }
            

            DateTime? requestDate = null;
            long epoch = 0;
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
            if (!string.IsNullOrWhiteSpace(rawEpoch) && long.TryParse(rawEpoch, NumberStyles.Any, CultureInfo.InvariantCulture, out epoch))
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
                headers.Add(new ApiHeader
                {
                    Name = httpHeader.Key,
                    Value = httpHeader.Value.ToString()
                });
            }

            return headers;
        }

        /// <summary>Processes the header date value.</summary>
        /// <param name="val">The value.</param>
        /// <param name="serverTime">The server time (UTC) when the service started processing the request.</param>
        /// <returns></returns>
        private DateTime? ProcessHeaderDateValue(string val, DateTime serverTime)
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

            if (processedVal.Contains("GMT", StringComparison.OrdinalIgnoreCase) || processedVal.EndsWith("00:00") || processedVal.EndsWith("0000"))
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


            DateTimeOffset parsedOffset;
            DateTime parsedDateTime;
            if (DateTimeOffset.TryParse(processedVal, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedOffset))
            {
                return ProcessDateHeaderValueKind(parsedOffset.DateTime, serverTime, isInUtc, hasLocalOffSet);
            }
            else if (DateTime.TryParse(processedVal, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime))
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
        private DateTime ProcessDateHeaderValueKind(DateTime parsed, DateTime serverTime, bool isInUtc, bool hasLocalOffSet)
        {
            if (parsed.Kind != DateTimeKind.Utc && isInUtc)
                return parsed.ChangeKind(DateTimeKind.Utc);


            if (parsed.Kind != DateTimeKind.Utc)
            {
                if (parsed.Hour != serverTime.Hour && hasLocalOffSet)
                {
                    return parsed.ToUniversalTime();
                }
                else
                {
                    return parsed.ChangeKind(DateTimeKind.Utc);
                }
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


            string accept = null;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "accept-charset");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    accept = header.Value;
                }
            }

            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-accept-charset");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    accept = header.Value;
                }
            }


            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xacceptcharset");
            if (queryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(queryString.Value))
                {
                    accept = queryString.Value;
                }
            }

            return accept;
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
            var authPlacement = RequestMetaDataPlacement.None;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "authorization");
            if (header.Key != null)
            {
                var authParts = header.Value.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (authParts.Length == 2)
                {
                    authScheme = authParts[0];
                    authValue = authParts[1];
                    authPlacement = RequestMetaDataPlacement.StandardHeader;
                }
            }


            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-authorization");
            if (header.Key != null)
            {
                var authParts = header.Value.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (authParts.Length == 2)
                {
                    authScheme = authParts[0];
                    authValue = authParts[1];
                    authPlacement = RequestMetaDataPlacement.CustomHeader;
                }
            }


            var authSchemeQueryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xscheme");
            var authValueQueryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "xauth");
            if (authSchemeQueryString.Key != null && authValueQueryString.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(authSchemeQueryString.Value) && !string.IsNullOrWhiteSpace(authValueQueryString.Value))
                {
                    authScheme = authSchemeQueryString.Value.ToString();
                    authValue = authValueQueryString.Value.ToString();
                    authPlacement = RequestMetaDataPlacement.QueryString;
                }
            }



            if (!string.IsNullOrWhiteSpace(authScheme) && !string.IsNullOrWhiteSpace(authValue))
            {
                return new ClientAuthentication
                {
                    AuthScheme = authScheme,
                    AuthValue = authValue,
                    AuthValuePlacement = authPlacement
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

            string origin = string.Empty;
            string requestMethod = string.Empty;
            string requestHeaders = string.Empty;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "origin");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    origin = header.Value;
                }
            }


            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "access-control-request-method");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    requestMethod = header.Value;
                }
            }


            header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "access-control-request-headers");
            if (header.Key != null)
            {
                if (!string.IsNullOrWhiteSpace(header.Value))
                {
                    requestHeaders = header.Value;
                }
            }



            return new CrossOriginRequestValues
            {
                Origin = origin,
                AccessControlRequestMethod = requestMethod,
                AccessControlRequestHeaders = requestHeaders
            };


        }

        /// <summary> Gets the transport security mode.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private TransportSecurity GetTransportSecurityMode(HttpRequest request)
        {
            if (request.IsHttps)
                return TransportSecurity.SSL;

            if (request.Headers.ContainsKey("X-Forwarded-Proto"))
            {
                var headerVal = request.Headers["X-Forwarded-Proto"][0];

                if (string.Compare(headerVal, "https", true) == 0)
                    return TransportSecurity.SSL;
            }

            if (request.Headers.ContainsKey("X-Original-Proto"))
            {
                var headerVal = request.Headers["X-Original-Proto"][0];

                if (string.Compare(headerVal, "https", true) == 0)
                    return TransportSecurity.SSL;
            }

            return TransportSecurity.None;
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

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = SplitCsv(GetHeaderValueAs<string>(context, "X-Forwarded-For")).FirstOrDefault();

            if(tryUseXForwardHeader && string.IsNullOrWhiteSpace(ip))
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

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-Port

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
                port = context.Connection.RemotePort.ToString();

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
            StringValues values;

            if (context?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
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

        /// <summary>Gets the force download flag.</summary>
        /// <param name="request">The request.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private bool GetForceDownloadFlag(HttpRequest request)
        {
            if (request == null)
            {
                return false;
            }

            bool download = false;
            bool returnDownload = false;

            var header = request.Headers.FirstOrDefault(i => i.Key.ToLower() == "x-download");
            if (header.Key != null)
            {
                foreach (string val in header.Value)
                {
                    if (bool.TryParse(val, out download) && download)
                    {
                        returnDownload = true;
                    }
                }
            }


            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "download");
            if (queryString.Key != null)
            {
                if (bool.TryParse(queryString.Value, out download) && download)
                {
                    returnDownload = true;
                }
            }

            return returnDownload;
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


            var queryString = request.Query.FirstOrDefault(i => i.Key.ToLower() == "prettyprint");
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
                    qvars.Add(q.Key, WebUtility.UrlDecode(q.Value));
                }
            }

            return qvars;
        }
        #endregion

        /// <summary>Invokes the specified httpcontext.</summary>
        /// <param name="httpcontext">The httpcontext.</param>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="requestPipeline">The request pipeline.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpcontext, IApiRequestContextResolver contextResolver, IApiRequestPipeline requestPipeline)
        {
            contextResolver.SetContext(await BuildApiRequestContext(httpcontext));
            var context = contextResolver.GetContext();
            

            if (!context.RequestAborted.IsCancellationRequested)
            {
                await requestPipeline.Run(contextResolver);

                await _next.Invoke(httpcontext);

                httpcontext.Response.Headers.Add("Date", DateTime.UtcNow.ToString("r"));


                // Merge status code to the http response
                if (context.ResponseInfo.ResponseObject.StatusCode > 0)
                {
                    httpcontext.Response.StatusCode = context.ResponseInfo.ResponseObject.StatusCode;
                }

                // Add any headers to the http context
                context.ResponseInfo.Headers.ForEach(h =>
                {
                    if (!httpcontext.Response.Headers.ContainsKey(h.Name))
                    {
                        httpcontext.Response.Headers.Add(h.Name, context.ResponseInfo.GetHeaderValues(h.Name));
                    }
                });


                if (context.ResponseInfo.RawResponseObject != null && context.ResponseInfo.RawResponseObject.Length > 0)
                {
                    httpcontext.Response.Headers.Add("Content-Length", context.ResponseInfo.ContentLength.ToString());
                    httpcontext.Response.Headers.Add("Content-Type", context.ResponseInfo.ContentType.ToString());

                    if (!string.IsNullOrWhiteSpace(context.ResponseInfo.ContentLanguage))
                    {
                        httpcontext.Response.Headers.Add("Content-Language", context.ResponseInfo.ContentLanguage);
                    }

                    if (!context.RequestInfo.IsHeadRequest())
                    {
                        await httpcontext.Response.Body.WriteAsync(context.ResponseInfo.RawResponseObject, 0, context.ResponseInfo.RawResponseObject.Length);
                    }
                }
                else
                {
                    httpcontext.Response.Headers.Add("Content-Length", context.ResponseInfo.ContentLength.ToString());
                }
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
    }
}
