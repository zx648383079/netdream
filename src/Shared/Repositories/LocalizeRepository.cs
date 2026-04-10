using NetDream.Shared.Interfaces;
using NetDream.Shared.Repositories.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetDream.Shared.Repositories
{
    public partial class LocalizeRepository(IClientContext environment) : ILocalizeRepository
    {
        readonly Dictionary<string, string> LANGUAGE_MAP = new()
        {
            {"zh", "中"},
            {"en", "EN"},
        };

        public string Default => LANGUAGE_MAP.Keys.First();

        public string Language => environment.Language;

        public string[] Keys => [.. LANGUAGE_MAP.Keys];

        public IOptionItem<string>[] Items => LANGUAGE_MAP.Select(i => new LanguageFormatted(i.Value, i.Key)).ToArray();

        public string Translate(string message)
        {
            return message;
        }

        public string Translate(string message, IDictionary<string, object> parameteres)
        {
            return PlacementRegex().Replace(message, match => {
                if (parameteres.TryGetValue(match.Groups[1].Value, out var val))
                {
                    return val.ToString();
                }
                return match.Value;
            });
        }

        public string Translate(string message, object[] parameteres)
        {
            return string.Format(message, parameteres);
        }

        [GeneratedRegex(@"\{(a-zA-Z0-9_)+\}")]
        private static partial Regex PlacementRegex();
    }
}
