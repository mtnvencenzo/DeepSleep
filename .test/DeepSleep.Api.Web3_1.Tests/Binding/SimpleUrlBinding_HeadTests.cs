namespace DeepSleep.Api.Web.Tests.Binding
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Binding;
    using global::Api.DeepSleep.Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Xunit;

    public class SimpleUrlBinding_HeadTests : PipelineTestBase
    {
        private readonly string query_url = "/binding/simple/url";

        // --- SUCCESS QUERY STRING BINDING
        // -------------------------------

        [Fact]
        public async Task HEAD_binding_simple_url_querystring_correct_case_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            var nowDt = DateTime.Now;
            var nowOff = DateTimeOffset.Now;
            var ts = TimeSpan.FromSeconds(1);

            string stringVar = "test";
            char charVar = 'A';
            char? nullCharVar = 'A';
            short int16Var = -10;
            ushort uInt16Var = 12;
            short? nullInt16Var = -13;
            ushort? nullUInt16Var = 14;
            int int32Var = -22;
            uint uInt32Var = 22;
            int? nullInt32Var = -65;
            uint? nullUInt32Var = 43;
            long int64Var = -54;
            ulong uInt64Var = 52;
            long? nullInt64Var = -943;
            ulong? nullUInt64Var = 2344;
            double doubleVar = -23492834.33234;
            double? nullDoubleVar = -234234.423423434;
            decimal decimalVar = -342934934.2342m;
            decimal? nullDecimalVar = 31928318723.1123m;
            float floatVar = 129381923.123f;
            float? nullFloatVar = -1231231.12f;
            bool boolVar = true;
            bool? nullBoolVar = false;
            DateTime dateTimeVar = nowDt;
            DateTime? nullDateTimeVar = nowDt;
            DateTimeOffset dateTimeOffsetVar = nowOff;
            DateTimeOffset? nullDateTimeOffsetVar = nowOff;
            TimeSpan timeSpanVar = ts;
            TimeSpan? nullTimeSpanVar = ts;
            byte byteVar = 1;
            byte? nullByteVar = 4;
            sbyte sByteVar = 9;
            sbyte? nullSByteVar = 12;
            Guid guidVar = Guid.NewGuid();
            Guid? nullGuidVar = Guid.NewGuid();
            SimpleUrlBindingEnum enumVar = SimpleUrlBindingEnum.Four;
            SimpleUrlBindingEnum? nullEnumVar = SimpleUrlBindingEnum.Eight;

            string qs = "?";
            qs += $"stringVar={UrlEncode(stringVar)}&";
            qs += $"charVar={charVar}&";
            qs += $"nullCharVar={nullCharVar}&";
            qs += $"int16Var={int16Var}&";
            qs += $"uInt16Var={uInt16Var}&";
            qs += $"nullInt16Var={nullInt16Var}&";
            qs += $"nullUInt16Var={nullUInt16Var}&";
            qs += $"int32Var={int32Var}&";
            qs += $"uInt32Var={uInt32Var}&";
            qs += $"nullInt32Var={nullInt32Var}&";
            qs += $"nullUInt32Var={nullUInt32Var}&";
            qs += $"int64Var={int64Var}&";
            qs += $"uInt64Var={uInt64Var}&";
            qs += $"nullInt64Var={nullInt64Var}&";
            qs += $"nullUInt64Var={nullUInt64Var}&";
            qs += $"doubleVar={doubleVar}&";
            qs += $"nullDoubleVar={nullDoubleVar}&";
            qs += $"decimalVar={decimalVar}&";
            qs += $"nullDecimalVar={nullDecimalVar}&";
            qs += $"floatVar={floatVar}&";
            qs += $"nullFloatVar={nullFloatVar}&";
            qs += $"boolVar={boolVar}&";
            qs += $"nullBoolVar={nullBoolVar}&";
            qs += $"dateTimeVar={UrlEncode(dateTimeVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"nullDateTimeVar={UrlEncode(nullDateTimeVar.Value.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"dateTimeOffsetVar={UrlEncode(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"nullDateTimeOffsetVar={UrlEncode(nullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"timeSpanVar={UrlEncode(timeSpanVar.ToString())}&";
            qs += $"nullTimeSpanVar={UrlEncode(nullTimeSpanVar.ToString())}&";
            qs += $"byteVar={byteVar}&";
            qs += $"nullByteVar={nullByteVar}&";
            qs += $"sByteVar={sByteVar}&";
            qs += $"nullSByteVar={nullSByteVar}&";
            qs += $"guidVar={guidVar}&";
            qs += $"nullGuidVar={nullGuidVar}&";
            qs += $"enumVar={enumVar}&";
            qs += $"nullEnumVar={nullEnumVar}";

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{query_url}{qs} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

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
                expectedContentLength: 898,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task HEAD_binding_simple_url_querystring_case_insensitive_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            var nowDt = DateTime.Now;
            var nowOff = DateTimeOffset.Now;
            var ts = TimeSpan.FromSeconds(1);

            string stringVar = "test";
            char charVar = 'A';
            char? nullCharVar = 'A';
            short int16Var = -10;
            ushort uInt16Var = 12;
            short? nullInt16Var = -13;
            ushort? nullUInt16Var = 14;
            int int32Var = -22;
            uint uInt32Var = 22;
            int? nullInt32Var = -65;
            uint? nullUInt32Var = 43;
            long int64Var = -54;
            ulong uInt64Var = 52;
            long? nullInt64Var = -943;
            ulong? nullUInt64Var = 2344;
            double doubleVar = -23492834.33234;
            double? nullDoubleVar = -234234.423423434;
            decimal decimalVar = -342934934.2342m;
            decimal? nullDecimalVar = 31928318723.1123m;
            float floatVar = 129381923.123f;
            float? nullFloatVar = -1231231.12f;
            bool boolVar = true;
            bool? nullBoolVar = false;
            DateTime dateTimeVar = nowDt;
            DateTime? nullDateTimeVar = nowDt;
            DateTimeOffset dateTimeOffsetVar = nowOff;
            DateTimeOffset? nullDateTimeOffsetVar = nowOff;
            TimeSpan timeSpanVar = ts;
            TimeSpan? nullTimeSpanVar = ts;
            byte byteVar = 1;
            byte? nullByteVar = 4;
            sbyte sByteVar = 9;
            sbyte? nullSByteVar = 12;
            Guid guidVar = Guid.NewGuid();
            Guid? nullGuidVar = Guid.NewGuid();
            SimpleUrlBindingEnum enumVar = SimpleUrlBindingEnum.Four;
            SimpleUrlBindingEnum? nullEnumVar = SimpleUrlBindingEnum.Eight;

            string qs = "?";
            qs += $"StringVar={UrlEncode(stringVar)}&";
            qs += $"CharVar={charVar}&";
            qs += $"NullCharVar={nullCharVar}&";
            qs += $"Int16Var={int16Var}&";
            qs += $"UInt16Var={uInt16Var}&";
            qs += $"NullInt16Var={nullInt16Var}&";
            qs += $"NullUInt16Var={nullUInt16Var}&";
            qs += $"Int32Var={int32Var}&";
            qs += $"UInt32Var={uInt32Var}&";
            qs += $"NullInt32Var={nullInt32Var}&";
            qs += $"NullUInt32Var={nullUInt32Var}&";
            qs += $"Int64Var={int64Var}&";
            qs += $"UInt64Var={uInt64Var}&";
            qs += $"NullInt64Var={nullInt64Var}&";
            qs += $"NullUInt64Var={nullUInt64Var}&";
            qs += $"DoubleVar={doubleVar}&";
            qs += $"NullDoubleVar={nullDoubleVar}&";
            qs += $"DecimalVar={decimalVar}&";
            qs += $"NullDecimalVar={nullDecimalVar}&";
            qs += $"FloatVar={floatVar}&";
            qs += $"NullFloatVar={nullFloatVar}&";
            qs += $"BoolVar={boolVar}&";
            qs += $"NullBoolVar={nullBoolVar}&";
            qs += $"DateTimeVar={UrlEncode(dateTimeVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"NullDateTimeVar={UrlEncode(nullDateTimeVar.Value.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"DateTimeOffsetVar={UrlEncode(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"NullDateTimeOffsetVar={UrlEncode(nullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"TimeSpanVar={UrlEncode(timeSpanVar.ToString())}&";
            qs += $"NullTimeSpanVar={UrlEncode(nullTimeSpanVar.ToString())}&";
            qs += $"ByteVar={byteVar}&";
            qs += $"NullByteVar={nullByteVar}&";
            qs += $"SByteVar={sByteVar}&";
            qs += $"NullSByteVar={nullSByteVar}&";
            qs += $"guidVar={guidVar}&";
            qs += $"nullGuidVar={nullGuidVar}&";
            qs += $"enumVar={enumVar}&";
            qs += $"nullEnumVar={nullEnumVar}";

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{query_url}{qs} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedContentLength: 898,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task HEAD_binding_simple_url_querystring_nulls_for_nullables_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            var nowDt = DateTime.Now;
            var nowOff = DateTimeOffset.Now;
            var ts = TimeSpan.FromSeconds(1);

            string stringVar = "test";
            char charVar = 'A';
            char? nullCharVar = null;
            short int16Var = -10;
            ushort uInt16Var = 12;
            short? nullInt16Var = null;
            ushort? nullUInt16Var = null;
            int int32Var = -22;
            uint uInt32Var = 22;
            int? nullInt32Var = null;
            uint? nullUInt32Var = null;
            long int64Var = -54;
            ulong uInt64Var = 52;
            long? nullInt64Var = null;
            ulong? nullUInt64Var = null;
            double doubleVar = -23492834.33234;
            double? nullDoubleVar = null;
            decimal decimalVar = -342934934.2342m;
            decimal? nullDecimalVar = null;
            float floatVar = 129381923.123f;
            float? nullFloatVar = null;
            bool boolVar = true;
            bool? nullBoolVar = null;
            DateTime dateTimeVar = nowDt;
            DateTimeOffset dateTimeOffsetVar = nowOff;
            DateTimeOffset? nullDateTimeOffsetVar = nowOff;
            TimeSpan timeSpanVar = ts;
            TimeSpan? nullTimeSpanVar = null;
            byte byteVar = 1;
            byte? nullByteVar = null;
            sbyte sByteVar = 9;
            sbyte? nullSByteVar = null;
            Guid guidVar = Guid.NewGuid();
            Guid? nullGuidVar = null;
            SimpleUrlBindingEnum enumVar = SimpleUrlBindingEnum.Four;
            SimpleUrlBindingEnum? nullEnumVar = null;

            string qs = "?";
            qs += $"stringVar={UrlEncode(stringVar)}&";
            qs += $"charVar={charVar}&";
            qs += $"nullCharVar={nullCharVar}&";
            qs += $"int16Var={int16Var}&";
            qs += $"uInt16Var={uInt16Var}&";
            qs += $"nullInt16Var={nullInt16Var}&";
            qs += $"nullUInt16Var={nullUInt16Var}&";
            qs += $"int32Var={int32Var}&";
            qs += $"uInt32Var={uInt32Var}&";
            qs += $"nullInt32Var={nullInt32Var}&";
            qs += $"nullUInt32Var={nullUInt32Var}&";
            qs += $"int64Var={int64Var}&";
            qs += $"uInt64Var={uInt64Var}&";
            qs += $"nullInt64Var={nullInt64Var}&";
            qs += $"nullUInt64Var={nullUInt64Var}&";
            qs += $"doubleVar={doubleVar}&";
            qs += $"nullDoubleVar={nullDoubleVar}&";
            qs += $"decimalVar={decimalVar}&";
            qs += $"nullDecimalVar={nullDecimalVar}&";
            qs += $"floatVar={floatVar}&";
            qs += $"nullFloatVar={nullFloatVar}&";
            qs += $"boolVar={boolVar}&";
            qs += $"nullBoolVar={nullBoolVar}&";
            qs += $"dateTimeVar={UrlEncode(dateTimeVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"nullDateTimeVar=&";
            qs += $"dateTimeOffsetVar={UrlEncode(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"nullDateTimeOffsetVar=&";
            qs += $"timeSpanVar={UrlEncode(timeSpanVar.ToString())}&";
            qs += $"nullTimeSpanVar={nullTimeSpanVar}&";
            qs += $"byteVar={byteVar}&";
            qs += $"nullByteVar={nullByteVar}&";
            qs += $"sByteVar={sByteVar}&";
            qs += $"nullSByteVar={nullSByteVar}&";
            qs += $"guidVar={guidVar}&";
            qs += $"nullGuidVar={nullGuidVar}&";
            qs += $"enumVar={enumVar}&";
            qs += $"nullEnumVar={nullEnumVar}";

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{query_url}{qs} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

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
                expectedContentLength: 418,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("0", 379)]
        [InlineData("false", 379)]
        [InlineData("False", 379)]
        [InlineData("1", 377)]
        [InlineData("true", 377)]
        [InlineData("True", 377)]
        public async Task HEAD_binding_simple_url_querystring_bool_vars_success(string value, int expectedContentLength)
        {
            base.SetupEnvironment(services =>
            {
            });

            var nowDt = DateTime.Now;
            var nowOff = DateTimeOffset.Now;
            var ts = TimeSpan.FromSeconds(1);

            string qs = "?";
            qs += $"boolVar={value}";
            qs += $"&nullBoolVar={value}";

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{query_url}{qs} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

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
                expectedContentLength: expectedContentLength,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task HEAD_binding_simple_url_querystring_unknown_variables_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            var nowDt = DateTime.Now;
            var nowOff = DateTimeOffset.Now;
            var ts = TimeSpan.FromSeconds(1);

            string stringVar = "test";
            char charVar = 'A';
            char? nullCharVar = 'A';
            short int16Var = -10;
            ushort uInt16Var = 12;
            short? nullInt16Var = -13;
            ushort? nullUInt16Var = 14;
            int int32Var = -22;
            uint uInt32Var = 22;
            int? nullInt32Var = -65;
            uint? nullUInt32Var = 43;
            long int64Var = -54;
            ulong uInt64Var = 52;
            long? nullInt64Var = -943;
            ulong? nullUInt64Var = 2344;
            double doubleVar = -23492834.33234;
            double? nullDoubleVar = -234234.423423434;
            decimal decimalVar = -342934934.2342m;
            decimal? nullDecimalVar = 31928318723.1123m;
            float floatVar = 129381923.123f;
            float? nullFloatVar = -1231231.12f;
            bool boolVar = true;
            bool? nullBoolVar = false;
            DateTime dateTimeVar = nowDt;
            DateTime? nullDateTimeVar = nowDt;
            DateTimeOffset dateTimeOffsetVar = nowOff;
            DateTimeOffset? nullDateTimeOffsetVar = nowOff;
            TimeSpan timeSpanVar = ts;
            TimeSpan? nullTimeSpanVar = ts;
            byte byteVar = 1;
            byte? nullByteVar = 4;
            sbyte sByteVar = 9;
            sbyte? nullSByteVar = 12;
            Guid guidVar = Guid.NewGuid();
            Guid? nullGuidVar = Guid.NewGuid();
            SimpleUrlBindingEnum enumVar = SimpleUrlBindingEnum.Four;
            SimpleUrlBindingEnum? nullEnumVar = SimpleUrlBindingEnum.Eight;

            string qs = "?";
            qs += $"stringVar1={UrlEncode(stringVar)}&";
            qs += $"charVar1={charVar}&";
            qs += $"nullCharVar1={nullCharVar}&";
            qs += $"int16Var1={int16Var}&";
            qs += $"uInt16Var1={uInt16Var}&";
            qs += $"nullInt16Var1={nullInt16Var}&";
            qs += $"nullUInt16Var1={nullUInt16Var}&";
            qs += $"int32Var1={int32Var}&";
            qs += $"uInt32Var1={uInt32Var}&";
            qs += $"nullInt32Var1={nullInt32Var}&";
            qs += $"nullUInt32Va1r={nullUInt32Var}&";
            qs += $"int64Var1={int64Var}&";
            qs += $"uInt64Var1={uInt64Var}&";
            qs += $"nullInt64Var1={nullInt64Var}&";
            qs += $"nullUInt64Var1={nullUInt64Var}&";
            qs += $"doubleVar1={doubleVar}&";
            qs += $"nullDoubleVar1={nullDoubleVar}&";
            qs += $"decimalVar1={decimalVar}&";
            qs += $"nullDecimalVar1={nullDecimalVar}&";
            qs += $"floatVar1={floatVar}&";
            qs += $"nullFloatVar1={nullFloatVar}&";
            qs += $"boolVar1={boolVar}&";
            qs += $"nullBoolVar1={nullBoolVar}&";
            qs += $"dateTimeVar1={UrlEncode(dateTimeVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"nullDateTimeVar1={UrlEncode(nullDateTimeVar.Value.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"dateTimeOffsetVar1={UrlEncode(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"nullDateTimeOffsetVar1={UrlEncode(nullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"timeSpanVar1={UrlEncode(timeSpanVar.ToString())}&";
            qs += $"nullTimeSpanVar1={UrlEncode(nullTimeSpanVar.ToString())}&";
            qs += $"byteVar1={byteVar}&";
            qs += $"nullByteVar1={nullByteVar}&";
            qs += $"sByteVa1r={sByteVar}&";
            qs += $"nullSByteVar1={nullSByteVar}&";
            qs += $"guidVar1={guidVar}&";
            qs += $"nullGuidVar1={nullGuidVar}&";
            qs += $"enumVar1={enumVar}&";
            qs += $"nullEnumVar1={nullEnumVar}";

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{query_url}{qs} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

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
                expectedContentLength: 359,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("4", 897)]
        [InlineData("8", 899)]
        [InlineData("Eight", 899)]
        [InlineData("Four", 897)]
        [InlineData("four", 897)]
        [InlineData("four,eight", 911)]
        [InlineData("12", 911)]
        public async Task HEAD_binding_simple_url_querystring_enum_values_success(string value, int expectedContentLength)
        {
            base.SetupEnvironment(services =>
            {
            });

            var nowDt = DateTime.Now;
            var nowOff = DateTimeOffset.Now;
            var ts = TimeSpan.FromSeconds(1);

            string stringVar = "test";
            char charVar = 'A';
            char? nullCharVar = 'A';
            short int16Var = -10;
            ushort uInt16Var = 12;
            short? nullInt16Var = -13;
            ushort? nullUInt16Var = 14;
            int int32Var = -22;
            uint uInt32Var = 22;
            int? nullInt32Var = -65;
            uint? nullUInt32Var = 43;
            long int64Var = -54;
            ulong uInt64Var = 52;
            long? nullInt64Var = -943;
            ulong? nullUInt64Var = 2344;
            double doubleVar = -23492834.33234;
            double? nullDoubleVar = -234234.423423434;
            decimal decimalVar = -342934934.2342m;
            decimal? nullDecimalVar = 31928318723.1123m;
            float floatVar = 129381923.123f;
            float? nullFloatVar = -1231231.12f;
            bool boolVar = true;
            bool? nullBoolVar = false;
            DateTime dateTimeVar = nowDt;
            DateTime? nullDateTimeVar = nowDt;
            DateTimeOffset dateTimeOffsetVar = nowOff;
            DateTimeOffset? nullDateTimeOffsetVar = nowOff;
            TimeSpan timeSpanVar = ts;
            TimeSpan? nullTimeSpanVar = ts;
            byte byteVar = 1;
            byte? nullByteVar = 4;
            sbyte sByteVar = 9;
            sbyte? nullSByteVar = 12;
            Guid guidVar = Guid.NewGuid();
            Guid? nullGuidVar = Guid.NewGuid();

            string qs = "?";
            qs += $"stringVar={UrlEncode(stringVar)}&";
            qs += $"charVar={charVar}&";
            qs += $"nullCharVar={nullCharVar}&";
            qs += $"int16Var={int16Var}&";
            qs += $"uInt16Var={uInt16Var}&";
            qs += $"nullInt16Var={nullInt16Var}&";
            qs += $"nullUInt16Var={nullUInt16Var}&";
            qs += $"int32Var={int32Var}&";
            qs += $"uInt32Var={uInt32Var}&";
            qs += $"nullInt32Var={nullInt32Var}&";
            qs += $"nullUInt32Var={nullUInt32Var}&";
            qs += $"int64Var={int64Var}&";
            qs += $"uInt64Var={uInt64Var}&";
            qs += $"nullInt64Var={nullInt64Var}&";
            qs += $"nullUInt64Var={nullUInt64Var}&";
            qs += $"doubleVar={doubleVar}&";
            qs += $"nullDoubleVar={nullDoubleVar}&";
            qs += $"decimalVar={decimalVar}&";
            qs += $"nullDecimalVar={nullDecimalVar}&";
            qs += $"floatVar={floatVar}&";
            qs += $"nullFloatVar={nullFloatVar}&";
            qs += $"boolVar={boolVar}&";
            qs += $"nullBoolVar={nullBoolVar}&";
            qs += $"dateTimeVar={UrlEncode(dateTimeVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"nullDateTimeVar={UrlEncode(nullDateTimeVar.Value.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"dateTimeOffsetVar={UrlEncode(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"nullDateTimeOffsetVar={UrlEncode(nullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"timeSpanVar={UrlEncode(timeSpanVar.ToString())}&";
            qs += $"nullTimeSpanVar={UrlEncode(nullTimeSpanVar.ToString())}&";
            qs += $"byteVar={byteVar}&";
            qs += $"nullByteVar={nullByteVar}&";
            qs += $"sByteVar={sByteVar}&";
            qs += $"nullSByteVar={nullSByteVar}&";
            qs += $"guidVar={guidVar}&";
            qs += $"nullGuidVar={nullGuidVar}&";
            qs += $"enumVar={value}&";
            qs += $"nullEnumVar={value}";

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{query_url}{qs} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

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
                expectedContentLength: expectedContentLength,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task HEAD_binding_simple_url_querystring_empty_values_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            string qs = "?";
            qs += $"stringVar=&";
            qs += $"charVar=&";
            qs += $"nullCharVar=&";
            qs += $"int16Var=&";
            qs += $"uInt16Var=&";
            qs += $"nullInt16Var=&";
            qs += $"nullUInt16Var=&";
            qs += $"int32Var=&";
            qs += $"uInt32Var=&";
            qs += $"nullInt32Var=&";
            qs += $"nullUInt32Var=&";
            qs += $"int64Var=&";
            qs += $"uInt64Var=&";
            qs += $"nullInt64Var=&";
            qs += $"nullUInt64Var=&";
            qs += $"doubleVar=&";
            qs += $"nullDoubleVar=&";
            qs += $"decimalVar=&";
            qs += $"nullDecimalVar=&";
            qs += $"floatVar=&";
            qs += $"nullFloatVar=&";
            qs += $"boolVar=&";
            qs += $"nullBoolVar=&";
            qs += $"dateTimeVar=&";
            qs += $"nullDateTimeVar=&";
            qs += $"dateTimeOffsetVar=&";
            qs += $"nullDateTimeOffsetVar=&";
            qs += $"timeSpanVar=&";
            qs += $"nullTimeSpanVar=&";
            qs += $"byteVar=&";
            qs += $"nullByteVar=&";
            qs += $"sByteVar=&";
            qs += $"nullSByteVar=&";
            qs += $"guidVar=&";
            qs += $"nullGuidVar=&";
            qs += $"enumVar=&";
            qs += $"nullEnumVar=";

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{query_url}{qs} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

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
                expectedContentLength: 359,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task HEAD_binding_simple_url_querystring_no_vars_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{query_url} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

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
                expectedContentLength: 359,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("?")]
        [InlineData("")]
        public async Task HEAD_binding_simple_url_querystring_no_query_success(string qs)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{query_url}{qs} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

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
                expectedContentLength: 359,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task HEAD_binding_simple_url_querystring_dup_query_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            string qs = "?";
            qs += $"stringVar=test1&";
            qs += $"stringVar=test2";

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{query_url}{qs} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

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
                expectedContentLength: 385,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        // --- SUCCESS QUERY STRING BINDING
        // -------------------------------

        [Fact]
        public async Task HEAD_binding_simple_url_route_correct_case_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            var nowDt = DateTime.Now;
            var nowOff = DateTimeOffset.Now;
            var ts = TimeSpan.FromSeconds(1);

            string stringVar = "test";
            char charVar = 'A';
            char? nullCharVar = 'A';
            short int16Var = -10;
            ushort uInt16Var = 12;
            short? nullInt16Var = -13;
            ushort? nullUInt16Var = 14;
            int int32Var = -22;
            uint uInt32Var = 22;
            int? nullInt32Var = -65;
            uint? nullUInt32Var = 43;
            long int64Var = -54;
            ulong uInt64Var = 52;
            long? nullInt64Var = -943;
            ulong? nullUInt64Var = 2344;
            double doubleVar = -23492834.33234;
            double? nullDoubleVar = -234234.423423434;
            decimal decimalVar = -342934934.2342m;
            decimal? nullDecimalVar = 31928318723.1123m;
            float floatVar = 129381923.123f;
            float? nullFloatVar = -1231231.12f;
            bool boolVar = true;
            bool? nullBoolVar = false;
            DateTime dateTimeVar = nowDt;
            DateTime? nullDateTimeVar = nowDt;
            DateTimeOffset dateTimeOffsetVar = nowOff;
            DateTimeOffset? nullDateTimeOffsetVar = nowOff;
            TimeSpan timeSpanVar = ts;
            TimeSpan? nullTimeSpanVar = ts;
            byte byteVar = 1;
            byte? nullByteVar = 4;
            sbyte sByteVar = 9;
            sbyte? nullSByteVar = 12;
            Guid guidVar = Guid.NewGuid();
            Guid? nullGuidVar = Guid.NewGuid();
            SimpleUrlBindingEnum enumVar = SimpleUrlBindingEnum.Four;
            SimpleUrlBindingEnum? nullEnumVar = SimpleUrlBindingEnum.Eight;

            string qs = "?";
            qs += $"stringVar={UrlEncode(stringVar + "test")}&";
            qs += $"charVar={charVar}&";
            qs += $"nullCharVar={nullCharVar}&";
            qs += $"int16Var={int16Var}&";
            qs += $"uInt16Var={uInt16Var}&";
            qs += $"nullInt16Var={nullInt16Var}&";
            qs += $"nullUInt16Var={nullUInt16Var}&";
            qs += $"int32Var={int32Var}&";
            qs += $"uInt32Var={uInt32Var}&";
            qs += $"nullInt32Var={nullInt32Var}&";
            qs += $"nullUInt32Var={nullUInt32Var}&";
            qs += $"int64Var={int64Var}&";
            qs += $"uInt64Var={uInt64Var}&";
            qs += $"nullInt64Var={nullInt64Var}&";
            qs += $"nullUInt64Var={nullUInt64Var}&";
            qs += $"doubleVar={doubleVar}&";
            qs += $"nullDoubleVar={nullDoubleVar}&";
            qs += $"decimalVar={decimalVar}&";
            qs += $"nullDecimalVar={nullDecimalVar}&";
            qs += $"floatVar={floatVar}&";
            qs += $"nullFloatVar={nullFloatVar}&";
            qs += $"boolVar={boolVar}&";
            qs += $"nullBoolVar={nullBoolVar}&";
            qs += $"dateTimeVar={UrlEncode(dateTimeVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"nullDateTimeVar={UrlEncode(nullDateTimeVar.Value.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"dateTimeOffsetVar={UrlEncode(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"nullDateTimeOffsetVar={UrlEncode(nullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"timeSpanVar={UrlEncode(timeSpanVar.ToString())}&";
            qs += $"nullTimeSpanVar={UrlEncode(nullTimeSpanVar.ToString())}&";
            qs += $"byteVar={byteVar}&";
            qs += $"nullByteVar={nullByteVar}&";
            qs += $"sByteVar={sByteVar}&";
            qs += $"nullSByteVar={nullSByteVar}&";
            qs += $"guidVar={guidVar}&";
            qs += $"nullGuidVar={nullGuidVar}&";
            qs += $"enumVar={enumVar}&";
            qs += $"nullEnumVar={nullEnumVar}";

            var routeUrl = $"/binding/simple/url/{stringVar}/resource";
            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{routeUrl}{qs} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

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
                expectedContentLength: 898,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        // --- SUCCESS QUERY STRING BINDING
        // -------------------------------

        [Fact]
        public async Task HEAD_binding_simple_url_mixed_correct_case_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            var nowDt = DateTime.Now;
            var nowOff = DateTimeOffset.Now;
            var ts = TimeSpan.FromSeconds(1);

            string stringVar = "test";
            char charVar = 'A';
            char? nullCharVar = 'A';
            short int16Var = -10;
            ushort uInt16Var = 12;
            short? nullInt16Var = -13;
            ushort? nullUInt16Var = 14;
            int int32Var = -22;
            uint uInt32Var = 22;
            int? nullInt32Var = -65;
            uint? nullUInt32Var = 43;
            long int64Var = -54;
            ulong uInt64Var = 52;
            long? nullInt64Var = -943;
            ulong? nullUInt64Var = 2344;
            double doubleVar = -23492834.33234;
            double? nullDoubleVar = -234234.423423434;
            decimal decimalVar = -342934934.2342m;
            decimal? nullDecimalVar = 31928318723.1123m;
            float floatVar = 129381923.123f;
            float? nullFloatVar = -1231231.12f;
            bool boolVar = true;
            bool? nullBoolVar = false;
            DateTime dateTimeVar = nowDt;
            DateTime? nullDateTimeVar = nowDt;
            DateTimeOffset dateTimeOffsetVar = nowOff;
            DateTimeOffset? nullDateTimeOffsetVar = nowOff;
            TimeSpan timeSpanVar = ts;
            TimeSpan? nullTimeSpanVar = ts;
            byte byteVar = 1;
            byte? nullByteVar = 4;
            sbyte sByteVar = 9;
            sbyte? nullSByteVar = 12;
            Guid guidVar = Guid.NewGuid();
            Guid? nullGuidVar = Guid.NewGuid();
            SimpleUrlBindingEnum enumVar = SimpleUrlBindingEnum.Four;
            SimpleUrlBindingEnum? nullEnumVar = SimpleUrlBindingEnum.Eight;

            string qs = "?";
            qs += $"stringVar={UrlEncode(stringVar + "test")}&";
            qs += $"charVar={charVar}&";
            qs += $"nullCharVar={nullCharVar}&";
            qs += $"int16Var={int16Var}&";
            qs += $"uInt16Var={uInt16Var}&";
            qs += $"nullInt16Var={nullInt16Var}&";
            qs += $"nullUInt16Var={nullUInt16Var}&";
            qs += $"int32Var={int32Var}&";
            qs += $"uInt32Var={uInt32Var}&";
            qs += $"nullInt32Var={nullInt32Var}&";
            qs += $"nullUInt32Var={nullUInt32Var}&";
            qs += $"int64Var={int64Var}&";
            qs += $"uInt64Var={uInt64Var}&";
            qs += $"nullInt64Var={nullInt64Var}&";
            qs += $"nullUInt64Var={nullUInt64Var}&";
            qs += $"doubleVar={doubleVar}&";
            qs += $"nullDoubleVar={nullDoubleVar}&";
            qs += $"decimalVar={decimalVar}&";
            qs += $"nullDecimalVar={nullDecimalVar}&";
            qs += $"floatVar={floatVar}&";
            qs += $"nullFloatVar={nullFloatVar}&";
            qs += $"boolVar={boolVar}&";
            qs += $"nullBoolVar={nullBoolVar}&";
            qs += $"dateTimeVar={UrlEncode(dateTimeVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"nullDateTimeVar={UrlEncode(nullDateTimeVar.Value.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"dateTimeOffsetVar={UrlEncode(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"nullDateTimeOffsetVar={UrlEncode(nullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture))}&";
            qs += $"timeSpanVar={UrlEncode(timeSpanVar.ToString())}&";
            qs += $"nullTimeSpanVar={UrlEncode(nullTimeSpanVar.ToString())}&";
            qs += $"byteVar={byteVar}&";
            qs += $"nullByteVar={nullByteVar}&";
            qs += $"sByteVar={sByteVar}&";
            qs += $"nullSByteVar={nullSByteVar}&";
            qs += $"guidVar={guidVar}&";
            qs += $"nullGuidVar={nullGuidVar}&";
            qs += $"enumVar={enumVar}&";
            qs += $"nullEnumVar={nullEnumVar}";

            var routeUrl = $"/binding/simple/url/{stringVar}/mixed";
            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{routeUrl}{qs} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

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
                expectedContentLength: 1799,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var list = await base.GetResponseData<IList<SimpleUrlBindingRs>>(response).ConfigureAwait(false);
            list.Should().BeNull();
        }

        // --- Failures
        // -------------------------------

        [Theory]
        [InlineData("CharVar", "aa", 143)]
        [InlineData("NullCharVar", "aa", 158)]
        [InlineData("Int16Var", "a", 144)]
        [InlineData("NullInt16Var", "a", 158)]
        [InlineData("UInt16Var", "a", 146)]
        [InlineData("UInt16Var", "-1", 147)]
        [InlineData("NullUInt16Var", "a", 159)]
        [InlineData("NullUInt16Var", "-1", 160)]
        [InlineData("Int32Var", "a", 144)]
        [InlineData("NullInt32Var", "a", 158)]
        [InlineData("UInt32Var", "a", 146)]
        [InlineData("UInt32Var", "-1", 147)]
        [InlineData("NullUInt32Var", "a", 159)]
        [InlineData("NullUInt32Var", "-1", 160)]
        [InlineData("Int64Var", "a", 144)]
        [InlineData("NullInt64Var", "a", 158)]
        [InlineData("UInt64Var", "a", 146)]
        [InlineData("UInt64Var", "-1", 147)]
        [InlineData("NullUInt64Var", "a", 159)]
        [InlineData("NullUInt64Var", "-1", 160)]
        [InlineData("DoubleVar", "a", 146)]
        [InlineData("NullDoubleVar", "a", 159)]
        [InlineData("DecimalVar", "a", 148)]
        [InlineData("NullDecimalVar", "a", 160)]
        [InlineData("FloatVar", "a", 145)]
        [InlineData("NullFloatVar", "a", 158)]
        [InlineData("BoolVar", "a", 145)]
        [InlineData("NullBoolVar", "a", 157)]
        [InlineData("DateTimeVar", "a", 150)]
        [InlineData("NullDateTimeVar", "a", 161)]
        [InlineData("DateTimeOffsetVar", "a", 162)]
        [InlineData("NullDateTimeOffsetVar", "a", 167)]
        [InlineData("TimeSpanVar", "a", 150)]
        [InlineData("NullTimeSpanVar", "a", 161)]
        [InlineData("ByteVar", "a", 142)]
        [InlineData("NullByteVar", "a", 157)]
        [InlineData("SByteVar", "a", 144)]
        [InlineData("NullSByteVar", "a", 158)]
        [InlineData("GuidVar", "a", 142)]
        [InlineData("NullGuidVar", "a", 157)]
        [InlineData("EnumVar", "a", 158)]
        [InlineData("NullEnumVar", "a", 157)]
        public async Task HEAD_binding_simple_url_querystring_unconvertable_char_type_fail(string varName, string value, int expectedContentLength)
        {
            base.SetupEnvironment(services =>
            {
            });

            string qs = $"?{UrlEncode(varName)}={UrlEncode(value)}";

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}{query_url}{qs} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedContentLength: expectedContentLength,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }
    }
}
