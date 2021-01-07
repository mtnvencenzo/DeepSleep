namespace DeepSleep.Api.NetCore.Tests.Binding
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Binding;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class BodyBinding_ArrayTests : PipelineTestBase
    {
        // JSON
        // ------------------

        [Fact]
        public async Task body_binding___array_ilist_json()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/binding/array/ilist HTTP/1.1
Host: {host}
Accept: {applicationJson}
Content-Type: {applicationJson}

[
    {{
        ""Value"": ""Item1""
    }},
    {{
        ""Value"": ""Item2""
    }}
]";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<IList<SimpleRs>>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().HaveCount(2);
            data[0].Value.Should().Be("Item1");
            data[1].Value.Should().Be("Item2");
        }

        [Fact]
        public async Task body_binding___array_list_json()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/binding/array/list HTTP/1.1
Host: {host}
Accept: {applicationJson}
Content-Type: {applicationJson}

[
    {{
        ""Value"": ""Item1""
    }},
    {{
        ""Value"": ""Item2""
    }}
]";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<List<SimpleRs>>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().HaveCount(2);
            data[0].Value.Should().Be("Item1");
            data[1].Value.Should().Be("Item2");
        }

        [Fact]
        public async Task body_binding___array_array_json()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/binding/array/array HTTP/1.1
Host: {host}
Accept: {applicationJson}
Content-Type: {applicationJson}

[
    {{
        ""Value"": ""Item1""
    }},
    {{
        ""Value"": ""Item2""
    }}
]";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<SimpleRs[]>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().HaveCount(2);
            data[0].Value.Should().Be("Item1");
            data[1].Value.Should().Be("Item2");
        }

        [Fact]
        public async Task body_binding___array_ienumerable_json()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/binding/array/ienumerable HTTP/1.1
Host: {host}
Accept: {applicationJson}
Content-Type: {applicationJson}

[
    {{
        ""Value"": ""Item1""
    }},
    {{
        ""Value"": ""Item2""
    }}
]";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<IEnumerable<SimpleRs>>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            var list = data.ToList();
            list.Should().HaveCount(2);
            list[0].Value.Should().Be("Item1");
            list[1].Value.Should().Be("Item2");
        }

        [Fact]
        public async Task body_binding___array_icollection_json()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/binding/array/icollection HTTP/1.1
Host: {host}
Accept: {applicationJson}
Content-Type: {applicationJson}

[
    {{
        ""Value"": ""Item1""
    }},
    {{
        ""Value"": ""Item2""
    }}
]";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<ICollection<SimpleRs>>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            var list = data.ToList();
            list.Should().HaveCount(2);
            list[0].Value.Should().Be("Item1");
            list[1].Value.Should().Be("Item2");
        }

        // XML
        // ------------------

        [Fact]
        public async Task body_binding___array_list_xml()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/binding/array/list HTTP/1.1
Host: {host}
Accept: {applicationXml}
Content-Type: {applicationXml}

<ArrayOfSimpleRs>
    <SimpleRs>
        <Value>Item1</Value>
    </SimpleRs>
    <SimpleRs>
        <Value>Item2</Value>
    </SimpleRs>
</ArrayOfSimpleRs>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationXml,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<List<SimpleRs>>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().HaveCount(2);
            data[0].Value.Should().Be("Item1");
            data[1].Value.Should().Be("Item2");
        }

        [Fact]
        public async Task body_binding___array_array_xml()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/binding/array/array HTTP/1.1
Host: {host}
Accept: {applicationXml}
Content-Type: {applicationXml}

<ArrayOfSimpleRs>
    <SimpleRs>
        <Value>Item1</Value>
    </SimpleRs>
    <SimpleRs>
        <Value>Item2</Value>
    </SimpleRs>
</ArrayOfSimpleRs>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationXml,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<SimpleRs[]>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().HaveCount(2);
            data[0].Value.Should().Be("Item1");
            data[1].Value.Should().Be("Item2");
        }
    }
}
