namespace DeepSleep.Api.OpenApiCheckTests.v2
{
    using FluentAssertions;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class BasicObjectEndpointsTests : PipelineTestBase
    {
        [Fact]
        public Task basicobject_endpoints___basic_object_type_check()
        {
            var properties = typeof(Models.BasicObject)
                .GetProperties()
                .ToList();

            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.ByteObject)).PropertyType.Should().Be(typeof(int?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.DecimalObject)).PropertyType.Should().Be(typeof(double?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.DoubleObject)).PropertyType.Should().Be(typeof(double?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.EnumObject)).PropertyType.Should().Be(typeof(string));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.FloatObject)).PropertyType.Should().Be(typeof(double?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.GuidObject)).PropertyType.Should().Be(typeof(Guid?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.Int32)).PropertyType.Should().Be(typeof(int?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.NullableEnumObject)).PropertyType.Should().Be(typeof(string));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.NullableGuidObject)).PropertyType.Should().Be(typeof(Guid?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.NullableInt32)).PropertyType.Should().Be(typeof(int?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.SByteObject)).PropertyType.Should().Be(typeof(int?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.ShortObject)).PropertyType.Should().Be(typeof(int?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.StringObject)).PropertyType.Should().Be(typeof(string));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.TimeSpanObject)).PropertyType.Should().Be(typeof(string));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.UriObject)).PropertyType.Should().Be(typeof(string));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.BoolObject)).PropertyType.Should().Be(typeof(bool?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.DateTimeObject)).PropertyType.Should().Be(typeof(DateTimeOffset?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.DateTimeOffsetObject)).PropertyType.Should().Be(typeof(DateTimeOffset?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.NullableBoolObject)).PropertyType.Should().Be(typeof(bool?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.NullableDateTimeObject)).PropertyType.Should().Be(typeof(DateTimeOffset?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.NullableDateTimeOffsetObject)).PropertyType.Should().Be(typeof(DateTimeOffset?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.LongObject)).PropertyType.Should().Be(typeof(long?));
            properties.FirstOrDefault(p => p.Name == nameof(Models.BasicObject.CharObject)).PropertyType.Should().Be(typeof(string));

            return Task.CompletedTask;
        }

        [Fact]
        public async Task basicobject_endpoints___post_basic_object_model_no_doc_attributes_all_nulls()
        {
            using (var client = base.GetClient())
            {
                var body = new Models.BasicObject
                {
                };

                var response = await client.PostBasicObjectModelNoDocAttributesAsync(
                    id: 1,
                    cancellationToken: default,
                    body: body).ConfigureAwait(false);

                response.Should().NotBeNull();
                response.ByteObject.Should().Be(default(byte));
                response.DecimalObject.Should().Be((double)default(decimal));
                response.DoubleObject.Should().Be(default(double));
                response.EnumObject.Should().Be(Models.BasicEnum.None);
                response.FloatObject.Should().Be((double)default(float));
                response.GuidObject.Should().Be(default(Guid));
                response.Int32.Should().Be(default(int));
                response.NullableEnumObject.Should().BeNull();
                response.NullableGuidObject.Should().BeNull();
                response.NullableInt32.Should().BeNull();
                response.SByteObject.Should().Be(default(sbyte));
                response.ShortObject.Should().Be(default(short));
                response.StringObject.Should().BeNull();
                response.TimeSpanObject.Should().Be(default(TimeSpan).ToString());
                response.UriObject.Should().BeNull();
                response.BoolObject.Should().Be(default(bool));
                response.DateTimeObject.Should().Be(default(DateTime));
                response.DateTimeOffsetObject.Should().Be(default(DateTimeOffset));
                response.NullableBoolObject.Should().BeNull();
                response.NullableDateTimeObject.Should().BeNull();
                response.NullableDateTimeOffsetObject.Should().BeNull();
                response.LongObject.Should().Be(default(long));
                response.CharObject.Should().Be(Convert.ToString(default(char)));
            }
        }

        [Fact]
        public async Task basicobject_endpoints___post_basic_object_model_no_doc_attributes_all_non_nulls()
        {
            using (var client = base.GetClient())
            {
                var body = new Models.BasicObject
                {
                    ByteObject = 1,
                    DecimalObject = (double)1200.00,
                    DoubleObject = double.MaxValue - 11d,
                    EnumObject = Models.BasicEnum.Something2,
                    FloatObject = (double)20090.1,
                    GuidObject = Guid.NewGuid(),
                    Int32 = 100,
                    NullableEnumObject = Models.BasicEnum.Something1,
                    NullableGuidObject = Guid.NewGuid(),
                    NullableInt32 = 200,
                    SByteObject = 1,
                    ShortObject = (int)short.MaxValue,
                    StringObject = "Test",
                    TimeSpanObject = "12.00:00:01",
                    UriObject = "http://www.google.com",
                    BoolObject = true,
                    DateTimeObject = DateTime.Now,
                    DateTimeOffsetObject = DateTimeOffset.Now,
                    NullableBoolObject = true,
                    NullableDateTimeObject = DateTime.Now,
                    NullableDateTimeOffsetObject = DateTimeOffset.Now,
                    LongObject = long.MaxValue,
                    CharObject = "c"
                };


                var response = await client.PostBasicObjectModelNoDocAttributesAsync(
                    id: 1,
                    cancellationToken: default,
                    body: body).ConfigureAwait(false);


                response.Should().NotBeNull();
                response.ByteObject.Should().Be(body.ByteObject);
                response.DecimalObject.Should().Be(body.DecimalObject);
                response.DoubleObject.Should().Be(body.DoubleObject);
                response.EnumObject.Should().Be(body.EnumObject);
                response.FloatObject.Should().Be(body.FloatObject);
                response.GuidObject.Should().Be(body.GuidObject);
                response.Int32.Should().Be(body.Int32);
                response.NullableEnumObject.Should().Be(body.NullableEnumObject);
                response.NullableGuidObject.Should().Be(body.NullableGuidObject);
                response.NullableInt32.Should().Be(body.NullableInt32);
                response.SByteObject.Should().Be(body.SByteObject);
                response.ShortObject.Should().Be(body.ShortObject);
                response.StringObject.Should().Be(body.StringObject);
                response.TimeSpanObject.Should().Be(body.TimeSpanObject);
                response.UriObject.Should().Be(body.UriObject);
                response.BoolObject.Should().Be(body.BoolObject);
                response.DateTimeObject.Should().Be(body.DateTimeObject);
                response.DateTimeOffsetObject.Should().Be(body.DateTimeOffsetObject);
                response.NullableBoolObject.Should().Be(body.NullableBoolObject);
                response.NullableDateTimeObject.Should().Be(body.NullableDateTimeObject);
                response.NullableDateTimeOffsetObject.Should().Be(body.NullableDateTimeOffsetObject);
                response.LongObject.Should().Be(body.LongObject);
                response.CharObject.Should().Be(body.CharObject);
            }
        }
    }
}
