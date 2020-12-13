namespace DeepSleep.Formatting.Converters
{
    using System;
    using System.Globalization;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    internal class NullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
    {
        /// <summary>Reads and converts the JSON to type</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return DateTimeOffset.Parse(value, CultureInfo.CurrentCulture);
        }

        /// <summary>Writes a specified value as JSON.</summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
