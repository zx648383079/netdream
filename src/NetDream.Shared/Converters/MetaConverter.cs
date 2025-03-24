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
                    ReadObject(ref reader, res, propertyName, options);
                }
            }
            return (T)res;
        }

        private static void ReadObject(ref Utf8JsonReader reader, object instance, string propertyName, JsonSerializerOptions options)
        {
            var type = instance.GetType();
            var field = type.GetProperty(propertyName);
            if (field is not null && field?.GetCustomAttribute<JsonMetaAttribute>() is null)
            {
                field?.SetValue(instance, JsonSerializer.Deserialize(ref reader, field.PropertyType, options));
                return;
            }
            foreach (var item in type.GetProperties())
            {
                if (item.GetCustomAttribute<JsonMetaAttribute>() is null)
                {
                    continue;
                }
                var meta = item.GetValue(instance);
                if (meta is null)
                {
                    meta = Activator.CreateInstance(item.PropertyType);
                    item.SetValue(instance, meta);
                }
                if (meta is null)
                {
                    continue;
                }
                ReadObject(ref reader, meta, propertyName, options);
                return;
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            WriteObject(writer, value, options);
            writer.WriteEndObject();
        }

        private static void WriteObject(Utf8JsonWriter writer, object instance, JsonSerializerOptions options)
        {
            var type = instance.GetType();
            var enumeratorFn = type.GetMethod("GetEnumerator");
            if (enumeratorFn is not null)
            {
                var enumerator = enumeratorFn.Invoke(instance, null);
                var enumeratorType = enumerator?.GetType();
                var nextFn = enumeratorType?.GetMethod("MoveNext");
                var currentProperty = enumeratorType?.GetProperty("Current");
                if (currentProperty is not null && nextFn is not null)
                {
                    while ((bool)nextFn.Invoke(enumerator, null))
                    {
                        var current = currentProperty.GetValue(enumerator);
                        if (current is null)
                        {
                            continue;
                        }
                        var keyValuePairType = current.GetType();
                        var keyProperty = keyValuePairType.GetProperty("Key");
                        var valueProperty = keyValuePairType.GetProperty("Value");
                        var val = valueProperty?.GetValue(current);
                        if (val is null)
                        {
                            continue;
                        }
                        writer.WritePropertyName((string)keyProperty.GetValue(current));
                        JsonSerializer.Serialize(writer, val, options);
                    }
                }
            }
            object? meta;
            foreach (var item in type.GetProperties())
            {
                if (item.GetCustomAttribute<JsonMetaAttribute>() is not null)
                {
                    meta = item.GetValue(instance);
                    if (meta is null)
                    {
                        continue;
                    }
                    WriteObject(writer, meta, options);
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
                var key = column is null || string.IsNullOrEmpty(column.Name) ? item.Name : column.Name;
                writer.WritePropertyName(
                    options.PropertyNamingPolicy?.ConvertName(key) ?? key
                    );
                JsonSerializer.Serialize(writer, val, options);
            }
        }
    }
    /// <summary>
    /// 实现附加合并
    /// </summary>
    public class JsonMetaAttribute : JsonAttribute
    {

    }
}
