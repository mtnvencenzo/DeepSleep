namespace DeepSleep.Formatting.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public class ContentDispositionConverter : JsonConverter<ContentDispositionHeader>
    {
        /// <summary>Reads and converts the JSON to type</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override ContentDispositionHeader Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                else
                {
                    return new ContentDispositionHeader(value);
                }
            }

            return null;
        }

        /// <summary>Writes a specified value as JSON.</summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, ContentDispositionHeader value, JsonSerializerOptions options)
        {
            var disposition = value?.ToString();

            if (disposition == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStringValue(disposition);
            }
        }
    }
}
