using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Forms;
using NetDream.Modules.Blog.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Repositories;
using NetDream.Shared.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Blog.Repositories
{
    public class PublishRepository(BlogContext db, 
        IClientContext client,
        LocalizeRepository localize,
        MetaRepository meta)
    {
        public const byte PUBLISH_STATUS_DRAFT = 0; // 草稿
        public const byte PUBLISH_STATUS_POSTED = 5; // 已发布
        public const byte PUBLISH_STATUS_TRASH = 9; // 垃圾箱
        public const byte PUBLISH_STATUS_AUTO_SAVE = 8; // 自动保存

        public const byte TYPE_ORIGINAL = 0; // 原创
        public const byte TYPE_REPRINT = 1; // 转载

        public const byte EDIT_HTML = 0;
        public const byte EDIT_MARKDOWN = 1; // markdown

        public const byte OPEN_PUBLIC = 0; // 公开
        public const byte OPEN_LOGIN = 1; // 需要登录
        public const byte OPEN_PASSWORD = 5; // 需要密码
        public const byte OPEN_BUY = 6; // 需要购买

        public IPage<BlogListItem> GetList(string keywords = "", 
            int category = 0, int status = 0, 
            int type = 0, string language = "", 
            int page = 1)
        {
            var items = db.Blogs.Search(keywords, "title")
                .When(category > 0, i => i.TermId == category)
                .When(type > 0, i => i.Type == type - 1)
                .When(status > 0, i => i.PublishStatus == status - 1, i => i.PublishStatus == PUBLISH_STATUS_AUTO_SAVE)
                .When(language, i => i.Language == language)
                .OrderByDescending(i => i.Id)
                .ToPage(page, query => query.Select<BlogEntity, BlogListItem>());
            CategoryRepository.WithCategory(db, items.Items);
            foreach (var item in items.Items)
            {
                var isLocal = item.ParentId > 0;
                if (!isLocal)
                {
                    isLocal = db.Blogs.Where(i => i.ParentId == item.Id).Any();
                }
                item.IsLocalization = isLocal;
            }
            return items;
        }


        public IOperationResult<BlogModel> Get(int id = 0, string language = "")
        {
            var res = GetBlog(id, language);
            if (!res.Succeeded)
            {
                return (OperationResult<BlogModel>)res;
            }
            var model = res.Result.CopyTo<BlogModel>();
            //tags = model.IsNewRecord ? [] : TagRepository.GetTags(model.Id);
            //data = model.ToArray();
            //data["tags"] = tags;
            //data["open_rule"] = model.OpenRule;
            //data["languages"] = self.LanguageAll(model.ParentId > 0 ? model.ParentId : model.Id);
            //return array_merge(data, BlogMetaModel.GetOrDefault(id));
            return OperationResult.Ok(model);
        }

        public IOperationResult<BlogEntity> GetOrNew(int id = 0, string language = "")
        {
            BlogEntity? model;
            if (id > 0)
            {
                model = db.Blogs.Where(i => i.Id == id && i.UserId == client.UserId).Single();
                if (model is null)
                {
                    return OperationResult<BlogEntity>.Fail("blog is not exist");
                }
                if (string.IsNullOrWhiteSpace(language) || model.Language == language)
                {
                    return OperationResult.Ok(model);
                }
                return OperationResult.Ok(new BlogEntity
                {
                    ParentId = model.ParentId > 0 ? model.ParentId : model.Id,
                    Language = language
                });
            }
            model = db.Blogs.Where(i => i.UserId == client.UserId && i.PublishStatus == PUBLISH_STATUS_AUTO_SAVE)
                .OrderBy(i => i.ParentId).Single();
            if (model is null)
            {
                return OperationResult.Ok(new BlogEntity
                {
                    Language = localize.FirstLanguage()
                });
            }
            if (string.IsNullOrWhiteSpace(language) || model.Language == language)
            {
                return OperationResult.Ok(model);
            }
            return OperationResult.Ok(new BlogEntity
            {
                ParentId = model.ParentId > 0 ? model.ParentId : model.Id,
                Language = language
            });
        }

        /**
         * 获取所有的支持语言列表，显示在后台
         * @param int id
         * @return array
         */
        public IList<ILanguageFormatted> LanguageAll(int id)
        {
            var items = db.Blogs.Where(i => i.ParentId == id || i.Id == id).Select(i => new BlogEntity()
            {
                Id = i.Id,
                Language = i.Language,
            }).ToArray();
            return localize.FormatLanguageList(items, false);
        }

        private IOperationResult<BlogEntity> GetBlog(int id = 0, 
            string language = "")
        {
            if (id > 0)
            {
                return GetBlogById(id, language);
            }
            BlogEntity? model;
            if (string.IsNullOrWhiteSpace(language))
            {
                
                model = db.Blogs.Where(i => i.UserId == client.UserId && i.PublishStatus == PUBLISH_STATUS_DRAFT)
                    .OrderBy(i => i.ParentId).Single();
            }
            else
            {
                model = db.Blogs.Where(i => i.UserId == client.UserId && i.PublishStatus == PUBLISH_STATUS_DRAFT && i.Language == language).Single();
            }
            if (model is null)
            {
                return OperationResult<BlogEntity>.Fail("blog is not exist");
            }
            return OperationResult.Ok(model);
        }

        private IOperationResult<BlogEntity> GetBlogById(int id, string language = "")
        {
            var model = db.Blogs.Where(i => i.Id == id && i.UserId == client.UserId).Single();
            if (model is null)
            {
                return OperationResult<BlogEntity>.Fail("blog is not exist");
            }
            if (string.IsNullOrWhiteSpace(language) || 
                language == model.Language)
            {
                return OperationResult.Ok(model);
            }
            if (model.ParentId > 0)
            {
                model = db.Blogs.Where(i => (i.ParentId == model.ParentId || i.Id == model.ParentId) && i.Language == language)
                    .Single();
            }
            else
            {
                model = db.Blogs.Where(i => i.ParentId == model.Id && i.Language == language).Single();
            }
            if (model is null)
            {
                throw new Exception("blog is not exist");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<BlogEntity> Save(BlogForm data, int id = 0)
        {
            if (id > 0)
            {
                data.Id = id;
            }
            var model = data.Id > 0 ? db.Blogs.Where(i => i.Id == data.Id && i.UserId == client.UserId).Single() :
                new BlogEntity();
            if (model is null)
            {
                return OperationResult<BlogEntity>.Fail("blog is error");
            }
            model.UserId = client.UserId;
            model.Language = data.Language;
            model.ParentId = data.ParentId;
            model.Title = data.Title;
            model.Description = data.Description;
            model.Keywords = data.Keywords;
            model.Content = data.Content;
            model.Type = data.Type;
            model.TermId = data.TermId;
            model.EditType = data.EditType;
            model.OpenType = data.OpenType;
            model.OpenRule = data.OpenRule;
            model.PublishStatus = data.PublishStatus;
            // 需要同步的字段
            var asyncColumn = new string[] {
                "user_id",
                "term_id",
                "programming_language",
                "type",
                "thumb",
                "open_type",
                "open_rule", };
            if (model.ParentId > 0)
            {
                var parent = db.Blogs.Where(i => i.Id == model.ParentId && i.UserId == client.UserId).Single();
                if (string.IsNullOrWhiteSpace(model.Language) || model.Language == "zh")
                {
                    model.Language = "en";
                }
                foreach (var key in asyncColumn)
                {
                    var field = typeof(BlogEntity).GetField(StrHelper.Studly(key));
                    field?.SetValue(model, field.GetValue(parent));
                }
            }
            db.Blogs.Save(model, client.Now);
            if (db.SaveChanges() == 0)
            {
                return OperationResult<BlogEntity>.Fail("save error");
            }
            //if (model.ParentId < 1)
            //{
            //    TagRepository.AddTag(model.Id,
            //        isset(data["tags"]) && !empty(data["tags"]) ? data["tags"] : []);
            //    asyncData = [];
            //    foreach (async_column as key)
            //    {
            //        asyncData[key] = model.GetAttributeSource(key);
            //    }
            //    BlogModel.Where("parent_id", model.Id).Update(asyncData);
            //}
            // meta.SaveBatch(model.Id, data);
            // event (new BlogUpdate(model.Id, isNew? 0 : 1, time()));
            return OperationResult.Ok(model);
        }

        /// <summary>
        /// 保存为草稿箱
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        public IOperationResult<BlogEntity> SaveDraft(BlogForm data, int id = 0)
        {
            data.PublishStatus = PUBLISH_STATUS_AUTO_SAVE;
            return Save(data, id);
        }

        public IOperationResult<BlogEntity> Update(int id, 
            IDictionary<string, string> data)
        {
            var model = db.Blogs.Where(i => i.Id == id && i.UserId == client.UserId).Single();
            if (model is null)
            {
                return OperationResult.Fail<BlogEntity>("blog is not exist");
            }
            db.Blogs.BatchToggle(model, data, "type", "publish_status");
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id)
        {
            var model = db.Blogs.Where(i => i.Id == id && i.UserId == client.UserId).Single();
            if (model is null)
            {
                return OperationResult.Fail("blog is not exist");
            }
            db.Blogs.Remove(model);
            db.SaveChanges();
            if (model.ParentId < 1)
            {
                db.Blogs.Where(i => i.ParentId == id).ExecuteDelete();
            }
            meta.DeleteBatch(id);
            // event(new BlogUpdate(model.Id, 2, time()));
            return OperationResult.Ok();
        }
    }
}
