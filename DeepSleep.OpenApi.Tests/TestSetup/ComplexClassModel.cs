namespace DeepSleep.OpenApi.Tests.TestSetup
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ComplexClassModel
    {
        public string MyProp1 { get; set; }

        public int MyInProp { get; set; }

        public int? MuNullableIntProp { get; set; }

        public int[] MyIntAray { get; set; }

        public IEnumerable<string> MyStringEnumerable { get; set; }

        public List<bool> MyBoolList { get; set; }

        public object MyObj { get; set; }

        public CustomObj NestedObj { get; set; }

        public Guid UniqId { get; set; }
    }
}
