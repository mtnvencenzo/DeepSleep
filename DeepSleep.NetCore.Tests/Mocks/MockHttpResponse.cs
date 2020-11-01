namespace DeepSleep.NetCore.Tests.Mocks
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.Extensions.ObjectPool;
    using Microsoft.Extensions.Primitives;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public class MockHttpResponse : DefaultHttpResponse, IDisposable
    {
        private readonly HeaderDictionary headers;
        private IResponseCookies cookies;
        private IFormCollection form;
        private Stream stream;

        public MockHttpResponse(HttpContext context)
            : base(context)
        {
            this.headers = new HeaderDictionary();
            this.cookies = new ResponseCookies(new HeaderDictionary(), new DefaultObjectPool<StringBuilder>(new DefaultPooledObjectPolicy<StringBuilder>()));
            this.form = new FormCollection(new Dictionary<string, StringValues>());
            this.stream = new MemoryStream();
        }

        public override IHeaderDictionary Headers => this.headers;

        public override IResponseCookies Cookies => this.cookies;

        public override Stream Body { get => this.stream; set => this.stream = value; }

        public override long? ContentLength { get; set; }

        public override string ContentType { get; set; }

        public override int StatusCode { get; set; }

        public void Dispose()
        {
            if (this.stream != null)
            {
                this.stream.Dispose();
            }
        }
    }
}
