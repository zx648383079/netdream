using NetDream.Shared.Helpers;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Extensions
{
    public static class Relation
    {

        public static void Bind<K, LinkT>(IEnumerable<K> items,
            string kFieldKey, IEnumerable<LinkT> data, Func<K, LinkT, bool> check)
            where LinkT : class
            where K : class
        {
            if (!data.Any())
            {
                return;
            }
            var field = typeof(K).GetField(kFieldKey);
            if (field is null)
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var u in data)
                {
                    if (check.Invoke(item, u))
                    {
                        field.SetValue(item, u);
                        break;
                    }
                }
            }
        }

        private static bool IsNotEmpty(object? val)
        {
            if (val is null)
            {
                return false;
            }
            if (val is string s)
            {
                return string.IsNullOrWhiteSpace(s);
            }
            if (val is int o)
            {
                return o > 0;
            }
            return true;
        }
    }
}
