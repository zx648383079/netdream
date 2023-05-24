using NetDream.Modules.SEO.Entities;
using NPoco;

namespace NetDream.Modules.SEO.Repositories
{
    public class OptionRepository
    {
        private readonly Dictionary<string, object> data = new();

        private readonly bool booted = false;

        public OptionRepository(IDatabase db)
        {
            if (booted)
            {
                return;
            }
            booted = true;
            data = new Dictionary<string, object>();
            var items = db.Fetch<OptionEntity>();
            foreach (var item in items)
            {
                var val = FormatOptionValue(item);
                if (val is null)
                {
                    continue;
                }
                data.Add(item.Code, val);
            }
        }

        public T? Get<T>(string key)
        {
            return data.ContainsKey(key) ? (T)data[key] : default;
        }

        public static object? FormatOptionValue(OptionEntity option)
        {
            return option.Type switch
            {
                "switch" => option.Value == "1" || option.Value == "true",
                "json" => string.IsNullOrWhiteSpace(option.Value) ? null : "",
                _ => option.Value,
            };
        }
    }
}
