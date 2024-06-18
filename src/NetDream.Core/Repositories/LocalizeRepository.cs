using NetDream.Core.Helpers;
using NetDream.Core.Interfaces;
using NetDream.Core.Interfaces.Database;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Core.Repositories
{
    public class LocalizeRepository(IClientEnvironment environment)
    {
        const string LANGUAGE_COLUMN_KEY = "language";
        const string BROWSER_DEFAULT_LANGUAGE = "en";
        public readonly Dictionary<string, string> LANGUAGE_MAP = new()
        {
            {"zh", "中"},
            {"en", "EN"},
        };

        private string _firstLang = "";
        private string _browserLang = "";

        public string BrowserLanguage()
        {
            if (!string.IsNullOrEmpty(_browserLang))
            {
                return _browserLang; 
            }
            var lang = environment.Language;
            var hasEn = false;
            var enLang = BROWSER_DEFAULT_LANGUAGE;
            var firstLang = string.Empty;
            foreach (var item in LANGUAGE_MAP) {
                if (lang.Contains(item.Key, System.StringComparison.OrdinalIgnoreCase))
                {
                    return _browserLang = item.Key;
                }
                if (string.IsNullOrEmpty(firstLang))
                {
                    firstLang = item.Key;
                }
                if (item.Key == enLang)
                {
                    hasEn = true;
                }
            }
            return _browserLang = hasEn ? enLang : firstLang;
        }

        public string FirstLanguage()
        {
            if (!string.IsNullOrEmpty(_firstLang))
            {
                return _firstLang;
            }
            foreach (var item in LANGUAGE_MAP)
            {
                return _firstLang = item.Key;
            }
            return string.Empty;
        }
        /// <summary>
        /// 把语言作为前缀
        /// </summary>
        /// <returns></returns>
        public string[] LanguageAsColumnPrefix()
        {
            return LANGUAGE_MAP.Select((i, j) => j < 1 ? string.Empty : $"{i.Key}_").ToArray();
        }

        public string FormatKeyWidthPrefix(string key)
        {
            var lang = BrowserLanguage();
            if (FirstLanguage() == lang)
            {
                return key;
            }
            return $"{lang}_{key}";
        }

        public string FormatValueWidthPrefix(object data, string key)
        {
            var formatKey = FormatKeyWidthPrefix(key);
            if (data is Dictionary<string, object> o)
            {
                if (o.TryGetValue(formatKey, out var value))
                {
                    return (string)value;
                }
                return string.Empty;
            }
            var info = data.GetType().GetProperty(StrHelper.Studly(formatKey));
            if (info is null)
            {
                return string.Empty;
            }
            return info.GetValue(data)?.ToString() ?? string.Empty;
        }

        public (string, string) LanguageColumnsWidthPrefix(string key)
        {
            var formatKey = FormatKeyWidthPrefix(key);
            return (key, formatKey);
        }

        public bool BrowserLanguageIsDefault => BrowserLanguage() == BROWSER_DEFAULT_LANGUAGE;

        public void AddTableColumn(ITable table)
        {
            table.Enum(LANGUAGE_COLUMN_KEY, LANGUAGE_MAP.Keys)
                .Default(FirstLanguage())
                .Comment("多语言配置");
        }
    }
}
