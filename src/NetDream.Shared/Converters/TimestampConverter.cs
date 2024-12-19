using NetDream.Shared.Helpers;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace NetDream.Shared.Converters
{
    internal partial class TimestampConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString() ?? string.Empty;
            if (IntRegex().IsMatch(str))
            {
                return Convert.ToInt32(str);
            }
            return TimeHelper.TimestampFrom(str);
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(TimeHelper.TimestampTo(value).ToString("yyyy-MM-dd HH:mm:ss"));
        }

        [GeneratedRegex(@"^\d{10}$")]
        private static partial Regex IntRegex();
    }
}
