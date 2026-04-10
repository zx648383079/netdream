using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Tag.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Tag.Repositories
{
    public class TagRepository(TagContext db): ITagRepository
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

        public IPage<TagEntity> AdvancedList(ModuleTargetType type, QueryForm form)
        {
            return db.Tags.Search(form.Keywords, "name")
                .OrderBy(i => i.Id).ToPage(form);
        }

        public IOperationResult AdvancedRemove(ModuleTargetType type, params int[] idItems)
        {
            db.Tags.Where(i => idItems.Contains(i.Id)).ExecuteDelete();
            db.TagLinks.Where(i => idItems.Contains(i.TagId)).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult AdvancedRemove(ModuleTargetType type, string[] items)
        {
            return AdvancedRemove(type, db.Tags.Where(i => items.Contains(i.Name)).Pluck(i => i.Id));
        }


        public int[] Search(ModuleTargetType type, string keywords)
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
            return db.TagLinks.Where(i => i.ItemType == (byte)type && tagId.Contains(i.TagId)).Select(i => i.ItemId).Distinct().ToArray();
        }

        public string[] Get(ModuleTargetType type, int target)
        {
            var data = db.TagLinks.Where(i => i.ItemType == (byte)type && i.ItemId == target)
                .Select(i => i.TagId).ToArray();
            if (data.Length == 0)
            {
                return [];
            }
            return db.Tags.Where(i => data.Contains(i.Id)).Select(i => i.Name).ToArray();
        }

        public int[] Get(ModuleTargetType type, string tag)
        {
            var model = db.Tags.Where(i => i.Name == tag).SingleOrDefault();
            if (model is null)
            {
                return [];
            }
            return db.TagLinks.Where(i => i.ItemType == (byte)type && i.TagId == model.Id)
                .Select(i => i.ItemId).Distinct().ToArray();
        }

        public IOperationResult Bind(ModuleTargetType type, int target, IEnumerable<string> tags)
        {
            var tagId = Save(tags.ToArray());
            if (tagId.Length == 0)
            {
                return OperationResult.Fail("保存失败");
            }
            var (add, _, remove) = ModelHelper.SplitId(tagId,
                db.TagLinks.Where(i => i.ItemType == (byte)type && i.ItemId == target)
                .Select(i => i.TagId)
                .ToArray());
            if (remove.Count > 0)
            {
                db.TagLinks.Where(i => i.ItemType == (byte)type && i.ItemId == target && remove.Contains(i.TagId)).ExecuteDelete();
            }
            if (add.Count > 0)
            {
                db.TagLinks.AddRange(add.Select(i => new TagLinkEntity()
                {
                    TagId = i,
                    ItemType = (byte)type,
                    ItemId = target,
                }));
            }
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult Remove(ModuleTargetType type, int target, IEnumerable<string> tags)
        {
            var data = db.Tags.Where(i => tags.Contains(i.Name)).Select(i => i.Id).ToArray();
            if (data.Length > 0)
            {
                db.TagLinks.Where(i => i.ItemType == (byte)type && data.Contains(i.TagId) && i.ItemId == target)
                .ExecuteDelete();
                db.SaveChanges();
            }
            
            return OperationResult.Ok();
        }

        public int[] GetRelation(ModuleTargetType type, int target)
        {
            var data = db.TagLinks.Where(i => i.ItemType == (byte)type && i.ItemId == target)
                .Select(i => i.TagId)
                .ToArray();
            if (data.Length == 0)
            {
                return [];
            }
            return db.TagLinks.Where(i => i.ItemType == (byte)type && data.Contains(i.TagId) && i.ItemId != target)
                .Select(i => i.ItemId)
                .Distinct().ToArray();
        }

        public StatisticsItem[] Get(ModuleTargetType type)
        {
            var data = db.TagLinks.Where(i => i.ItemType == (byte)type)
                .GroupBy(i => i.TagId)
                .Select(i => new KeyValuePair<int, int>(i.Key, i.Count()))
                .ToDictionary();
            if (data.Count == 0)
            {
                return [];
            }
            var items = db.Tags.Where(i => data.Keys.Contains(i.Id)).ToArray();
            if (items.Length == 0)
            {
                return [];
            }
            return items.Select(i => new StatisticsItem(i.Name, data[i.Id])).ToArray();
        }

        public IOperationResult Remove(ModuleTargetType type, int target)
        {
            db.TagLinks.Where(i => i.ItemType == (byte)type && i.ItemId == target)
                .ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }
    }
}
