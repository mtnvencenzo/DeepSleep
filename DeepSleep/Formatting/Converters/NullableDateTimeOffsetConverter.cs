namespace DeepSleep.Formatting.Converters
{
    using System;
    using System.Globalization;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class NullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
    {
        /// <summary>Reads and converts the JSON to type</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.None)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string value = reader.GetString();

                if (value == null)
                {
                    return null;
                }

                return DateTimeOffset.Parse(value, CultureInfo.CurrentCulture);
            }

            throw new JsonException($"Value cannot be converted to a datetimeoffset? value");
        }

        /// <summary>Writes a specified value as JSON.</summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(value.Value);
            }
        }
    }
}
