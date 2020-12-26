namespace DeepSleep.Tests.TestArtifacts
{
    using System;

    public class StandardNullableModel : StandardModelBase
    {
        public char? CharProp { get; set; }

        public byte? ByteProp { get; set; }

        public sbyte? SByteProp { get; set; }

        public bool? BoolProp { get; set; }

        public int? IntProp { get; set; }

        public uint? UIntProp { get; set; }

        public short? ShortProp { get; set; }

        public ushort? UShortProp { get; set; }

        public long? LongProp { get; set; }

        public ulong? ULongProp { get; set; }

        public double? DoubleProp { get; set; }

        public float? FloatProp { get; set; }

        public decimal? DecimalProp { get; set; }

        public DateTime? DateTimeProp { get; set; }

        public DateTimeOffset? DateTimeOfsetProp { get; set; }

        public TimeSpan? TimeSpanProp { get; set; }

        public Guid? GuidProp { get; set; }

        public StandardEnum? EnumProp { get; set; }
    }
}
