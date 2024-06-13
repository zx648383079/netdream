using NPoco;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Core.Helpers
{
    public static class SearchHelper
    {
        public static IEnumerable<string> Split(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return [];
            }
            return input.Split(' ').Where(i => !string.IsNullOrWhiteSpace(i));
        }

        public static IEnumerable<string> Split(string input, IDictionary<string, string> replaceTag)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return [];
            }
            foreach (var item in replaceTag)
            {
                input = input.Replace(item.Key, item.Value);
            }
            return Split(input);
        }

        public static IEnumerable<string> Split(string input, IEnumerable<string> removeTag)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return [];
            }
            foreach (var item in removeTag)
            {
                input = input.Replace(item, string.Empty);
            }
            return Split(input);
        }

        public static Sql Where(Sql sql, string column, string value)
        {
            var items = Split(value, ["%"]).ToArray();
            if (items.Length == 0) 
            {
                return sql;
            }
            return sql.Where(string.Join(" OR ",
                items.Select((_, j) => column + " LIKE @" + j)), items.Select(i => $"%{i}%"));
        }

        public static Sql Where(Sql sql, IList<string> columns, string value)
        {
            var items = Split(value, ["%"]).ToArray();
            if (items.Length == 0 || columns.Count == 0)
            {
                return sql;
            }
            var partItems = new string[items.Length * columns.Count];
            var data = new string[items.Length * columns.Count];
            for (var i = 0; i < columns.Count; i++)
            {
                for (var j = 0; j < items.Length; j++)
                {
                    var index = i * columns.Count + j;
                    partItems[index] = columns[i] + " LIKE @" + index;
                    data[index] = $"%{items[j]}%";
                }
            }
            return sql.Where(string.Join(" OR ", partItems), data);
        }
    }
}
