namespace DeepSleep.OpenApi.Tests.TestSetup
{
    using System;

    public class ComplexResponse
    {
        public string Name { get; set; }

        public string OtherName { get; set; }

        public DateTimeOffset? Offset { get; set; }

        public int Int { get; set; }

        public long Long { get; set; }
    }
}
