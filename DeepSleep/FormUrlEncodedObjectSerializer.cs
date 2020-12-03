namespace DeepSleep
{
    using DeepSleep.Formatting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// 
    /// </summary>
    public class FormUrlEncodedObjectSerializer : IFormUrlEncodedObjectSerializer
    {
        private readonly static Regex regexArrayReplace = new Regex(@"\[[0-9] {0,}\]");
        private readonly static JsonWriterOptions writerOptions;

        static FormUrlEncodedObjectSerializer()
        {
            writerOptions = new JsonWriterOptions
            {
                Indented = false,
                SkipValidation = false
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="urlDecoded"></param>
        /// <returns></returns>
        public virtual async Task<T> Deserialize<T>(string data, bool urlDecoded = false)
        {
            var obj = await this.Deserialize(data, typeof(T), urlDecoded).ConfigureAwait(false);

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
        /// <param name="urlDecode"></param>
        /// <returns></returns>
        public virtual async Task<object> Deserialize(string data, Type objType, bool urlDecode = false)
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
                    string sName = s.Length > 0
                        ? urlDecode ? s[0] : HttpUtility.UrlDecode(s[0])
                        : string.Empty;

                    string sValue = s.Length > 1
                        ? urlDecode ? s[1] : HttpUtility.UrlDecode(s[1])
                        : string.Empty;

                    var isRoot = !sName.Contains('.');
                    var parts = sName.Trim().Split('.', StringSplitOptions.RemoveEmptyEntries);

                    var parent = isRoot
                        ? "__root__"
                        : "__root__." + parts[..(parts.Length - 1)].Concatenate(".").Trim().TrimEnd('.');

                    var name = sName.Trim()[(sName.LastIndexOf('.') + 1)..].Trim();

                    if (parts[^1] == name && name.EndsWith(']'))
                    {
                        name = string.Empty;
                        parent = parent + "." + parts[^1];
                    }

                    return (
                        name: name,
                        value: sValue,
                        parent: parent,
                        parentIsArray: parent.EndsWith(']'),
                        parentIsPrimitiveArray: parent.EndsWith(']') && name == string.Empty,
                        isPrimitiveArrayItem: parent.EndsWith(']') && name == string.Empty,
                        parentArrayIndex: (parent.EndsWith(']'))
                            ? Convert.ToInt32(parent[(parent.LastIndexOf('[') + 1)..^1])
                            : -1
                    );
                })
                .Where(s => s.parentIsPrimitiveArray || !string.IsNullOrWhiteSpace(s.name))
                .Where(s => !string.IsNullOrWhiteSpace(s.value))
                .OrderBy(s => s.parent)
                .ThenByDescending(s => s.name)
                .ToList();

            var allParents = elements.SelectMany(e =>
            {
                var items = new List<(string parent, bool isArray, int arrayCount)>();
                if (string.IsNullOrWhiteSpace(e.parent))
                {
                    items.Add((
                        parent: e.parent,
                        isArray: false,
                        arrayCount: 0)
                    );
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

            // System.Diagnostics.Debug.Write(json);

            if (string.IsNullOrWhiteSpace(json))
            {
                json = "{}";
            }


            var obj = JsonSerializer.Deserialize(json, objType, JsonReaderSerializationOptions.ReaderOptions);
            return obj;
        }

        private void WriteObject(
            Utf8JsonWriter writer,
            string parentPath,
            string propertyName,
            IEnumerable<(string name, string value, string parent, bool parentIsArray, bool parentIsPrimitiveArray, bool isPrimitiveArrayItem, int parentArrayIndex)> elementPool,
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

                var elements = elementPool
                    .Where(e => e.parentIsArray)
                    .Where(e => e.parentArrayIndex >= 0)
                    .Where(e => e.parent.StartsWith($"{arrayParent}"));

                var primitiveElements = elementPool
                    .Where(e => e.parentIsArray)
                    .Where(e => e.parentArrayIndex >= 0)
                    .Where(e => e.isPrimitiveArrayItem)
                    .Where(e => e.parentIsPrimitiveArray)
                    .Where(e => e.parent[..(e.parent.LastIndexOf('['))] == arrayParent);

                if (elements.Any() || primitiveElements.Any())
                {
                    var arrayPropertyName = childArray.parent[(childArray.parent.LastIndexOf('.') + 1)..];

                    if (primitiveElements.Any())
                    {
                        writeStart();

                        this.WritePrimitiveArray(
                            writer,
                            arrayPropertyName,
                            primitiveElements);
                    }
                    else if(elements.Any())
                    {
                        writeStart();

                        this.WriteArray(
                            writer,
                            arrayParent,
                            arrayPropertyName,
                            childArray.arrayCount,
                            elementPool,
                            parentPool);
                    }
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
            IEnumerable<(string name, string value, string parent, bool parentIsArray, bool parentIsPrimitiveArray, bool isPrimitiveArrayItem, int parentArrayIndex)> elementPool,
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

        private void WritePrimitiveArray(
            Utf8JsonWriter writer,
            string propertyName,
            IEnumerable<(string name, string value, string parent, bool parentIsArray, bool parentIsPrimitiveArray, bool isPrimitiveArrayItem, int parentArrayIndex)> elementPool)
        {
            writer.WriteStartArray(propertyName);

            foreach (var element in elementPool)
            {
                writer.WriteStringValue(element.value);
            }

            writer.WriteEndArray();
        }
    }
}
