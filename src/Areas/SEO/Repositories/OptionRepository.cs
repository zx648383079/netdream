using NetDream.Areas.SEO.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Areas.SEO.Repositories
{
    public class OptionRepository
    {
        private Dictionary<string, object> data;

        private readonly bool booted = false;

        public OptionRepository(IDatabase db)
        {
            if (booted)
            {
                return;
            }
            booted = true;
            data = new Dictionary<string, object>();
            var items = db.Fetch<OptionModel>();
            foreach (var item in items)
            {
                data.Add(item.Code, item.FormatValue());
            }
        }

        public object Get(string key)
        {
            return data.ContainsKey(key) ? data[key] : null;
        }
    }
}
