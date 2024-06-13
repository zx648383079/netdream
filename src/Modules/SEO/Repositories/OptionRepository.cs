using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Models;
using NPoco;

namespace NetDream.Modules.SEO.Repositories
{
    public class OptionRepository
    {
        private readonly IDatabase _db;

        public OptionRepository(IDatabase db)
        {
            _db = db;
        }

        public GlobeOption LoadOption()
        {
            var data = new GlobeOption();
            var items = _db.Fetch<OptionEntity>();
            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Code))
                {
                    continue;
                }
                var val = FormatOptionValue(item);
                if (val is null)
                {
                    continue;
                }
                data.Add(item.Code, val);
            }
            return data;
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
