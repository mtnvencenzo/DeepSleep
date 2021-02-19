namespace DeepSleep.Tests.OpenApi.TestSetup
{
    using System;

    public class CustomObj
    {
        public int MyCusProp1 { get; set; }

        public long LongProp { get; set; }

        public DateTimeOffset DTOffsetProp { get; set; }

        public DateTime DTProp { get; set; }

        public Uri MyUri { get; set; }
    }
}
