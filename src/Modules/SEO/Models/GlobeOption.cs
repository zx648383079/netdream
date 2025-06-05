using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NetDream.Modules.SEO.Models
{
    public class GlobeOption : IGlobeOption
    {
        private readonly Dictionary<string, object> data = [];

        public T? Get<T>(string key)
        {
            if (data.TryGetValue(key, out var val))
            {
                return (T?)val;
            }
            return default;
        }

        public bool TryGet<T>(string key, [NotNullWhen(true)] out T? value)
        {
            if (data.TryGetValue(key, out var val))
            {
                value = (T?)val;
                return value is not null;
            }
            value = default;
            return false;
        }

        public void Add(string key, object value)
        {
            data.TryAdd(key, value);
        }

        public void AddGroup(string groupName, IEnumerable<IOptionConfigureItem> items)
        {
            throw new NotImplementedException();
        }

        public void AddGroup(string groupName, Func<IEnumerable<IOptionConfigureItem>> cb)
        {
            throw new NotImplementedException();
        }

        public void InsertOrUpdate(string key, object data, string name)
        {

        }
    }
}
