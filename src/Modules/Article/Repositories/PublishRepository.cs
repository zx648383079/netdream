using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Article.Entities;
using NetDream.Modules.Article.Forms;
using NetDream.Modules.Article.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using NetDream.Shared.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Article.Repositories
{
    public class PublishRepository(ArticleContext db, 
        IClientContext client,
        ICategoryRepository categoryStore,
        ILocalizeRepository localize,
        ITagRepository tagStore,
        IMetaRepository metaStore)
    {

        public IPage<ArticleListItem> GetList(BlogQueryForm form)
        {
            var items = db.Articles.Search(form.Keywords, "title")
                .When(form.Category > 0, i => i.CatId == form.Category)
                .When(form.Type > 0, i => i.Type == form.Type - 1)
                .When(form.Status > 0, i => i.PublishStatus == form.Status - 1, 
                i => i.PublishStatus == (byte)PublishStatus.AutoSave)
                .When(form.Language, i => i.Language == form.Language)
                .OrderByDescending(i => i.Id)
                .ToPage(form, query => query.SelectAs());
            categoryStore.Include(ModuleTargetType.Article, items.Items);
            foreach (var item in items.Items)
            {
                var isLocal = item.ParentId > 0;
                if (!isLocal)
                {
                    isLocal = db.Articles.Where(i => i.ParentId == item.Id).Any();
                }
                item.IsLocalization = isLocal;
            }
            return items;
        }


        public IOperationResult<ArticleModel> Get(int id = 0, string language = "")
        {
            var res = GetBlog(id, language);
            if (!res.Succeeded)
            {
                return (OperationResult<ArticleModel>)res;
            }
            var model = res.Result.CopyTo<ArticleModel>();
            //tags = model.IsNewRecord ? [] : TagRepository.GetTags(model.Id);
            //data = model.ToArray();
            //data["tags"] = tags;
            //data["open_rule"] = model.OpenRule;
            //data["languages"] = self.LanguageAll(model.ParentId > 0 ? model.ParentId : model.Id);
            //return array_merge(data, BlogMetaModel.GetOrDefault(id));
            return OperationResult.Ok(model);
        }

        public IOperationResult<ArticleEntity> GetOrNew(int id = 0, string language = "")
        {
            ArticleEntity? model;
            if (id > 0)
            {
                model = db.Articles.Where(i => i.Id == id && i.UserId == client.UserId).Single();
                if (model is null)
                {
                    return OperationResult<ArticleEntity>.Fail("blog is not exist");
                }
                if (string.IsNullOrWhiteSpace(language) || model.Language == language)
                {
                    return OperationResult.Ok(model);
                }
                return OperationResult.Ok(new ArticleEntity
                {
                    ParentId = model.ParentId > 0 ? model.ParentId : model.Id,
                    Language = language
                });
            }
            model = db.Articles.Where(i => i.UserId == client.UserId && i.PublishStatus == (byte)PublishStatus.AutoSave)
                .OrderBy(i => i.ParentId).Single();
            if (model is null)
            {
                return OperationResult.Ok(new ArticleEntity
                {
                    Language = localize.Default
                });
            }
            if (string.IsNullOrWhiteSpace(language) || model.Language == language)
            {
                return OperationResult.Ok(model);
            }
            return OperationResult.Ok(new ArticleEntity
            {
                ParentId = model.ParentId > 0 ? model.ParentId : model.Id,
                Language = language
            });
        }

        /// <summary>
        /// 获取所有的支持语言列表，显示在后台
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<ILanguageFormatted> LanguageAll(int id)
        {
            var items = db.Articles.Where(i => i.ParentId == id || i.Id == id)
                .Select(i => new KeyValuePair<string, int>(i.Language, i.Id))
                .ToDictionary();
            return localize.Items.Select(i => {
                var item = (LanguageFormatted)i;
                if (items.TryGetValue(item.Value, out var id))
                {
                    item.Id = id;
                }
                return item;
            }).ToArray();
        }

        private IOperationResult<ArticleEntity> GetBlog(int id = 0, 
            string language = "")
        {
            if (id > 0)
            {
                return GetBlogById(id, language);
            }
            ArticleEntity? model;
            if (string.IsNullOrWhiteSpace(language))
            {
                
                model = db.Articles.Where(i => i.UserId == client.UserId && i.PublishStatus == (byte)PublishStatus.Draft)
                    .OrderBy(i => i.ParentId).Single();
            }
            else
            {
                model = db.Articles.Where(i => i.UserId == client.UserId && i.PublishStatus == (byte)PublishStatus.Draft && i.Language == language).Single();
            }
            if (model is null)
            {
                return OperationResult<ArticleEntity>.Fail("blog is not exist");
            }
            return OperationResult.Ok(model);
        }

        private IOperationResult<ArticleEntity> GetBlogById(int id, string language = "")
        {
            var model = db.Articles.Where(i => i.Id == id && i.UserId == client.UserId).Single();
            if (model is null)
            {
                return OperationResult<ArticleEntity>.Fail("blog is not exist");
            }
            if (string.IsNullOrWhiteSpace(language) || 
                language == model.Language)
            {
                return OperationResult.Ok(model);
            }
            if (model.ParentId > 0)
            {
                model = db.Articles.Where(i => (i.ParentId == model.ParentId || i.Id == model.ParentId) && i.Language == language)
                    .Single();
            }
            else
            {
                model = db.Articles.Where(i => i.ParentId == model.Id && i.Language == language).Single();
            }
            if (model is null)
            {
                throw new Exception("blog is not exist");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<ArticleEntity> Save(ArticleForm data, int id = 0)
        {
            if (id > 0)
            {
                data.Id = id;
            }
            var model = data.Id > 0 ? db.Articles.Where(i => i.Id == data.Id && i.UserId == client.UserId).Single() :
                new ArticleEntity();
            if (model is null)
            {
                return OperationResult<ArticleEntity>.Fail("blog is error");
            }
            model.UserId = client.UserId;
            model.Language = data.Language;
            model.ParentId = data.ParentId;
            model.Title = data.Title;
            model.Description = data.Description;
            model.Keywords = data.Keywords;
            model.Content = data.Content;
            model.OriginalType = data.OriginalType;
            model.CatId = data.CatId;
            model.EditType = data.EditType;
            model.OpenType = data.OpenType;
            model.OpenRule = data.OpenRule;
            model.PublishStatus = data.PublishStatus;
            // 需要同步的字段
            var asyncColumn = new string[] {
                "user_id",
                "cat_id",
                "programming_language",
                "original_type",
                "thumb",
                "open_type",
                "open_rule", };
            if (model.ParentId > 0)
            {
                var parent = db.Articles.Where(i => i.Id == model.ParentId && i.UserId == client.UserId).Single();
                if (string.IsNullOrWhiteSpace(model.Language) || model.Language == "zh")
                {
                    model.Language = "en";
                }
                foreach (var key in asyncColumn)
                {
                    var field = typeof(ArticleEntity).GetField(StrHelper.Studly(key));
                    field?.SetValue(model, field.GetValue(parent));
                }
            }
            db.Articles.Save(model, client.Now);
            if (db.SaveChanges() == 0)
            {
                return OperationResult<ArticleEntity>.Fail("save error");
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
        public IOperationResult<ArticleEntity> SaveDraft(ArticleForm data, int id = 0)
        {
            data.PublishStatus = (byte)PublishStatus.AutoSave;
            return Save(data, id);
        }

        public IOperationResult<ArticleEntity> Update(int id, 
            IDictionary<string, string> data)
        {
            var model = db.Articles.Where(i => i.Id == id && i.UserId == client.UserId).Single();
            if (model is null)
            {
                return OperationResult.Fail<ArticleEntity>("blog is not exist");
            }
            db.Articles.BatchToggle(model, data, "type", "publish_status");
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id)
        {
            var model = db.Articles.Where(i => i.Id == id && i.UserId == client.UserId).Single();
            if (model is null)
            {
                return OperationResult.Fail("blog is not exist");
            }
            db.Articles.Remove(model);
            db.SaveChanges();
            if (model.ParentId < 1)
            {
                db.Articles.Where(i => i.ParentId == id).ExecuteDelete();
            }
            metaStore.Remove(ModuleTargetType.Article, id);
            // event(new BlogUpdate(model.Id, 2, time()));
            return OperationResult.Ok();
        }
    }
}
