namespace DeepSleep.NetCore.Tests.Mocks
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class MockHttpRequest : DefaultHttpRequest, IDisposable
    {
        private readonly HeaderDictionary headers;
        private IRequestCookieCollection cookies;
        private IFormCollection form;
        private Stream stream;


        public MockHttpRequest(HttpContext context)
            : base(context) 
        {
            this.headers = new HeaderDictionary();
            this.cookies = new RequestCookieCollection();
            this.form = new FormCollection(new Dictionary<string, StringValues>());
            this.stream = new MemoryStream();
        }

        public override IHeaderDictionary Headers => this.headers;

        public override Stream Body { get => this.stream; set => this.stream = value; }

        public override long? ContentLength { get; set; }

        public override string ContentType { get; set; }

        public override HostString Host { get; set; }

        public override bool IsHttps { get; set; }

        public override string Method { get; set; }

        public override PathString Path { get; set; }

        public override PathString PathBase { get; set; }

        public override string Protocol { get; set; }

        public override IQueryCollection Query { get; set; }

        public override string Scheme { get; set; }

        public override QueryString QueryString { get; set; }

        public override IRequestCookieCollection Cookies { get => this.cookies; set => this.cookies = value; }

        public override IFormCollection Form { get => this.form; set => this.form = value; }

        public void Dispose()
        {
            if (this.stream != null)
            {
                this.stream.Dispose();
            }
        }

        public static MockHttpRequest FromHttpRequestString(HttpContext context, string http)
        {
            var httpLines = http.Split(System.Environment.NewLine);
            var method = null as string;
            var protocol = null as string;
            var rawuri = null as string;
            var headers = new List<(string name, string value)>();

            // Pull main header line
            foreach (var httpLine in httpLines)
            {
                var line = httpLine.Trim();

                if (!string.IsNullOrWhiteSpace(line))
                {
                    method = line.Substring(0, line.IndexOf(" "));
                    protocol = line.Substring(line.LastIndexOf(" "));

                    rawuri = line.Substring(method.Length);
                    rawuri = rawuri.Substring(0, rawuri.LastIndexOf(protocol));
                    break;
                }
            }

            var inHeaders = false;
            var inBody = false;
            var bodyText = string.Empty;
            // Pull headers
            foreach (var httpLine in httpLines)
            {
                var line = httpLine.Trim();

                if (!inBody && !inHeaders && !string.IsNullOrWhiteSpace(line))
                {
                    inHeaders = true;
                    continue;
                }

                if (inHeaders && !inBody)
                {
                    if(string.IsNullOrWhiteSpace(line))
                    {
                        inHeaders = false;
                        inBody = true;
                        continue;
                    }

                    string headerName = line.Substring(0, line.IndexOf(":"));
                    string headerValue = line.Substring(headerName.Length + 1);
                    headers.Add((headerName.Trim(), headerValue.Trim()));
                    continue;
                }

                if (inHeaders && string.IsNullOrWhiteSpace(line))
                {
                    inHeaders = false;
                    inBody = true;
                    continue;
                }

                if (inBody)
                {
                    bodyText += httpLine;
                    bodyText += System.Environment.NewLine;
                }
            }


            var uri = new Uri(rawuri.Trim());

            var host = new HostString(uri.Host, uri.Port);
            var path = new PathString(uri.AbsolutePath);
            var pathBase = new PathString(uri.AbsolutePath);
            var queryString = new QueryString(uri.Query);

            var request = new MockHttpRequest(context)
            {
                ContentLength = 0,
                ContentType = null,
                Host = host,
                IsHttps = true,
                Method = method.Trim().ToUpper(),
                Protocol = protocol.Trim().ToUpper(),
                Path = path,
                PathBase = pathBase,
                QueryString = queryString,
                Scheme = uri.Scheme,
                Form = new FormCollection(new Dictionary<string, StringValues>()),
                Cookies = new RequestCookieCollection(new Dictionary<string, string>()),
                Query = new QueryCollection(new Dictionary<string, StringValues>())
            };

            if (headers.Count > 0)
            {
                headers.ForEach(h => request.headers.Add(new KeyValuePair<string, StringValues>(h.name, new StringValues(h.value))));
            }

            if (!string.IsNullOrWhiteSpace(bodyText))
            {
                request.Body.Seek(0, SeekOrigin.Begin);

                using (var writer = new StreamWriter(request.Body))
                {
                    writer.Write(bodyText);
                    writer.Flush();
                    request.Body.Position = 0;
                    request.Body.Seek(0, SeekOrigin.Begin);
                }
            }

            return request;
        }
    }
}
