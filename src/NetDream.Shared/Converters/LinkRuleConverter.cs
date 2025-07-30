using NetDream.Shared.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetDream.Shared.Converters
{
    public class LinkRuleConverter : JsonConverter<LinkExtraRule>
    {
        public override LinkExtraRule? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                return default;
            }
            var res = new LinkExtraRule();
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
                    res.Add(propertyName, reader.GetString()); 
                }
            }
            return res;
        }

        public override void Write(Utf8JsonWriter writer, LinkExtraRule value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("s");
            writer.WriteStringValue(value.Word);
            if (value.User > 0)
            {
                writer.WritePropertyName("u");
                writer.WriteNumberValue(value.User);
            }
            if (!string.IsNullOrWhiteSpace(value.ExtraKey))
            {
                writer.WritePropertyName(value.ExtraKey);
                writer.WriteStringValue(value.ExtraValue);
            }
            writer.WriteEndObject();
        }
    }
}
