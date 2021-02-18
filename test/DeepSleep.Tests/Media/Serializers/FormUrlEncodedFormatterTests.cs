namespace DeepSleep.Tests.Media.Serializers
{
    using DeepSleep.Media.Serializers;
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using Xunit;

    public class FormUrlEncodedFormatterTests
    {
        public class MyType
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public MyType Item { get; set; }
            public IList<MyType> Items { get; set; }
            public bool? MyBool { get; set; }
            public DateTimeOffset? MyDateTimeOffset { get; set; }
            public decimal MyDecimal { get; set; }
            public IList<string> PrimitiveItems { get; set; }
        }

        [Fact]
        public async Task BuildsJsonCorrectly()
        {
            var formatter = new DeepSleepFormUrlEncodedMediaSerializer(new FormUrlEncodedObjectSerializer());

            string offset = DateTimeOffset.Now.ToString("o", CultureInfo.InvariantCulture);

            var data =
                "Name=roottest" +
                "&Value=rootvalue" +
                "&PrimitiveItems[0]=prim0" +
                "&PrimitiveItems[1]=prim1" +
                "&MyBool=True" +
                "&Item.Name=itemObjName" +
                "&Item.Value=itemObjValue" +
                "&Item.Item.Name=itemItemObjName" +
                "&Item.Item.Value=itemItemObjValue" +
                "&Items[0].Name=" + HttpUtility.UrlEncode("4f8QBJIA+KbUqHBgGJz62FoG8iXauLCD8UFMr+YXh5w=") +
                "&Items[0].Value=item0Value" +
                "&Items[1].Name=item1Name" +
                "&Items[1].Value=item1Value" +
                "&Items[0].Items[1].Name=item1item1Name" +
                "&Items[0].Items[1].Value=item1item1Value" +
                "&Items[0].Items[0].Name=item1item0Name" +
                "&Items[0].Items[0].Value=item1item0Value" +
                "&Items[2].Items[0].Value=item2item0Value" +
                "&Item.Item.Item.Name=itemItemItemObjName" +
                "&Item.Item.Item.Value=itemItemItemObjValue" +
                "&Items[0].Items[1].Items[0].Items[0].MyBool=true" +
                "&Items[0].Items[1].Items[0].Items[0].MyDecimal=293839298.2212343" +
                $"&Items[0].Items[1].Items[0].Items[0].MyDateTimeOffset={HttpUtility.UrlEncode(offset)}" +
                "&Items[0].Items[1].Items[0].Items[0].MyBool=true" +
                "&Items[0].Items[1].Items[0].Items[0].PrimitiveItems[0]=0" +
                "&Items[0].Items[1].Items[0].Items[0].PrimitiveItems[1]=1" +
                "&Items[0].Items[1].Items[0].Items[0].PrimitiveItems[2]=2" +
                "&PrimitiveItems[2]=prim2";

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                ms.Position = 0;
                ms.Seek(0, SeekOrigin.Begin);

                var obj = await formatter.ReadType(ms, typeof(MyType)).ConfigureAwait(false);

                obj.Should().NotBeNull();
                obj.Should().BeOfType<MyType>();

                var o = obj as MyType;
                o.Name.Should().Be("roottest");
                o.Value.Should().Be("rootvalue");

                o.PrimitiveItems.Should().NotBeNull();
                o.PrimitiveItems.Should().HaveCount(3);
                o.PrimitiveItems[0].Should().Be("prim0");
                o.PrimitiveItems[1].Should().Be("prim1");
                o.PrimitiveItems[2].Should().Be("prim2");


                o.Item.Should().NotBeNull();
                o.Item.Name.Should().Be("itemObjName");
                o.Item.Value.Should().Be("itemObjValue");
                o.Item.PrimitiveItems.Should().BeNull();
                o.Item.Items.Should().BeNull();
                o.Item.Item.Should().NotBeNull();
                o.Item.Item.Name.Should().Be("itemItemObjName");
                o.Item.Item.Value.Should().Be("itemItemObjValue");
                o.Item.Item.PrimitiveItems.Should().BeNull();
                o.Item.Item.Items.Should().BeNull();
                o.Item.Item.Item.Should().NotBeNull();
                o.Item.Item.Item.Name.Should().Be("itemItemItemObjName");
                o.Item.Item.Item.Value.Should().Be("itemItemItemObjValue");
                o.Item.Item.Item.PrimitiveItems.Should().BeNull();
                o.Item.Item.Item.Items.Should().BeNull();

                o.Items.Should().NotBeNull();
                o.Items.Should().HaveCount(3);
                o.Items[0].Name.Should().Be("4f8QBJIA+KbUqHBgGJz62FoG8iXauLCD8UFMr+YXh5w=");
                o.Items[0].Value.Should().Be("item0Value");
                o.Items[0].PrimitiveItems.Should().BeNull();
                o.Items[0].Items.Should().NotBeNull();
                o.Items[0].Items.Should().HaveCount(2);
                o.Items[0].MyBool.Should().BeNull();
                o.Items[0].MyDecimal.Should().Be(0);
                o.Items[0].MyDateTimeOffset.Should().BeNull();
                o.Items[0].Items[0].Name.Should().Be("item1item0Name");
                o.Items[0].Items[0].Value.Should().Be("item1item0Value");
                o.Items[0].Items[0].PrimitiveItems.Should().BeNull();
                o.Items[0].Items[0].Item.Should().BeNull();
                o.Items[0].Items[0].Items.Should().BeNull();
                o.Items[0].Items[0].MyBool.Should().BeNull();
                o.Items[0].Items[0].MyDecimal.Should().Be(0);
                o.Items[0].Items[0].MyDateTimeOffset.Should().BeNull();
                o.Items[0].Items[1].Name.Should().Be("item1item1Name");
                o.Items[0].Items[1].Value.Should().Be("item1item1Value");
                o.Items[0].Items[1].PrimitiveItems.Should().BeNull();
                o.Items[0].Items[1].MyBool.Should().BeNull();
                o.Items[0].Items[1].MyDecimal.Should().Be(0);
                o.Items[0].Items[1].MyDateTimeOffset.Should().BeNull();
                o.Items[0].Items[1].Item.Should().BeNull();
                o.Items[0].Items[1].Items.Should().NotBeNull();
                o.Items[0].Items[1].Items.Should().HaveCount(1);
                o.Items[0].Items[1].Items[0].Name.Should().BeNull();
                o.Items[0].Items[1].Items[0].PrimitiveItems.Should().BeNull();
                o.Items[0].Items[1].Items[0].Value.Should().BeNull();
                o.Items[0].Items[1].Items[0].MyBool.Should().BeNull();
                o.Items[0].Items[1].Items[0].MyDecimal.Should().Be(0);
                o.Items[0].Items[1].Items[0].MyDateTimeOffset.Should().BeNull();
                o.Items[0].Items[1].Items[0].Items[0].Name.Should().BeNull();
                o.Items[0].Items[1].Items[0].Items[0].Value.Should().BeNull();
                o.Items[0].Items[1].Items[0].Items[0].MyBool.Should().BeTrue();
                o.Items[0].Items[1].Items[0].Items[0].MyDecimal.Should().Be(293839298.2212343m);
                o.Items[0].Items[1].Items[0].Items[0].MyDateTimeOffset.Value.ToString("o", CultureInfo.InvariantCulture).Should().Be(offset);

                o.Items[0].Items[1].Items[0].Items[0].PrimitiveItems.Should().NotBeNull();
                o.Items[0].Items[1].Items[0].Items[0].PrimitiveItems.Should().HaveCount(3);
                o.Items[0].Items[1].Items[0].Items[0].PrimitiveItems[0].Should().Be("0");
                o.Items[0].Items[1].Items[0].Items[0].PrimitiveItems[1].Should().Be("1");
                o.Items[0].Items[1].Items[0].Items[0].PrimitiveItems[2].Should().Be("2");


                o.Items[1].Name.Should().Be("item1Name");
                o.Items[1].Value.Should().Be("item1Value");
                o.Items[1].MyBool.Should().BeNull();
                o.Items[1].MyDecimal.Should().Be(0);
                o.Items[1].MyDateTimeOffset.Should().BeNull();
                o.Items[1].Items.Should().BeNull();
                o.Items[1].PrimitiveItems.Should().BeNull();


                o.Items[2].Name.Should().BeNull();
                o.Items[2].Value.Should().BeNull();
                o.Items[2].PrimitiveItems.Should().BeNull();
                o.Items[2].MyBool.Should().BeNull();
                o.Items[2].MyDecimal.Should().Be(0);
                o.Items[2].MyDateTimeOffset.Should().BeNull();
                o.Items[2].Items.Should().NotBeNull();
                o.Items[2].Items.Should().HaveCount(1);
                o.Items[2].Items[0].Name.Should().BeNull();
                o.Items[2].Items[0].Value.Should().Be("item2item0Value");
                o.Items[2].Items[0].PrimitiveItems.Should().BeNull();
                o.Items[2].Items[0].Item.Should().BeNull();
                o.Items[2].Items[0].Items.Should().BeNull();
                o.Items[2].Items[0].MyBool.Should().BeNull();
                o.Items[2].Items[0].MyDecimal.Should().Be(0);
                o.Items[2].Items[0].MyDateTimeOffset.Should().BeNull();
            }
        }
    }
}
