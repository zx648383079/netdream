using NetDream.Modules.Bot.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Bot.Models
{
    public class ReplyMatchItem(int id, string[] wordItems, bool isStrict)
    {
        public int Id => id;

        public bool IsMatch(string text)
        {
            if (!isStrict)
            {
                return wordItems.Any(text.Contains);
            }
            return wordItems.Any(text.Equals);
        }
    }

    public class ReplyMatchCollection : List<ReplyMatchItem>
    {
        public void Add(ReplyEntity entity)
        {
            Add(new ReplyMatchItem(entity.Id, entity.Keywords.Split(",")
                .Where(i => !string.IsNullOrWhiteSpace(i)).Distinct().ToArray(), entity.Match > 0));
        }

        public void Add(IEnumerable<ReplyEntity> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public bool TryGet(string text, out int result)
        {
            foreach (var item in this)
            {
                if (item.IsMatch(text))
                {
                    result = item.Id;
                    return true;
                }
            }
            result = 0;
            return false;
        }
    }
}
