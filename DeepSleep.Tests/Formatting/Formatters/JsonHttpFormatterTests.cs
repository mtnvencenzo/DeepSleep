﻿namespace DeepSleep.Tests.Formatting.Formatters
{
    using DeepSleep.Formatting.Formatters;
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
            public bool? MyBool { get; set; }
            public DateTimeOffset? MyDateTimeOffset { get; set; }
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
            public DateTimeOffset? CreatedOn { get; set; }
        }

        public class UserAccountLoginBodyRq
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
            public bool? Persist { get; set; }
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
            var formatter = new JsonHttpFormatter(null);
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
        public async Task ReadsJsonCorrectly()
        {
            var json = @"{
    ""FullName"": ""Ronaldo Vecchi"",
    ""EmailAddress"": ""rvecchi@gmail.com"",
    ""EmailAddressCompare"": ""rvecchi@gmail.com"",
    ""Password"": ""1298dhh((838Jk"",
    ""PreferredCulture"": ""en-US"",
    ""CaptchaConfirmation"": "" "",
    ""CreatedOn"": ""2020-11-10T09:00:00 -7:00""
}";
            using var ms = new MemoryStream();
            using var writer = new StreamWriter(ms);

            await writer.WriteAsync(json).ConfigureAwait(false);
            writer.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            var formatter = new JsonHttpFormatter(null);
            await formatter.ReadType(ms, typeof(UserAccountRegisterBodyRq)).ConfigureAwait(false);
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

            var formatter = new JsonHttpFormatter(null);
            await formatter.ReadType(ms, typeof(UserAccountLoginBodyRq)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ReadsJsonCorrectly3()
        {
            var json = @"{
    ""EmailAddress"": ""rvecchi+unittest@gmail.com"",
    ""Password"": ""my-ut-password"",
    ""Persist"": ""True""
}";
            using var ms = new MemoryStream();
            using var writer = new StreamWriter(ms);

            await writer.WriteAsync(json).ConfigureAwait(false);
            writer.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            var formatter = new JsonHttpFormatter(null);
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

            var formatter = new JsonHttpFormatter(null);
            await formatter.ReadType(ms, typeof(UserAccountLoginBodyRq)).ConfigureAwait(false);
        }

        [Fact]
        public async Task ReadsJsonCorrectly5()
        {
            var json = @"[
    { ""op"": ""replace"", ""path"": ""/Name"", ""value"": ""My Super Duper DUper Trip"" }
]";
            using var ms = new MemoryStream();
            using var writer = new StreamWriter(ms);

            await writer.WriteAsync(json).ConfigureAwait(false);
            writer.Flush();
            ms.Seek(0, SeekOrigin.Begin);

            var formatter = new JsonHttpFormatter(null);
            var ops = await formatter.ReadType(ms, typeof(IList<Operation>)).ConfigureAwait(false) as IList<Operation>;

            ops.Should().NotBeNull();
            ops.Should().HaveCount(1);
            ops[0].op.Should().Be("replace");
            ops[0].path.Should().Be("/Name");
            ops[0].from.Should().BeNull();
            ops[0].value.Should().Be("My Super Duper DUper Trip");
        }
    }
}