using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Providers
{
    public class TagProvider(ITagContext db)
    {

        public void Remove(int id) {
            db.Tags.Where(i => i.Id == id).ExecuteDelete();
            db.TagLinks.Where(i => i.TagId == id).ExecuteDelete();
        }

        /// <summary>
        /// 保存标签并获取标签的id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        public int[] Save(string[] name, IDictionary<string, object> append) {
            //var items = [];
            //idItems = [];
            //foreach ((array)name as item) {
            //    if (is_array(item)) {
            //        if (isset(item["id"]) && item["id"] > 0) {
            //            idItems[] = item["id"];
            //            continue;
            //        }
            //        item = item["name"] ?? null;
            //    }
            //    if (empty(item)) {
            //        continue;
            //    }
            //    if (in_array(name, items)) {
            //        continue;
            //    }
            //    items[] = item;
            //}
            //exist = TagQuery().WhereIn("name", items)
            //    .Pluck("id", "name");
            //foreach (items as name) {
            //    if (isset(exist[name])) {
            //        idItems[] = exist[name];
            //        continue;
            //    }
            //    id = TagQuery().Insert(array_merge(
            //        append,
            //        [
            //            "name" => name,
            //        ]
            //    ));
            //    if (id > 0) {
            //        idItems[] = id;
            //    }
            //}
            //return idItems;
            return [];
        }

        public IPage<TagEntity> GetList(string keywords = "", long page = 1, int perPage = 20) {
            IQueryable<TagEntity> query = db.Tags;
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(i => i.Name.Contains(keywords));
            }
            var res = new Page<TagEntity>(query.Count(), (int)page, perPage);
            if (!res.IsEmpty)
            {
                res.Items = query.Skip(res.ItemsOffset).Take(res.ItemsPerPage).ToArray();
            }
            return res;
        }

        public string[] Suggest(string keywords = "", int count = 5) {
            IQueryable<TagEntity> query = db.Tags;
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(i => i.Name.Contains(keywords));
            }
            return query.Take(count).Select(i => i.Name).ToArray();
        }

        public TagEntity[] All() {
            return db.Tags.ToArray();
        }

        public TagEntity[] GetTags(int target) {
            var tagId = db.TagLinks.Where(i => i.TargetId == target).Select(i => i.TagId).ToArray();
            if (tagId is null || tagId.Length == 0)
            {
                return [];
            }
            return db.Tags.Where(i => tagId.Contains(i.Id)).ToArray();
        }

        public IDictionary<int, IList<TagEntity>> GetManyTags(int[] target) {
            var data = db.TagLinks.Where(i => target.Contains(i.TargetId)).ToArray();
            var res = new Dictionary<int, IList<TagEntity>>();
            if (data is null || data.Length == 0) {
                return res;
            }
            var tagIdItems = data.Select(i => i.TagId).Distinct();
            var tags = db.Tags.Where(i => tagIdItems.Contains(i.Id)).ToArray();
            foreach (var id in target) {
                var tagId = new List<int>();
                foreach (var item in data)
                {
                    if (item.TargetId == id)
                    {
                        tagId.Add(item.TagId);
                    }
                }
                if (tagId.Count == 0)
                {
                    continue;
                }
                var items = tags.Where(i => tagId.Contains(i.Id));
                if (items.Any())
                {
                    res.Add(id, items.ToArray());
                }
            }
            return res;
        }

        /// <summary>
        /// 根据关键词搜索标签表，并根据中间表找到链接的id集合
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public IList<int> SearchTag(string keywords = "") {
            IQueryable<TagEntity> query = db.Tags;
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(i => i.Name.Contains(keywords));
            }
            var tagId = query.Select(i => i.Id).ToArray();
            if (tagId.Length == 0)
            {
                return [];
            }
            return db.TagLinks.Where(i => tagId.Contains(i.TagId)).Select(i => i.TargetId).Distinct().ToArray();
        }

        /// <summary>
        /// 关联标签
        /// </summary>
        /// <param name="target">其他表的主键值</param>
        /// <param name="tags"></param>
        /// <param name="append"></param>
        /// <param name="afterBind"></param>
        public void BindTag(int target, string[] tags, IDictionary<string, object> append, Action<int[], int[], int[]>? afterBind = null) {
            var tagId = Save(tags, append);
            if (tagId.Length == 0) {
                return;
            }
            var (add, _, remove) = ModelHelper.SplitId(tagId,
                db.TagLinks.Where(i => i.TargetId == target).Select(i => i.TagId)
                .ToArray());
            if (remove.Count > 0)
            {
                db.TagLinks.Where(i => i.TargetId == target && remove.Contains(i.TagId)).ExecuteDelete();
            }
            if (add.Count > 0)
            {
                db.TagLinks.AddRange(add.Select(i => new TagLinkEntity()
                {
                    TagId = i,
                    TargetId = target,
                }));
                db.SaveChanges();
            }
            afterBind?.Invoke(tagId, [.. add], [.. remove]);
        }


        public void RemoveLink(int target) 
        {
            db.TagLinks.Where(i => i.TargetId == target).ExecuteDelete();
        }
    }
}
