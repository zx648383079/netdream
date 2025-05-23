using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using System;
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

        public static ITreeItem[] Create(IList<ITreeItem> items)
        {
            return Create(items, 0, 0);
        }

        private static ITreeItem[] Create(IList<ITreeItem> items, 
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

        public static T[] Sort<T>(IList<T> items)
            where T : ILevelItem
        {
            var res = new List<T>(items.Count);
            Create(res, items, 0, 0);
            return [.. res];
        }

        private static void Create<T>(List<T> res, 
            IList<T> items, int parentId, int level) where T : ILevelItem
        {
            foreach (var item in items)
            {
                if (item.ParentId == parentId)
                {
                    item.Level = level;
                    res.Add(item);
                    Create(res, items, item.Id, level + 1);
                }
            }
        }

        /// <summary>
        /// 获取后代id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="selfId"></param>
        /// <param name="includeSelf"></param>
        /// <returns></returns>
        public static int[] GetChildren<T>(IList<T> items, int selfId, bool includeSelf = false) where T : ITreeLinkItem
        {
            var res = new List<int>();
            if (includeSelf)
            {
                res.Add(selfId);
            }
            var matches = new int[] { selfId };
            var next = new List<int>();
            while (matches.Length > 0)
            {
                foreach (var item in items)
                {
                    if (matches.Contains(item.ParentId))
                    {
                        next.Add(item.Id);
                    }
                }
                res.AddRange(next);
                matches = [.. next];
                next.Clear();
            }
            return [..res];
        }
    }
}
