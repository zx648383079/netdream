using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Helpers
{
    public static class TreeHelper
    {

        public static void Add(this ITreeGroup group, ITreeGroupItem item)
        {
            group.TryAdd(item.Id, item);
        }

        public static void Add(this ITreeGroupItem group, ITreeGroupItem item)
        {
            group.Children ??= new TreeGroup();
            group.Children.TryAdd(item.Id, item);
        }
        public static void Add(this ITreeItem group, ITreeItem item)
        {
            group.Children ??= [];
            group.Children.Add(item);
        }

        public static IList<ITreeItem> Create(IList<ITreeItem> items)
        {
            return Create(items, 0, 0);
        }

        private static IList<ITreeItem> Create(IList<ITreeItem> items, 
            int parentId, int level)
        {
            var data = items.Where(i => i.ParentId == parentId && i.Id != parentId).ToArray();
            foreach (var item in data)
            {
                item.Level = level;
                item.Children = Create(items, item.Id, level + 1);
            }
            return data;
        }

        public static ITreeGroup Create(IList<ITreeGroupItem> items)
        {
            return Create(items, 0, 0);
        }

        private static ITreeGroup Create(IList<ITreeGroupItem> items,
           int parentId, int level)
        {
            var data = new TreeGroup();
            foreach (var item in items)
            {
                if (item.ParentId != parentId || item.Id == parentId)
                {
                    continue;
                }
                item.Level = level;
                item.Children = Create(items, item.Id, level + 1);
                data.Add(item);
            }
            return data;
        }
    }
}
