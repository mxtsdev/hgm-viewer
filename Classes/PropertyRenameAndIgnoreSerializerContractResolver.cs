using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace HgmViewer.Classes
{
    // Modified
    // From: https://github.com/RicoSuter/NSwag/blob/940431dfce7801595e529e39c93da7af2dacf978/src/NSwag.Core/Infrastructure/PropertyRenameAndIgnoreSerializerContractResolver.cs
    public class PropertyRenameAndIgnoreSerializerContractResolver : DefaultContractResolver
    {
        public static Type AnyType = typeof(object);

        private readonly Dictionary<Type, HashSet<string>> _ignores;
        private readonly Dictionary<Type, Dictionary<string, string>> _renames;

        public PropertyRenameAndIgnoreSerializerContractResolver()
        {
            _ignores = new Dictionary<Type, HashSet<string>>();
            _renames = new Dictionary<Type, Dictionary<string, string>>();
        }

        public void IgnoreProperty(Type type, params string[] jsonPropertyNames)
        {
            if (!_ignores.ContainsKey(type))
                _ignores[type] = new HashSet<string>();

            foreach (var prop in jsonPropertyNames)
                _ignores[type].Add(prop);
        }

        public void RenameProperty(Type type, string propertyName, string newJsonPropertyName)
        {
            if (!_renames.ContainsKey(type))
                _renames[type] = new Dictionary<string, string>();

            _renames[type][propertyName] = newJsonPropertyName;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (IsIgnored(property.DeclaringType, property.PropertyName))
            {
                property.ShouldSerialize = _ => false;
                property.Ignored = true;
            }

            if (IsRenamed(property.DeclaringType, property.PropertyName, out var newJsonPropertyName))
                property.PropertyName = newJsonPropertyName;

            return property;
        }

        private bool IsIgnored(Type type, string jsonPropertyName)
        {
            if (_ignores.ContainsKey(type))
                return _ignores[type].Contains(jsonPropertyName);

            // any type
            return _ignores.ContainsKey(AnyType) && _ignores[AnyType].Contains(jsonPropertyName);
        }

        private bool IsRenamed(Type type, string jsonPropertyName, out string newJsonPropertyName)
        {
            if (_renames.TryGetValue(type, out Dictionary<string, string> renames) && renames.TryGetValue(jsonPropertyName, out newJsonPropertyName))
            {
                return true;
            }

            if (_renames.TryGetValue(AnyType, out renames) && renames.TryGetValue(jsonPropertyName, out newJsonPropertyName))
            {
                return true;
            }

            newJsonPropertyName = null;
            return false;
        }
    }

    //https://stackoverflow.com/a/15228384
    public class ByteArrayConverter : JsonConverter
    {
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            byte[] data = (byte[])value;

            // Compose an array.
            writer.WriteStartArray();

            for (var i = 0; i < data.Length; i++)
            {
                writer.WriteValue(data[i]);
            }

            writer.WriteEndArray();
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                var byteList = new List<byte>();

                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonToken.Integer:
                            byteList.Add(Convert.ToByte(reader.Value));
                            break;

                        case JsonToken.EndArray:
                            return byteList.ToArray();

                        case JsonToken.Comment:
                            // skip
                            break;

                        default:
                            throw new Exception(
                            string.Format(
                                "Unexpected token when reading bytes: {0}",
                                reader.TokenType));
                    }
                }

                throw new Exception("Unexpected end when reading bytes.");
            }
            else
            {
                throw new Exception(
                    string.Format(
                        "Unexpected token parsing binary. "
                        + "Expected StartArray, got {0}.",
                        reader.TokenType));
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }
    }

    // https://stackoverflow.com/a/53245784
    public class CustomJsonTextWriter : JsonTextWriter
    {
        public CustomJsonTextWriter(TextWriter writer) : base(writer)
        {
        }

        protected override void WriteIndent()
        {
            if (WriteState != WriteState.Array)
                base.WriteIndent();
            else
                WriteIndentSpace();
        }
    }

    public static class JsonConvert_Kaitai
    {
        public static string SerializeObject(object value, Formatting formatting)
        {
            Type type = null;

            var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
            jsonResolver.IgnoreProperty(PropertyRenameAndIgnoreSerializerContractResolver.AnyType, "M_Root");
            jsonResolver.IgnoreProperty(PropertyRenameAndIgnoreSerializerContractResolver.AnyType, "M_Parent");
            jsonResolver.IgnoreProperty(PropertyRenameAndIgnoreSerializerContractResolver.AnyType, "M_Io");

            var settings = new JsonSerializerSettings
            {
                ContractResolver = jsonResolver
            };
            settings.Converters.Add(new ByteArrayConverter());

            var jsonSerializer = JsonSerializer.CreateDefault(settings);
            jsonSerializer.Formatting = formatting;

            var sb = new StringBuilder(256);
            var sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (var jsonWriter = new CustomJsonTextWriter(sw))
            {
                jsonWriter.Formatting = jsonSerializer.Formatting;

                jsonSerializer.Serialize(jsonWriter, value, type);
            }

            return sw.ToString();
        }
    }
}