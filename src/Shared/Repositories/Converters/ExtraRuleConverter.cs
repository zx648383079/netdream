using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetDream.Shared.Models;
using System.Text.Json;

namespace NetDream.Shared.Repositories.Converters
{
    public sealed class ExtraRuleConverter : ValueConverter<LinkExtraRule[]?, string>
    {
        private static string ArrayToStr(LinkExtraRule[]? items)
        {
            if (items is null || items.Length == 0)
            {
                return string.Empty;
            }
            return JsonSerializer.Serialize(items);
        }
        private static LinkExtraRule[]? StrToArray(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return [];
            }
            return JsonSerializer.Deserialize<LinkExtraRule[]>(value);
        }

        public ExtraRuleConverter()
            : base(
                  i => ArrayToStr(i),
                  s => StrToArray(s)
                 )
        {
            
        }
    }
}
