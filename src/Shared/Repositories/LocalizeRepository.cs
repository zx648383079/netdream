using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Database;
using NetDream.Shared.Repositories.Models;
using NPoco;

namespace NetDream.Shared.Repositories
{
    public class LocalizeRepository(IClientEnvironment environment)
    {
        public const string LANGUAGE_COLUMN_KEY = "language";
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
                if (lang.Contains(item.Key, StringComparison.OrdinalIgnoreCase))
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

        /// <summary>
        /// 根据标识获取自适应的语言版本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T? GetByKey<T>(IDatabase db, Sql query, string key, string value)
        {
            var lang = BrowserLanguage();
            var firstLang = FirstLanguage();
            if (lang == firstLang)
            {
                return db.Single<T>(query.Where($"{LANGUAGE_COLUMN_KEY}=@0 AND {key}=@1", lang, value).OrderBy("id DESC"));
            }
            var langItems = new List<string> { lang, firstLang };
            if (!LANGUAGE_MAP.ContainsKey(BROWSER_DEFAULT_LANGUAGE) 
                && !langItems.Contains(BROWSER_DEFAULT_LANGUAGE)) {
                langItems.Add(BROWSER_DEFAULT_LANGUAGE);
            }
            return db.Single<T>(query.WhereIn(LANGUAGE_COLUMN_KEY, [..langItems])
                .Where($"{key}=@1", value).OrderBy("id DESC"));
        }

        /// <summary>
        /// 一篇文章可以切换的获取语言切换标识
        /// </summary>
        /// <param name="items"></param>
        /// <param name="justExist"></param>
        /// <returns></returns>
        public IList<ILanguageFormatted> FormatLanguageList<T>(IEnumerable<T> items, bool justExist = true)
            where T : ILanguageEntity
        {
            var data = new List<ILanguageFormatted>();
            foreach (var item in LANGUAGE_MAP) {
                foreach(var it in items)
                {
                    if (it.Language == item.Key)
                    {
                        data.Add(new LanguageFormatted(item.Value, item.Key, it.Id));
                    }
                }
                if (justExist)
                {
                    continue;
                }
                data.Add(new LanguageFormatted(item.Value, item.Key));
            }
            return data;
        }
    }
}
