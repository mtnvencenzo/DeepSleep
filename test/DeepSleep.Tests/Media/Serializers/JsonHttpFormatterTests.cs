﻿namespace DeepSleep.Tests.Media.Serializers
{
    using DeepSleep.Media.Serializers;
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Xunit;

    public class JsonHttpFormatterTests
    {
        public class MyType
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public MyType Item { get; set; }
            public IList<MyType> Items { get; set; }
            public bool MyBool { get; set; }
            public DateTimeOffset MyDateTimeOffset { get; set; }
            public decimal MyDecimal { get; set; }
            public IList<string> PrimitiveItems { get; set; }
        }

        public class UserAccountRegisterBodyRq
        {
            public string FullName { get; set; }
            public string EmailAddress { get; set; }
            public string EmailAddressCompare { get; set; }
            public string Password { get; set; }
            public string PreferredCulture { get; set; }
            public string CaptchaConfirmation { get; set; }
            public DateTimeOffset CreatedOn { get; set; }
        }

        public class UserAccountLoginBodyRq
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
            public bool Persist { get; set; }
        }

        public class Operation
        {
            public string path { get; set; }
            public string op { get; set; }
            public string from { get; set; }
            public object value { get; set; }
        }

        [Fact]
        public async Task WritesObjectCorretly()
        {
            var formatter = new DeepSleepJsonMediaSerializer(null);
            long length = 0;

            var obj = new MyType
            {
                MyBool = true
            };

            using var ms = new MemoryStream();
            await formatter.WriteType(ms, obj, (l) => length = l).ConfigureAwait(false);

            ms.Length.Should().BeGreaterThan(0);
            length.Should().Be(ms.Length);
        }

        [Fact]
        public async Task ReadsJsonCorrectly2()
        {
            var json = @"{
    ""EmailAddress"": ""rvecchi+unittest@gmail.com"",
    ""Password"": ""my-ut-password"",
    ""Persist"": true
}";
            using var ms = new MemoryStream();
            using var writer = new StreamWriter(ms);

            await writer.WriteAsync(json).ConfigureAwait(false);
            writer.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            var formatter = new DeepSleepJsonMediaSerializer(null);
            await formatter.ReadType(ms, typeof(UserAccountLoginBodyRq)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ReadsJsonCorrectly4()
        {
            var json = @"{
    ""EmailAddress"": ""rvecchi+unittest@gmail.com"",
    ""Password"": ""my-ut-password""
}";
            using var ms = new MemoryStream();
            using var writer = new StreamWriter(ms);

            await writer.WriteAsync(json).ConfigureAwait(false);
            writer.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            var formatter = new DeepSleepJsonMediaSerializer(null);
            await formatter.ReadType(ms, typeof(UserAccountLoginBodyRq)).ConfigureAwait(false);
        }
    }
}
