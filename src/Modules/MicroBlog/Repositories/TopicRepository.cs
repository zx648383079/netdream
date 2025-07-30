using MediatR;
using Microsoft.EntityFrameworkCore;
using NetDream.Modules.MicroBlog.Entities;
using NetDream.Modules.MicroBlog.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.MicroBlog.Repositories
{
    public class TopicRepository(MicroBlogContext db, 
        IClientContext client)
    {
        internal const string LINK_RULE_KEY = "t";
        public IOperationResult<TopicEntity> Save(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return OperationResult.Fail<TopicEntity>("话题为空");
            }
            if (db.Topics.Where(i => i.Name == name).Any())
            {
                return OperationResult.Fail<TopicEntity>("话题已存在");
            }
            var model = new TopicEntity()
            {
                Name = name,
                UserId = client.UserId,
                CreatedAt = client.Now,
                UpdatedAt = client.Now,
            };
            db.Topics.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }
        public TopicEntity[] Save(string[] nameItems)
        {
            if (nameItems.Length == 0)
            {
                return [];
            }
            var exist = db.Topics.Where(i => nameItems.Contains(i.Name)).ToArray();
            var items = nameItems.Select(i => new TopicEntity() { 
                Name = i, 
                UserId = client.UserId,
                CreatedAt = client.Now,
                UpdatedAt = client.Now,
            }).ToArray();
            foreach (var item in items)
            {
                var it = Array.Find(exist, i => i.Name == item.Name);
                if (it is not null)
                {
                    item.Id = it.Id;
                    continue;
                }
                db.Topics.Add(item);
            }
            db.SaveChanges();
            return items;
        }

        public IPage<TopicListItem> GetList(QueryForm form)
        {
            return db.Topics.Search(form.Keywords, "name")
                .OrderBy(i => i.Id)
                .ToPage(form, i => i.SelectAs());
        }

        public IOperationResult<TopicListItem> Get(int id)
        {
            var model = db.Topics.Where(i => i.Id == id).SelectAs().SingleOrDefault();
            if (model is null)
            {
                return OperationResult<TopicListItem>.Fail("话题不存在");
            }
            model.MicroCount = db.BlogTopics.Where(i => i.TopicId == model.Id).Count();
            return OperationResult.Ok(model);
        }

        public int[] SearchTag(string keywords = "")
        {
            var tagId = db.Topics.When(keywords, i => i.Name.Contains(keywords))
                .Select(i => i.Id).ToArray();
            if (tagId.Length == 0)
            {
                return [];
            }
            return db.BlogTopics.Where(i => tagId.Contains(i.TopicId))
                .Select(i => i.MicroId).Distinct().ToArray();
        }

        public TopicEntity[] GetTags(int target)
        {
            var tagId = db.BlogTopics.Where(i => i.MicroId == target).Select(i => i.TopicId).ToArray();
            if (tagId is null || tagId.Length == 0)
            {
                return [];
            }
            return db.Topics.Where(i => tagId.Contains(i.Id)).ToArray();
        }
        public void BindTag(int target, IIdEntity[] tagItems)
        {
            BindTag(target, tagItems.Select(i => i.Id).Where(i => i > 0).ToArray());
        }

        public void BindTag(int target, int[] tagItems)
        {
            if (tagItems.Length == 0)
            {
                return;
            }
            var (add, _, remove) = ModelHelper.SplitId(tagItems,
                db.BlogTopics.Where(i => i.MicroId == target).Select(i => i.TopicId)
                .ToArray());
            if (remove.Count > 0)
            {
                db.BlogTopics.Where(i => i.MicroId == target && remove.Contains(i.TopicId)).ExecuteDelete();
            }
            if (add.Count > 0)
            {
                db.BlogTopics.AddRange(add.Select(i => new BlogTopicEntity()
                {
                    TopicId = i,
                    MicroId = target,
                }));
                db.SaveChanges();
            }
        }

        public void BindTag(int target, string[] tagItems)
        {
            var items = Save(tagItems);
            BindTag(target, items.Select(i => i.Id).Where(i => i > 0).ToArray());
        }

        public void Remove(int id)
        {
            db.Topics.Where(i => i.Id == id).ExecuteDelete();
            db.BlogTopics.Where(i => i.TopicId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public void Remove(params string[] words)
        {
            if (words.Length == 0)
            {
                return;
            }
            var idItems = db.Topics.Where(i => words.Contains(i.Name)).Pluck(i => i.Id);
            if (idItems.Length == 0)
            {
                return;
            }
            db.Topics.Where(i => idItems.Contains(i.Id)).ExecuteDelete();
            db.BlogTopics.Where(i => idItems.Contains(i.TopicId)).ExecuteDelete();
            db.SaveChanges();
        }
    }
}
