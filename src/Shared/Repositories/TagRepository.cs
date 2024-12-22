using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Shared.Repositories
{
    public abstract class TagRepository(ITagContext db)
    {
        public int[] Save(string[] nameItems)
        {
            var exist = db.Tags.Where(i => nameItems.Contains(i.Name)).ToArray();
            var items = nameItems.Select(i => new TagEntity() { Name = i }).ToArray();
            foreach (var item in items)
            {
                var it = Array.Find(exist, i => i.Name == item.Name);
                if (it is not null)
                {
                    item.Id = it.Id;
                    continue;
                }
                db.Tags.Add(item);
            }
            db.SaveChanges();
            return items.Select(i => i.Id).ToArray();
        }

        public IPage<TagEntity> GetList(string keywords = "", long page = 1)
        {
            IQueryable<TagEntity> query = db.Tags;
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(i => i.Name.Contains(keywords));
            }
            var res = new Page<TagEntity>(query.Count(), (int)page);
            if (!res.IsEmpty)
            {
                res.Items = query.Skip(res.ItemsOffset).Take(res.ItemsPerPage).ToArray();
            }
            return res;
        }

        public TagEntity[] AllList()
        {
            return db.Tags.ToArray();
        }

        public int[] SearchTag(string keywords = "")
        {
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

        public void BindTag(int target, string[] tagItems)
        {
            var tagId = Save(tagItems);
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
            OnAfterBind(tagId, add, remove);
        }

        protected virtual void OnAfterBind(int[] tagId, IList<int> add, IList<int> remove)
        {

        }
    }
}
