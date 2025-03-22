using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetDream.Shared.Converters
{
    public class MetaConverter<T> : JsonConverter<T>
        where T : class, new()
    {


        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                return default;
            }

            var res = Activator.CreateInstance(typeToConvert);
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    reader.Read();
                    if (string.IsNullOrEmpty(propertyName))
                    {
                        continue;
                    }
                    MetaConverter<T>.ReadObject(ref reader, res, propertyName, options);
                }
            }
            return (T)res;
        }

        private static void ReadObject(ref Utf8JsonReader reader, object instance, string propertyName, JsonSerializerOptions options)
        {
            var type = instance.GetType();
            var field = type.GetField(propertyName);
            if (field is not null && field?.GetCustomAttribute<JsonMetaAttribute>() is null)
            {
                field?.SetValue(instance, JsonSerializer.Deserialize(ref reader, field.FieldType, options));
                return;
            }
            foreach (var item in type.GetFields())
            {
                if (item.GetCustomAttribute<JsonMetaAttribute>() is null)
                {
                    continue;
                }
                var meta = item.GetValue(instance);
                if (meta is null)
                {
                    meta = Activator.CreateInstance(item.FieldType);
                    item.SetValue(instance, meta);
                }
                if (meta is null)
                {
                    continue;
                }
                MetaConverter<T>.ReadObject(ref reader, meta, propertyName, options);
                return;
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            MetaConverter<T>.WriteObject(writer, value, options);
            writer.WriteEndObject();
        }

        private static void WriteObject(Utf8JsonWriter writer, object instance, JsonSerializerOptions options)
        {
            var type = instance.GetType();
            object? meta;
            foreach (var item in type.GetFields())
            {
                if (item.GetCustomAttribute<JsonMetaAttribute>() is null)
                {
                    meta = item.GetValue(instance);
                    if (meta is null)
                    {
                        continue;
                    }
                    MetaConverter<T>.WriteObject(writer, meta, options);
                    continue;
                }
                if (item.GetCustomAttribute<JsonIgnoreAttribute>() is not null)
                {
                    continue;
                }
                var val = item.GetValue(instance);
                if (val is null)
                {
                    continue;
                }
                var column = item.GetCustomAttribute<ColumnAttribute>();
                writer.WritePropertyName(column is null || string.IsNullOrEmpty(column.Name) ? item.Name : column.Name);
                JsonSerializer.Serialize(writer, val, options);
            }
        }
    }

    public class JsonMetaAttribute : JsonAttribute
    {

    }
}
