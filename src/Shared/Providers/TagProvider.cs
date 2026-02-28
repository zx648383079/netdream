using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Providers
{
    public class TagProvider(ITagContext db)
    {

        public void Remove(int id) 
        {
            db.Tags.Where(i => i.Id == id).ExecuteDelete();
            db.TagLinks.Where(i => i.TagId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IOperationResult<TagEntity> Save(TagForm data)
        {
            if (db.Tags.Where(i => i.Id != data.Id && i.Name == data.Name).Any())
            {
                return OperationResult.Fail<TagEntity>("已存在");
            }
            var model = data.Id > 0 ? db.Tags.Where(i => i.Id == data.Id).SingleOrDefault() : new TagEntity();
            if (model == null)
            {
                return OperationResult.Fail<TagEntity>("id is error");
            }
            model.Name = data.Name;
            db.Tags.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        /// <summary>
        /// 保存标签并获取标签的id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int[] Save(string[] nameItems) 
        {
            if (nameItems.Length == 0)
            {
                return [];
            }
            var exist = db.Tags.Where(i => nameItems.Contains(i.Name))
                .Select(i => new KeyValuePair<string, int>(i.Name, i.Id)).ToDictionary();
            var res = new int[nameItems.Length];
            for (int i = 0; i < nameItems.Length; i++)
            {
                var name = nameItems[i];
                if (exist.TryGetValue(name, out var id))
                {
                    res[i] = id;
                    continue;
                }
                // 可以提前优化，批量插入
                var model = new TagEntity()
                {
                    Name = name
                };
                db.Tags.Add(model);
                db.SaveChanges();
                res[i] = model.Id;
            }
            return res;
        }

        public IPage<TagEntity> GetList(QueryForm form) {
            return db.Tags.Search(form.Keywords, "name")
                .OrderBy(i => i.Id)
                .ToPage(form);
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

        /// <summary>
        /// 根据关联的id 找出所有其他管理的id
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public int[] GetRelationList(int sourceId)
        {
            var tagId = db.TagLinks.Where(i => i.TargetId == sourceId)
                .Select(i => i.TagId).ToArray();
            if (tagId.Length == 0)
            {
                return [];
            }
            return db.TagLinks.Where(i => tagId.Contains(i.TagId) && i.TargetId != sourceId).Select(i => i.TargetId).ToArray();
        }
        /// <summary>
        /// 根据标签id 找出所有关联的id
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public int[] GetTagRelationList(int tagId)
        {
            return db.TagLinks.Where(i => i.TagId == tagId).Select(i => i.TargetId).ToArray();
        }
        /// <summary>
        /// 根据标签获取关联的id
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public int[] GetTagRelationList(string tag)
        {
            var tagId = db.Tags.Where(i => i.Name == tag).Select(i => i.Id).FirstOrDefault();
            if (tagId <= 0)
            {
                return [];
            }
            return db.TagLinks.Where(i => i.TagId == tagId).Select(i => i.TargetId).ToArray();
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
        public void BindTag(int target, string[] tags, 
            IDictionary<string, object>? append, Action<int[], int[], int[]>? afterBind = null) {
            var tagId = Save(tags);
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

        public void BindTag(int target, string[] tags)
        {
            var tagId = Save(tags);
            if (tagId.Length == 0)
            {
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
        }


        public void RemoveLink(int target) 
        {
            db.TagLinks.Where(i => i.TargetId == target).ExecuteDelete();
        }

        
    }
}
