namespace DeepSleep.Formatting.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    internal class ContentDispositionConverter : JsonConverter<ContentDisposition>
    {
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
