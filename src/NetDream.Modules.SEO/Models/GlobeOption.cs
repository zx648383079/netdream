using NetDream.Core.Interfaces;

namespace NetDream.Modules.SEO.Models
{
    public class GlobeOption : IGlobeOption
    {
        private readonly Dictionary<string, object> data = new();

        public T? Get<T>(string key)
        {
            if (data.TryGetValue(key, out var val))
            {
                return (T?)val;
            }
            return default;
        }

        public void Add(string key, object value)
        {
            data.TryAdd(key, value);
        }
    }
}
