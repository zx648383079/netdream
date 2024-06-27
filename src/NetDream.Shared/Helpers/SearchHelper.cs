using NPoco;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Helpers
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
        public static (string, string) CheckSortOrder(string sort,
            bool order, string[] sortItems, string defaultOrder = "desc")
        {
            return CheckSortOrder(sort, order ? "desc" : "asc", sortItems, defaultOrder);
        }
        public static (string, string) CheckSortOrder(string sort,
            int order, string[] sortItems, string defaultOrder = "desc")
        {
            return CheckSortOrder(sort, order > 0, sortItems, defaultOrder);
        }
        public static (string, string) CheckSortOrder(string sort, 
            string order, string[] sortItems, string defaultOrder = "desc")
        {
            if(!order.Equals("desc", 
                System.StringComparison.OrdinalIgnoreCase) 
                && !order.Equals("asc",
                System.StringComparison.OrdinalIgnoreCase)) {
                order = defaultOrder;
            }
            if (!sortItems.Contains(sort))
            {
                sort = sortItems.First();
            }
            return (sort, order.ToUpper());
        }
    }
}
