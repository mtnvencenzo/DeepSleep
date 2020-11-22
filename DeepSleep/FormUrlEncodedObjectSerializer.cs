namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class FormUrlEncodedObjectSerializer : IFormUrlEncodedObjectSerializer
    {
        private readonly static Regex regexArrayReplace = new Regex(@"\[[0-9] {0,}\]");
        private readonly static JsonSerializerOptions readerOptions;
        private readonly static JsonWriterOptions writerOptions;

        static FormUrlEncodedObjectSerializer()
        {
            writerOptions = new JsonWriterOptions
            {
                Indented = false,
                SkipValidation = false
            };

            readerOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = true,
                IncludeFields = false,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
            };

            readerOptions.Converters.Add(new NullableBooleanConverter());
            readerOptions.Converters.Add(new BooleanConverter());
            readerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
            readerOptions.Converters.Add(new NullableDateTimeConverter());
            readerOptions.Converters.Add(new DateTimeConverter());
            readerOptions.Converters.Add(new NullableDateTimeOffsetConverter());
            readerOptions.Converters.Add(new DateTimeOffsetConverter());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual async Task<T> Deserialize<T>(string data)
        {
            var obj = await this.Deserialize(data, typeof(T)).ConfigureAwait(false);

            if (obj == null)
            {
                return default;
            }

            return (T) obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public virtual async Task<object> Deserialize(string data, Type objType)
        {
            if (data == null)
            {
                return null;
            }

            var elements = (data.TrimStart('?').Split("&", StringSplitOptions.RemoveEmptyEntries) ?? new string[] { })
                .Where(s => s.Contains("="))
                .Select(s => s.Split('='))
                .Select(s =>
                {
                    var parts = s[0].Trim().Split('.', StringSplitOptions.RemoveEmptyEntries);
                    var parent = parts.Length == 1
                        ? "__root__"
                        : "__root__." + parts[..(parts.Length - 1)].Concatenate(".").Trim().TrimEnd('.');

                    return (
                        name: s[0].Trim()[(s[0].LastIndexOf('.') + 1)..],
                        value: s[1].Trim(),
                        parent: parent,
                        parentIsArray: parent.EndsWith(']'),
                        parentArrayIndex: (parent.EndsWith(']'))
                            ? Convert.ToInt32(parent[(parent.LastIndexOf('[') + 1)..^1])
                            : -1
                    );
                })
                .Where(s => !string.IsNullOrWhiteSpace(s.name))
                .Where(s => !string.IsNullOrWhiteSpace(s.value))
                .OrderBy(s => s.name)
                .ToList();

            var allParents = elements.SelectMany(e =>
            {
                var items = new List<(string parent, bool isArray, int arrayCount)>();
                if (string.IsNullOrWhiteSpace(e.parent))
                {
                    items.Add((parent: e.parent, isArray: false, arrayCount: 0));
                }
                else
                {

                    var parts = e.parent.Split('.', StringSplitOptions.RemoveEmptyEntries);

                    for (int i = parts.Length - 1; i >= 0; i--)
                    {
                        var parent = parts[..^i].Concatenate(".").Trim().TrimEnd('.');

                        items.Add((
                            parent: regexArrayReplace.Replace((parent.EndsWith(']')
                                ? parent[..(parent.LastIndexOf('['))].Trim().TrimEnd('.')
                                : parent.Trim().TrimEnd('.')), string.Empty),
                            isArray: parent.EndsWith(']'),
                            arrayCount: (parent.EndsWith(']'))
                                ? Convert.ToInt32(parent[(parent.LastIndexOf('[') + 1)..^1]) + 1
                                : 0
                        ));
                    }
                }

                return items;
            });

            allParents = allParents
                .Select(p =>
                {
                    if (p.isArray)
                    {
                        return (
                            parent: p.parent,
                            isArray: p.isArray,
                            arrayCount: allParents.Where(a => a.parent == p.parent).Max(a => a.arrayCount)
                        );
                    }

                    return p;
                })
                .Distinct()
                .OrderBy(e => e.parent)
                .ToList();


            string json = null;
            using (var ms = new MemoryStream())
            using (var writer = new Utf8JsonWriter(ms, writerOptions))
            {
                this.WriteObject(
                    writer,
                    "__root__",
                    string.Empty,
                    elements,
                    allParents);

                await writer.FlushAsync().ConfigureAwait(false);

                ms.Position = 0;
                ms.Seek(0, SeekOrigin.Begin);
                json = Encoding.UTF8.GetString(ms.ToArray());
            }

            var obj = JsonSerializer.Deserialize(json, objType, readerOptions);
            return obj;
        }

        private void WriteObject(
            Utf8JsonWriter writer,
            string parentPath,
            string propertyName,
            IEnumerable<(string name, string value, string parent, bool parentIsArray, int parentArrayIndex)> elementPool,
            IEnumerable<(string parent, bool isArray, int arrayCount)> parentPool)
        {
            bool wroteStart = false;

            void writeStart()
            {
                if (!wroteStart)
                {
                    wroteStart = true;

                    if (string.IsNullOrWhiteSpace(propertyName))
                    {
                        writer.WriteStartObject();
                    }
                    else
                    {
                        writer.WriteStartObject(propertyName);
                    }
                }
            }

            // Find child properties of the root object being written
            foreach (var childProperty in elementPool.Where(e => e.parent == parentPath))
            {
                writeStart();
                writer.WritePropertyName(childProperty.name);
                writer.WriteStringValue(childProperty.value);
            }


            var preparedParentPath = regexArrayReplace.Replace(parentPath, string.Empty);
            var pathMatch = new Regex($@"^{preparedParentPath}\.[a-z_A-Z0-9]{{0,}}$");

            foreach (var childObject in parentPool.Where(p => !p.isArray && pathMatch.IsMatch(p.parent)))
            {
                writeStart();

                this.WriteObject(
                    writer,
                    childObject.parent,
                    childObject.parent[(childObject.parent.LastIndexOf('.') + 1)..],
                    elementPool,
                    parentPool);
            }

            foreach (var childArray in parentPool.Where(p => p.isArray && pathMatch.IsMatch(p.parent)))
            {
                var arrayParent = parentPath + '.' + childArray.parent[(childArray.parent.LastIndexOf('.') + 1)..];

                var hasElements = elementPool
                    .Any(e => e.parent.StartsWith($"{parentPath}."));

                if (hasElements)
                {
                    writeStart();

                    this.WriteArray(
                        writer,
                        arrayParent,
                        childArray.parent[(childArray.parent.LastIndexOf('.') + 1)..],
                        childArray.arrayCount,
                        elementPool,
                        parentPool);
                }
            }

            if (wroteStart)
            {
                writer.WriteEndObject();
            }
        }

        private void WriteArray(
            Utf8JsonWriter writer,
            string parentPath,
            string propertyName,
            int arrayCount,
            IEnumerable<(string name, string value, string parent, bool parentIsArray, int parentArrayIndex)> elementPool,
            IEnumerable<(string parent, bool isArray, int arrayCount)> parentPool)
        {
            writer.WriteStartArray(propertyName);

            for (int i = 0; i < arrayCount; i++)
            {
                this.WriteObject(
                    writer,
                    $"{parentPath}[{i}]",
                    string.Empty,
                    elementPool,
                    parentPool);
            }

            writer.WriteEndArray();
        }

        #region Internal Json Converters

        private class NullableBooleanConverter : JsonConverter<bool?>
        {
            public override bool? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string value = reader.GetString();
                string chkValue = value?.ToLower();

                if (chkValue == null)
                {
                    return null;
                }
                if (chkValue.Equals("true"))
                {
                    return true;
                }
                if (chkValue.Equals("false"))
                {
                    return false;
                }

                throw new JsonException($"Value '{value}' cannot be converted to a boolean value");
            }
            public override void Write(Utf8JsonWriter writer, bool? value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        private class BooleanConverter : JsonConverter<bool>
        {
            public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string value = reader.GetString();
                string chkValue = value?.ToLower();

                if (chkValue == null)
                {
                    return false;
                }
                if (chkValue.Equals("true"))
                {
                    return true;
                }
                if (chkValue.Equals("false"))
                {
                    return false;
                }

                throw new JsonException($"Value '{value}' cannot be converted to a boolean value");
            }
            public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        private class NullableDateTimeConverter : JsonConverter<DateTime?>
        {
            public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string value = reader.GetString();

                if (value == null)
                {
                    return null;
                }

                if (DateTime.TryParse(value, out var result))
                {
                    return result;
                }


                throw new JsonException($"Value '{value}' cannot be converted to a datetime value");
            }
            public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        private class DateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string value = reader.GetString();

                if (value == null)
                {
                    return DateTime.MinValue;
                }

                if (DateTime.TryParse(value, out var result))
                {
                    return result;
                }


                throw new JsonException($"Value '{value}' cannot be converted to a datetime value");
            }
            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        private class NullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
        {
            public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string value = reader.GetString();

                if (value == null)
                {
                    return null;
                }

                if (DateTimeOffset.TryParse(value, out var result))
                {
                    return result;
                }


                throw new JsonException($"Value '{value}' cannot be converted to a datetime offset value");
            }
            public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        private class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
        {
            public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string value = reader.GetString();

                if (value == null)
                {
                    return DateTimeOffset.MinValue;
                }

                if (DateTime.TryParse(value, out var result))
                {
                    return result;
                }


                throw new JsonException($"Value '{value}' cannot be converted to a datetime offset value");
            }
            public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
