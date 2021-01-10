namespace DeepSleep.Api.NetCore.Tests.Binding
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Binding;
    using global::Api.DeepSleep.Models;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Xunit;

    public class SimpleUrlBinding_GetTests : PipelineTestBase
    {
        // --- SUCCESS QUERY STRING BINDING
        // -------------------------------

        [Fact]
        public async Task GET_binding_simple_url_querystring_correct_case_success()
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
GET https://{host}/binding/simple/url{qs} HTTP/1.1
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().Be(boolVar);
            data.ByteVar.Should().Be(byteVar);
            data.CharVar.Should().Be(charVar);
            data.DateTimeOffsetVar.ToString(CultureInfo.CurrentCulture).Should().Be(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture));
            data.DateTimeVar.ToString(CultureInfo.CurrentCulture).Should().Be(dateTimeVar.ToUniversalTime().ToString(CultureInfo.CurrentCulture));
            data.DecimalVar.Should().Be(decimalVar);
            data.DoubleVar.Should().Be(doubleVar);
            data.FloatVar.Should().Be(floatVar);
            data.Int16Var.Should().Be(int16Var);
            data.Int32Var.Should().Be(int32Var);
            data.Int64Var.Should().Be(int64Var);
            data.NullBoolVar.Should().Be(nullBoolVar);
            data.NullByteVar.Should().Be(nullByteVar);
            data.NullCharVar.Should().Be(nullCharVar);
            data.NullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture).Should().Be(nullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture));
            data.NullDateTimeVar.Value.ToString(CultureInfo.CurrentCulture).Should().Be(nullDateTimeVar.Value.ToUniversalTime().ToString(CultureInfo.CurrentCulture));
            data.NullDecimalVar.Should().Be(nullDecimalVar);
            data.NullDoubleVar.Should().Be(nullDoubleVar);
            data.NullFloatVar.Should().Be(nullFloatVar);
            data.NullInt16Var.Should().Be(nullInt16Var);
            data.NullInt32Var.Should().Be(nullInt32Var);
            data.NullInt64Var.Should().Be(nullInt64Var);
            data.NullSByteVar.Should().Be(nullSByteVar);
            data.NullTimeSpanVar.Should().Be(nullTimeSpanVar);
            data.NullUInt16Var.Should().Be(nullUInt16Var);
            data.NullUInt32Var.Should().Be(nullUInt32Var);
            data.NullUInt64Var.Should().Be(nullUInt64Var);
            data.SByteVar.Should().Be(sByteVar);
            data.StringVar.Should().Be(stringVar);
            data.TimeSpanVar.Should().Be(timeSpanVar);
            data.UInt16Var.Should().Be(uInt16Var);
            data.UInt32Var.Should().Be(uInt32Var);
            data.UInt64Var.Should().Be(uInt64Var);
            data.GuidVar.Should().Be(guidVar);
            data.NullGuidVar.Should().Be(nullGuidVar);
            data.EnumVar.Should().Be(enumVar);
            data.NullEnumVar.Should().Be(nullEnumVar);
        }

        [Fact]
        public async Task GET_binding_simple_url_querystring_case_insensitive_success()
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
GET https://{host}/binding/simple/url{qs} HTTP/1.1
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().Be(boolVar);
            data.ByteVar.Should().Be(byteVar);
            data.CharVar.Should().Be(charVar);
            data.DateTimeOffsetVar.ToString(CultureInfo.CurrentCulture).Should().Be(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture));
            data.DateTimeVar.ToString(CultureInfo.CurrentCulture).Should().Be(dateTimeVar.ToUniversalTime().ToString(CultureInfo.CurrentCulture));
            data.DecimalVar.Should().Be(decimalVar);
            data.DoubleVar.Should().Be(doubleVar);
            data.FloatVar.Should().Be(floatVar);
            data.Int16Var.Should().Be(int16Var);
            data.Int32Var.Should().Be(int32Var);
            data.Int64Var.Should().Be(int64Var);
            data.NullBoolVar.Should().Be(nullBoolVar);
            data.NullByteVar.Should().Be(nullByteVar);
            data.NullCharVar.Should().Be(nullCharVar);
            data.NullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture).Should().Be(nullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture));
            data.NullDateTimeVar.Value.ToString(CultureInfo.CurrentCulture).Should().Be(nullDateTimeVar.Value.ToUniversalTime().ToString(CultureInfo.CurrentCulture));
            data.NullDecimalVar.Should().Be(nullDecimalVar);
            data.NullDoubleVar.Should().Be(nullDoubleVar);
            data.NullFloatVar.Should().Be(nullFloatVar);
            data.NullInt16Var.Should().Be(nullInt16Var);
            data.NullInt32Var.Should().Be(nullInt32Var);
            data.NullInt64Var.Should().Be(nullInt64Var);
            data.NullSByteVar.Should().Be(nullSByteVar);
            data.NullTimeSpanVar.Should().Be(nullTimeSpanVar);
            data.NullUInt16Var.Should().Be(nullUInt16Var);
            data.NullUInt32Var.Should().Be(nullUInt32Var);
            data.NullUInt64Var.Should().Be(nullUInt64Var);
            data.SByteVar.Should().Be(sByteVar);
            data.StringVar.Should().Be(stringVar);
            data.TimeSpanVar.Should().Be(timeSpanVar);
            data.UInt16Var.Should().Be(uInt16Var);
            data.UInt32Var.Should().Be(uInt32Var);
            data.UInt64Var.Should().Be(uInt64Var);
            data.GuidVar.Should().Be(guidVar);
            data.NullGuidVar.Should().Be(nullGuidVar);
            data.EnumVar.Should().Be(enumVar);
            data.NullEnumVar.Should().Be(nullEnumVar);
        }

        [Fact]
        public async Task GET_binding_simple_url_querystring_nulls_for_nullables_success()
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
            qs += $"nullTimeSpanVar=&";
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
GET https://{host}/binding/simple/url{qs} HTTP/1.1
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().Be(boolVar);
            data.ByteVar.Should().Be(byteVar);
            data.CharVar.Should().Be(charVar);
            data.DateTimeOffsetVar.ToString(CultureInfo.CurrentCulture).Should().Be(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture));
            data.DateTimeVar.ToString(CultureInfo.CurrentCulture).Should().Be(dateTimeVar.ToUniversalTime().ToString(CultureInfo.CurrentCulture));
            data.DecimalVar.Should().Be(decimalVar);
            data.DoubleVar.Should().Be(doubleVar);
            data.FloatVar.Should().Be(floatVar);
            data.Int16Var.Should().Be(int16Var);
            data.Int32Var.Should().Be(int32Var);
            data.Int64Var.Should().Be(int64Var);
            data.NullBoolVar.Should().Be(nullBoolVar);
            data.NullByteVar.Should().Be(nullByteVar);
            data.NullCharVar.Should().Be(nullCharVar);
            data.NullDateTimeOffsetVar.Should().BeNull();
            data.NullDateTimeVar.Should().BeNull();
            data.NullDecimalVar.Should().Be(nullDecimalVar);
            data.NullDoubleVar.Should().Be(nullDoubleVar);
            data.NullFloatVar.Should().Be(nullFloatVar);
            data.NullInt16Var.Should().Be(nullInt16Var);
            data.NullInt32Var.Should().Be(nullInt32Var);
            data.NullInt64Var.Should().Be(nullInt64Var);
            data.NullSByteVar.Should().Be(nullSByteVar);
            data.NullTimeSpanVar.Should().Be(nullTimeSpanVar);
            data.NullUInt16Var.Should().Be(nullUInt16Var);
            data.NullUInt32Var.Should().Be(nullUInt32Var);
            data.NullUInt64Var.Should().Be(nullUInt64Var);
            data.SByteVar.Should().Be(sByteVar);
            data.StringVar.Should().Be(stringVar);
            data.TimeSpanVar.Should().Be(timeSpanVar);
            data.UInt16Var.Should().Be(uInt16Var);
            data.UInt32Var.Should().Be(uInt32Var);
            data.UInt64Var.Should().Be(uInt64Var);
            data.GuidVar.Should().Be(guidVar);
            data.NullGuidVar.Should().Be(nullGuidVar);
            data.EnumVar.Should().Be(enumVar);
            data.NullEnumVar.Should().Be(nullEnumVar);
        }

        [Theory]
        [InlineData("0", false)]
        [InlineData("false", false)]
        [InlineData("False", false)]
        [InlineData("1", true)]
        [InlineData("true", true)]
        [InlineData("True", true)]
        public async Task GET_binding_simple_url_querystring_bool_vars_success(string value, bool expected)
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
GET https://{host}/binding/simple/url{qs} HTTP/1.1
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().Be(expected);
            data.ByteVar.Should().Be(default);
            data.CharVar.Should().Be(default);
            data.DateTimeOffsetVar.Should().Be(default);
            data.DateTimeVar.Should().Be(default);
            data.DecimalVar.Should().Be(default);
            data.DoubleVar.Should().Be(default);
            data.FloatVar.Should().Be(default);
            data.Int16Var.Should().Be(default);
            data.Int32Var.Should().Be(default);
            data.Int64Var.Should().Be(default);
            data.NullBoolVar.Should().Be(expected);
            data.NullByteVar.Should().BeNull();
            data.NullCharVar.Should().BeNull();
            data.NullDateTimeOffsetVar.Should().BeNull();
            data.NullDateTimeVar.Should().BeNull();
            data.NullDecimalVar.Should().BeNull();
            data.NullDoubleVar.Should().BeNull();
            data.NullFloatVar.Should().BeNull();
            data.NullInt16Var.Should().BeNull();
            data.NullInt32Var.Should().BeNull();
            data.NullInt64Var.Should().BeNull();
            data.NullSByteVar.Should().BeNull();
            data.NullTimeSpanVar.Should().BeNull();
            data.NullUInt16Var.Should().BeNull();
            data.NullUInt32Var.Should().BeNull();
            data.NullUInt64Var.Should().BeNull();
            data.SByteVar.Should().Be(default);
            data.StringVar.Should().Be(default);
            data.TimeSpanVar.Should().Be(default);
            data.UInt16Var.Should().Be(default);
            data.UInt32Var.Should().Be(default);
            data.UInt64Var.Should().Be(default);
            data.GuidVar.Should().Be(Guid.Empty);
            data.NullGuidVar.Should().BeNull();
            data.EnumVar.Should().Be(default(SimpleUrlBindingEnum));
            data.NullEnumVar.Should().BeNull();
        }

        [Fact]
        public async Task GET_binding_simple_url_querystring_unknown_variables_success()
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
GET https://{host}/binding/simple/url{qs} HTTP/1.1
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().Be(default);
            data.ByteVar.Should().Be(default);
            data.CharVar.Should().Be(default);
            data.DateTimeOffsetVar.Should().Be(default);
            data.DateTimeVar.Should().Be(default);
            data.DecimalVar.Should().Be(default);
            data.DoubleVar.Should().Be(default);
            data.FloatVar.Should().Be(default);
            data.Int16Var.Should().Be(default);
            data.Int32Var.Should().Be(default);
            data.Int64Var.Should().Be(default);
            data.NullBoolVar.Should().Be(null);
            data.NullByteVar.Should().Be(null);
            data.NullCharVar.Should().Be(null);
            data.NullDateTimeOffsetVar.Should().Be(null);
            data.NullDateTimeVar.Should().Be(null);
            data.NullDecimalVar.Should().Be(null);
            data.NullDoubleVar.Should().Be(null);
            data.NullFloatVar.Should().Be(null);
            data.NullInt16Var.Should().Be(null);
            data.NullInt32Var.Should().Be(null);
            data.NullInt64Var.Should().Be(null);
            data.NullSByteVar.Should().Be(null);
            data.NullTimeSpanVar.Should().Be(null);
            data.NullUInt16Var.Should().Be(null);
            data.NullUInt32Var.Should().Be(null);
            data.NullUInt64Var.Should().Be(null);
            data.SByteVar.Should().Be(default);
            data.StringVar.Should().Be(default);
            data.TimeSpanVar.Should().Be(default);
            data.UInt16Var.Should().Be(default);
            data.UInt32Var.Should().Be(default);
            data.UInt64Var.Should().Be(default);
            data.GuidVar.Should().Be(Guid.Empty);
            data.NullGuidVar.Should().BeNull();
            data.EnumVar.Should().Be(default(SimpleUrlBindingEnum));
            data.NullEnumVar.Should().BeNull();
        }

        [Theory]
        [InlineData("4", SimpleUrlBindingEnum.Four)]
        [InlineData("8", SimpleUrlBindingEnum.Eight)]
        [InlineData("Eight", SimpleUrlBindingEnum.Eight)]
        [InlineData("Four", SimpleUrlBindingEnum.Four)]
        [InlineData("four", SimpleUrlBindingEnum.Four)]
        [InlineData("four,eight", SimpleUrlBindingEnum.Four | SimpleUrlBindingEnum.Eight)]
        [InlineData("12", SimpleUrlBindingEnum.Four | SimpleUrlBindingEnum.Eight)]
        public async Task GET_binding_simple_url_querystring_enum_values_success(string value, SimpleUrlBindingEnum expected)
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
GET https://{host}/binding/simple/url{qs} HTTP/1.1
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().Be(boolVar);
            data.ByteVar.Should().Be(byteVar);
            data.CharVar.Should().Be(charVar);
            data.DateTimeOffsetVar.ToString(CultureInfo.CurrentCulture).Should().Be(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture));
            data.DateTimeVar.ToString(CultureInfo.CurrentCulture).Should().Be(dateTimeVar.ToUniversalTime().ToString(CultureInfo.CurrentCulture));
            data.DecimalVar.Should().Be(decimalVar);
            data.DoubleVar.Should().Be(doubleVar);
            data.FloatVar.Should().Be(floatVar);
            data.Int16Var.Should().Be(int16Var);
            data.Int32Var.Should().Be(int32Var);
            data.Int64Var.Should().Be(int64Var);
            data.NullBoolVar.Should().Be(nullBoolVar);
            data.NullByteVar.Should().Be(nullByteVar);
            data.NullCharVar.Should().Be(nullCharVar);
            data.NullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture).Should().Be(nullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture));
            data.NullDateTimeVar.Value.ToString(CultureInfo.CurrentCulture).Should().Be(nullDateTimeVar.Value.ToUniversalTime().ToString(CultureInfo.CurrentCulture));
            data.NullDecimalVar.Should().Be(nullDecimalVar);
            data.NullDoubleVar.Should().Be(nullDoubleVar);
            data.NullFloatVar.Should().Be(nullFloatVar);
            data.NullInt16Var.Should().Be(nullInt16Var);
            data.NullInt32Var.Should().Be(nullInt32Var);
            data.NullInt64Var.Should().Be(nullInt64Var);
            data.NullSByteVar.Should().Be(nullSByteVar);
            data.NullTimeSpanVar.Should().Be(nullTimeSpanVar);
            data.NullUInt16Var.Should().Be(nullUInt16Var);
            data.NullUInt32Var.Should().Be(nullUInt32Var);
            data.NullUInt64Var.Should().Be(nullUInt64Var);
            data.SByteVar.Should().Be(sByteVar);
            data.StringVar.Should().Be(stringVar);
            data.TimeSpanVar.Should().Be(timeSpanVar);
            data.UInt16Var.Should().Be(uInt16Var);
            data.UInt32Var.Should().Be(uInt32Var);
            data.UInt64Var.Should().Be(uInt64Var);
            data.GuidVar.Should().Be(guidVar);
            data.NullGuidVar.Should().Be(nullGuidVar);
            data.EnumVar.Should().Be(expected);
            data.NullEnumVar.Should().Be(expected);
        }

        [Fact]
        public async Task GET_binding_simple_url_querystring_empty_values_success()
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
GET https://{host}/binding/simple/url{qs} HTTP/1.1
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().Be(false);
            data.ByteVar.Should().Be(default);
            data.CharVar.Should().Be(default);
            data.DateTimeOffsetVar.Should().Be(default);
            data.DateTimeVar.Should().Be(default);
            data.DecimalVar.Should().Be(default);
            data.DoubleVar.Should().Be(default);
            data.FloatVar.Should().Be(default);
            data.Int16Var.Should().Be(default);
            data.Int32Var.Should().Be(default);
            data.Int64Var.Should().Be(default);
            data.NullBoolVar.Should().BeNull();
            data.NullByteVar.Should().BeNull();
            data.NullCharVar.Should().BeNull();
            data.NullDateTimeOffsetVar.Should().BeNull();
            data.NullDateTimeVar.Should().BeNull();
            data.NullDecimalVar.Should().BeNull();
            data.NullDoubleVar.Should().BeNull();
            data.NullFloatVar.Should().BeNull();
            data.NullInt16Var.Should().BeNull();
            data.NullInt32Var.Should().BeNull();
            data.NullInt64Var.Should().BeNull();
            data.NullSByteVar.Should().BeNull();
            data.NullTimeSpanVar.Should().BeNull();
            data.NullUInt16Var.Should().BeNull();
            data.NullUInt32Var.Should().BeNull();
            data.NullUInt64Var.Should().BeNull();
            data.SByteVar.Should().Be(default);
            data.StringVar.Should().Be(default);
            data.TimeSpanVar.Should().Be(default);
            data.UInt16Var.Should().Be(default);
            data.UInt32Var.Should().Be(default);
            data.UInt64Var.Should().Be(default);
            data.GuidVar.Should().Be(Guid.Empty);
            data.NullGuidVar.Should().BeNull();
            data.EnumVar.Should().Be(default(SimpleUrlBindingEnum));
            data.NullEnumVar.Should().BeNull();
        }

        [Fact]
        public async Task GET_binding_simple_url_querystring_no_vars_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/binding/simple/url HTTP/1.1
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().Be(false);
            data.ByteVar.Should().Be(default);
            data.CharVar.Should().Be(default);
            data.DateTimeOffsetVar.Should().Be(default);
            data.DateTimeVar.Should().Be(default);
            data.DecimalVar.Should().Be(default);
            data.DoubleVar.Should().Be(default);
            data.FloatVar.Should().Be(default);
            data.Int16Var.Should().Be(default);
            data.Int32Var.Should().Be(default);
            data.Int64Var.Should().Be(default);
            data.NullBoolVar.Should().BeNull();
            data.NullByteVar.Should().BeNull();
            data.NullCharVar.Should().BeNull();
            data.NullDateTimeOffsetVar.Should().BeNull();
            data.NullDateTimeVar.Should().BeNull();
            data.NullDecimalVar.Should().BeNull();
            data.NullDoubleVar.Should().BeNull();
            data.NullFloatVar.Should().BeNull();
            data.NullInt16Var.Should().BeNull();
            data.NullInt32Var.Should().BeNull();
            data.NullInt64Var.Should().BeNull();
            data.NullSByteVar.Should().BeNull();
            data.NullTimeSpanVar.Should().BeNull();
            data.NullUInt16Var.Should().BeNull();
            data.NullUInt32Var.Should().BeNull();
            data.NullUInt64Var.Should().BeNull();
            data.SByteVar.Should().Be(default);
            data.StringVar.Should().Be(default);
            data.TimeSpanVar.Should().Be(default);
            data.UInt16Var.Should().Be(default);
            data.UInt32Var.Should().Be(default);
            data.UInt64Var.Should().Be(default);
            data.GuidVar.Should().Be(Guid.Empty);
            data.NullGuidVar.Should().BeNull();
            data.EnumVar.Should().Be(default(SimpleUrlBindingEnum));
            data.NullEnumVar.Should().BeNull();
        }

        [Theory]
        [InlineData("?")]
        [InlineData("")]
        public async Task GET_binding_simple_url_querystring_no_query_success(string qs)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/binding/simple/url{qs} HTTP/1.1
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().Be(false);
            data.ByteVar.Should().Be(default);
            data.CharVar.Should().Be(default);
            data.DateTimeOffsetVar.Should().Be(default);
            data.DateTimeVar.Should().Be(default);
            data.DecimalVar.Should().Be(default);
            data.DoubleVar.Should().Be(default);
            data.FloatVar.Should().Be(default);
            data.Int16Var.Should().Be(default);
            data.Int32Var.Should().Be(default);
            data.Int64Var.Should().Be(default);
            data.NullBoolVar.Should().BeNull();
            data.NullByteVar.Should().BeNull();
            data.NullCharVar.Should().BeNull();
            data.NullDateTimeOffsetVar.Should().BeNull();
            data.NullDateTimeVar.Should().BeNull();
            data.NullDecimalVar.Should().BeNull();
            data.NullDoubleVar.Should().BeNull();
            data.NullFloatVar.Should().BeNull();
            data.NullInt16Var.Should().BeNull();
            data.NullInt32Var.Should().BeNull();
            data.NullInt64Var.Should().BeNull();
            data.NullSByteVar.Should().BeNull();
            data.NullTimeSpanVar.Should().BeNull();
            data.NullUInt16Var.Should().BeNull();
            data.NullUInt32Var.Should().BeNull();
            data.NullUInt64Var.Should().BeNull();
            data.SByteVar.Should().Be(default);
            data.StringVar.Should().Be(default);
            data.TimeSpanVar.Should().Be(default);
            data.UInt16Var.Should().Be(default);
            data.UInt32Var.Should().Be(default);
            data.UInt64Var.Should().Be(default);
            data.GuidVar.Should().Be(Guid.Empty);
            data.NullGuidVar.Should().BeNull();
            data.EnumVar.Should().Be(default(SimpleUrlBindingEnum));
            data.NullEnumVar.Should().BeNull();
        }

        [Fact]
        public async Task GET_binding_simple_url_querystring_dup_query_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            string qs = "?";
            qs += $"stringVar=test1&";
            qs += $"stringVar=test2";

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/binding/simple/url{qs} HTTP/1.1
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().Be(false);
            data.ByteVar.Should().Be(default);
            data.CharVar.Should().Be(default);
            data.DateTimeOffsetVar.Should().Be(default);
            data.DateTimeVar.Should().Be(default);
            data.DecimalVar.Should().Be(default);
            data.DoubleVar.Should().Be(default);
            data.FloatVar.Should().Be(default);
            data.Int16Var.Should().Be(default);
            data.Int32Var.Should().Be(default);
            data.Int64Var.Should().Be(default);
            data.NullBoolVar.Should().BeNull();
            data.NullByteVar.Should().BeNull();
            data.NullCharVar.Should().BeNull();
            data.NullDateTimeOffsetVar.Should().BeNull();
            data.NullDateTimeVar.Should().BeNull();
            data.NullDecimalVar.Should().BeNull();
            data.NullDoubleVar.Should().BeNull();
            data.NullFloatVar.Should().BeNull();
            data.NullInt16Var.Should().BeNull();
            data.NullInt32Var.Should().BeNull();
            data.NullInt64Var.Should().BeNull();
            data.NullSByteVar.Should().BeNull();
            data.NullTimeSpanVar.Should().BeNull();
            data.NullUInt16Var.Should().BeNull();
            data.NullUInt32Var.Should().BeNull();
            data.NullUInt64Var.Should().BeNull();
            data.SByteVar.Should().Be(default);
            data.StringVar.Should().Be("test1,test2");
            data.TimeSpanVar.Should().Be(default);
            data.UInt16Var.Should().Be(default);
            data.UInt32Var.Should().Be(default);
            data.UInt64Var.Should().Be(default);
            data.GuidVar.Should().Be(Guid.Empty);
            data.NullGuidVar.Should().BeNull();
            data.EnumVar.Should().Be(default(SimpleUrlBindingEnum));
            data.NullEnumVar.Should().BeNull();
        }

        // --- SUCCESS QUERY STRING BINDING
        // -------------------------------

        [Fact]
        public async Task GET_binding_simple_url_route_correct_case_success()
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
GET https://{host}{routeUrl}{qs} HTTP/1.1
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().Be(boolVar);
            data.ByteVar.Should().Be(byteVar);
            data.CharVar.Should().Be(charVar);
            data.DateTimeOffsetVar.ToString(CultureInfo.CurrentCulture).Should().Be(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture));
            data.DateTimeVar.ToString(CultureInfo.CurrentCulture).Should().Be(dateTimeVar.ToUniversalTime().ToString(CultureInfo.CurrentCulture));
            data.DecimalVar.Should().Be(decimalVar);
            data.DoubleVar.Should().Be(doubleVar);
            data.FloatVar.Should().Be(floatVar);
            data.Int16Var.Should().Be(int16Var);
            data.Int32Var.Should().Be(int32Var);
            data.Int64Var.Should().Be(int64Var);
            data.NullBoolVar.Should().Be(nullBoolVar);
            data.NullByteVar.Should().Be(nullByteVar);
            data.NullCharVar.Should().Be(nullCharVar);
            data.NullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture).Should().Be(nullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture));
            data.NullDateTimeVar.Value.ToString(CultureInfo.CurrentCulture).Should().Be(nullDateTimeVar.Value.ToUniversalTime().ToString(CultureInfo.CurrentCulture));
            data.NullDecimalVar.Should().Be(nullDecimalVar);
            data.NullDoubleVar.Should().Be(nullDoubleVar);
            data.NullFloatVar.Should().Be(nullFloatVar);
            data.NullInt16Var.Should().Be(nullInt16Var);
            data.NullInt32Var.Should().Be(nullInt32Var);
            data.NullInt64Var.Should().Be(nullInt64Var);
            data.NullSByteVar.Should().Be(nullSByteVar);
            data.NullTimeSpanVar.Should().Be(nullTimeSpanVar);
            data.NullUInt16Var.Should().Be(nullUInt16Var);
            data.NullUInt32Var.Should().Be(nullUInt32Var);
            data.NullUInt64Var.Should().Be(nullUInt64Var);
            data.SByteVar.Should().Be(sByteVar);
            data.StringVar.Should().Be(stringVar);
            data.TimeSpanVar.Should().Be(timeSpanVar);
            data.UInt16Var.Should().Be(uInt16Var);
            data.UInt32Var.Should().Be(uInt32Var);
            data.UInt64Var.Should().Be(uInt64Var);
            data.GuidVar.Should().Be(guidVar);
            data.NullGuidVar.Should().Be(nullGuidVar);
            data.EnumVar.Should().Be(enumVar);
            data.NullEnumVar.Should().Be(nullEnumVar);
        }

        // --- SUCCESS QUERY STRING BINDING
        // -------------------------------

        [Fact]
        public async Task GET_binding_simple_url_mixed_correct_case_success()
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
GET https://{host}{routeUrl}{qs} HTTP/1.1
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var list = await base.GetResponseData<IList<SimpleUrlBindingRs>>(response).ConfigureAwait(false);
            list.Should().NotBeNull();
            list.Should().HaveCount(2);

            foreach (var data in list)
            {
                data.BoolVar.Should().Be(boolVar);
                data.ByteVar.Should().Be(byteVar);
                data.CharVar.Should().Be(charVar);
                data.DateTimeOffsetVar.ToString(CultureInfo.CurrentCulture).Should().Be(dateTimeOffsetVar.ToString(CultureInfo.CurrentCulture));
                data.DateTimeVar.ToString(CultureInfo.CurrentCulture).Should().Be(dateTimeVar.ToUniversalTime().ToString(CultureInfo.CurrentCulture));
                data.DecimalVar.Should().Be(decimalVar);
                data.DoubleVar.Should().Be(doubleVar);
                data.FloatVar.Should().Be(floatVar);
                data.Int16Var.Should().Be(int16Var);
                data.Int32Var.Should().Be(int32Var);
                data.Int64Var.Should().Be(int64Var);
                data.NullBoolVar.Should().Be(nullBoolVar);
                data.NullByteVar.Should().Be(nullByteVar);
                data.NullCharVar.Should().Be(nullCharVar);
                data.NullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture).Should().Be(nullDateTimeOffsetVar.Value.ToString(CultureInfo.CurrentCulture));
                data.NullDateTimeVar.Value.ToString(CultureInfo.CurrentCulture).Should().Be(nullDateTimeVar.Value.ToUniversalTime().ToString(CultureInfo.CurrentCulture));
                data.NullDecimalVar.Should().Be(nullDecimalVar);
                data.NullDoubleVar.Should().Be(nullDoubleVar);
                data.NullFloatVar.Should().Be(nullFloatVar);
                data.NullInt16Var.Should().Be(nullInt16Var);
                data.NullInt32Var.Should().Be(nullInt32Var);
                data.NullInt64Var.Should().Be(nullInt64Var);
                data.NullSByteVar.Should().Be(nullSByteVar);
                data.NullTimeSpanVar.Should().Be(nullTimeSpanVar);
                data.NullUInt16Var.Should().Be(nullUInt16Var);
                data.NullUInt32Var.Should().Be(nullUInt32Var);
                data.NullUInt64Var.Should().Be(nullUInt64Var);
                data.SByteVar.Should().Be(sByteVar);
                data.StringVar.Should().Be(stringVar);
                data.TimeSpanVar.Should().Be(timeSpanVar);
                data.UInt16Var.Should().Be(uInt16Var);
                data.UInt32Var.Should().Be(uInt32Var);
                data.UInt64Var.Should().Be(uInt64Var);
                data.GuidVar.Should().Be(guidVar);
                data.NullGuidVar.Should().Be(nullGuidVar);
                data.EnumVar.Should().Be(enumVar);
                data.NullEnumVar.Should().Be(nullEnumVar);
            }
        }

        // --- Failures
        // -------------------------------

        [Theory]
        [InlineData("CharVar", "aa", typeof(char), "charVar", 143)]
        [InlineData("NullCharVar", "aa", typeof(char?), "nullCharVar", 158)]
        [InlineData("Int16Var", "a", typeof(short), "int16Var", 144)]
        [InlineData("NullInt16Var", "a", typeof(short?), "nullInt16Var", 158)]
        [InlineData("UInt16Var", "a", typeof(ushort), "uInt16Var", 146)]
        [InlineData("UInt16Var", "-1", typeof(ushort), "uInt16Var", 147)]
        [InlineData("NullUInt16Var", "a", typeof(ushort?), "nullUInt16Var", 159)]
        [InlineData("NullUInt16Var", "-1", typeof(ushort?), "nullUInt16Var", 160)]
        [InlineData("Int32Var", "a", typeof(int), "int32Var", 144)]
        [InlineData("NullInt32Var", "a", typeof(int?), "nullInt32Var", 158)]
        [InlineData("UInt32Var", "a", typeof(uint), "uInt32Var", 146)]
        [InlineData("UInt32Var", "-1", typeof(uint), "uInt32Var", 147)]
        [InlineData("NullUInt32Var", "a", typeof(uint?), "nullUInt32Var", 159)]
        [InlineData("NullUInt32Var", "-1", typeof(uint?), "nullUInt32Var", 160)]
        [InlineData("Int64Var", "a", typeof(long), "int64Var", 144)]
        [InlineData("NullInt64Var", "a", typeof(long?), "nullInt64Var", 158)]
        [InlineData("UInt64Var", "a", typeof(ulong), "uInt64Var", 146)]
        [InlineData("UInt64Var", "-1", typeof(ulong), "uInt64Var", 147)]
        [InlineData("NullUInt64Var", "a", typeof(ulong?), "nullUInt64Var", 159)]
        [InlineData("NullUInt64Var", "-1", typeof(ulong?), "nullUInt64Var", 160)]
        [InlineData("DoubleVar", "a", typeof(double), "doubleVar", 146)]
        [InlineData("NullDoubleVar", "a", typeof(double?), "nullDoubleVar", 159)]
        [InlineData("DecimalVar", "a", typeof(decimal), "decimalVar", 148)]
        [InlineData("NullDecimalVar", "a", typeof(decimal?), "nullDecimalVar", 160)]
        [InlineData("FloatVar", "a", typeof(float), "floatVar", 145)]
        [InlineData("NullFloatVar", "a", typeof(float?), "nullFloatVar", 158)]
        [InlineData("BoolVar", "a", typeof(bool), "boolVar", 145)]
        [InlineData("NullBoolVar", "a", typeof(bool?), "nullBoolVar", 157)]
        [InlineData("DateTimeVar", "a", typeof(DateTime), "dateTimeVar", 150)]
        [InlineData("NullDateTimeVar", "a", typeof(DateTime?), "nullDateTimeVar", 161)]
        [InlineData("DateTimeOffsetVar", "a", typeof(DateTimeOffset), "dateTimeOffsetVar", 162)]
        [InlineData("NullDateTimeOffsetVar", "a", typeof(DateTimeOffset?), "nullDateTimeOffsetVar", 167)]
        [InlineData("TimeSpanVar", "a", typeof(TimeSpan), "timeSpanVar", 150)]
        [InlineData("NullTimeSpanVar", "a", typeof(TimeSpan?), "nullTimeSpanVar", 161)]
        [InlineData("ByteVar", "a", typeof(byte), "byteVar", 142)]
        [InlineData("NullByteVar", "a", typeof(byte?), "nullByteVar", 157)]
        [InlineData("SByteVar", "a", typeof(sbyte), "sByteVar", 144)]
        [InlineData("NullSByteVar", "a", typeof(sbyte?), "nullSByteVar", 158)]
        [InlineData("GuidVar", "a", typeof(Guid), "guidVar", 142)]
        [InlineData("NullGuidVar", "a", typeof(Guid?), "nullGuidVar", 157)]
        [InlineData("EnumVar", "a", typeof(SimpleUrlBindingEnum), "enumVar", 158)]
        [InlineData("NullEnumVar", "a", typeof(SimpleUrlBindingEnum?), "nullEnumVar", 157)]
        public async Task GET_binding_simple_url_querystring_unconvertable_char_type_fail(string varName, string value, Type expectedType, string expectedVarName, int expectedContentLength)
        {
            base.SetupEnvironment(services =>
            {
            });

            string qs = $"?{UrlEncode(varName)}={UrlEncode(value)}";

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/binding/simple/url{qs} HTTP/1.1
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
                expectedHttpStatus: 400,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedContentLength: expectedContentLength,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be($"Uri type conversion for '{expectedVarName}' with value '{value}' could not be converted to type {expectedType.Name}.");
        }

        [Theory]
        [InlineData("CharVar", "aa", 0)]
        public async Task GET_binding_simple_url_querystring_unconvertable_char_type_fail_with_empty_configured_error(string varName, string value, int expectedContentLength)
        {
            base.SetupEnvironment(services =>
            {
            });

            string qs = $"?{UrlEncode(varName)}={UrlEncode(value)}";

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/binding/simple/url/empty/binding/error{qs} HTTP/1.1
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
                expectedHttpStatus: 400,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedContentLength: expectedContentLength,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().BeNull();

            apiContext.Validation.Errors.Should().NotBeNull();
            apiContext.Validation.Errors.Should().BeEmpty();
        }

        [Theory]
        [InlineData("CharVar", "aa", 50)]
        public async Task GET_binding_simple_url_querystring_unconvertable_char_type_fail_with_custom_configured_error(string varName, string value, int expectedContentLength)
        {
            base.SetupEnvironment(services =>
            {
            });

            string qs = $"?{UrlEncode(varName)}={UrlEncode(value)}";

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/binding/simple/url/custom/binding/error{qs} HTTP/1.1
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
                expectedHttpStatus: 400,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedContentLength: expectedContentLength,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.Messages[0].ErrorMessageStr.Should().Be($"Test charVar.");

            apiContext.Validation.Errors.Should().NotBeNull();
            apiContext.Validation.Errors.Should().HaveCount(1);
        }
    }
}
