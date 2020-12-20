namespace DeepSleep.Api.NetCore.Tests.Mocks
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Security.Claims;
    using System.Threading;

    /// <summary>
    /// 
    /// </summary>
    public class MockHttpContext : HttpContext, IDisposable
    {
        private readonly MockHttpRequest httpRequest;
        private readonly MockHttpResponse httpResponse;
        private readonly ConnectionInfo connectionInfo;
        private readonly FeatureCollection features;

        public MockHttpContext(IServiceProvider serviceProvider, string request)
        {
            RequestAborted = new CancellationToken();
            TraceIdentifier = $"UT-{Guid.NewGuid()}";
            RequestServices = serviceProvider;
            Items = new Dictionary<object, object>();

            this.features = new FeatureCollection();
            this.httpRequest = MockHttpRequest.FromHttpRequestString(this, request);
            this.connectionInfo = BuildConnectionInfo();
            this.httpResponse = new MockHttpResponse(this);
        }

        public override CancellationToken RequestAborted { get; set; }

        public override HttpRequest Request => httpRequest;

        public override HttpResponse Response => httpResponse;

        public override IServiceProvider RequestServices { get; set; }

        public override string TraceIdentifier { get; set; }

        public override ClaimsPrincipal User { get; set; }

        public override IDictionary<object, object> Items { get; set; }

        public override ConnectionInfo Connection => connectionInfo;

        public override IFeatureCollection Features => features;

        public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override WebSocketManager WebSockets => throw new NotImplementedException();

        public override void Abort()
        {
            RequestAborted = new CancellationToken(true);
        }

        private ConnectionInfo BuildConnectionInfo()
        {
            return new MockConnectionInfo()
            {
                LocalIpAddress = new IPAddress(0),
                RemoteIpAddress = new IPAddress(0),
                Id = Guid.NewGuid().ToString(),
                LocalPort = 0,
                RemotePort = 0
            };
        }

        public void Dispose()
        {
            this.httpRequest.Dispose();
            this.httpResponse.Dispose();
        }
    }
}
