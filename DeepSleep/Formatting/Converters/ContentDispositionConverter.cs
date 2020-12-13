namespace DeepSleep.Formatting.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    internal class ContentDispositionConverter : JsonConverter<ContentDisposition>
    {
        /// <summary>Reads and converts the JSON to type</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override ContentDisposition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

                if (!string.IsNullOrWhiteSpace(value))
                {
                    return new ContentDisposition(value);
                }
            }

            return null;
        }

        /// <summary>Writes a specified value as JSON.</summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, ContentDisposition value, JsonSerializerOptions options)
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
            {
                writer.WriteStringValue(value.ToString());
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
