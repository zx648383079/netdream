using Microsoft.AspNetCore.Mvc.ViewEngines;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Forms;
using NetDream.Modules.Blog.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Shared.Repositories;
using NetDream.Shared.Repositories.Models;
using NPoco;
using NPoco.FluentMappings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Blog.Repositories
{
    public class PublishRepository(IDatabase db, 
        IClientEnvironment environment,
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

        public Page<BlogModel> GetList(string keywords = "", 
            int category = 0, int status = 0, 
            int type = 0, string language = "", 
            int page = 1)
        {
            var sql = new Sql();
            sql.Select(["id", "title", "description", "user_id", "type",
                "thumb",
                "language",
                "programming_language",
                "term_id",
                "parent_id",
                "open_type",
                "comment_count",
                "publish_status",
                "click_count", "recommend_count", MigrationTable.COLUMN_CREATED_AT]);
            sql.From<BlogEntity>(db)
                .Where("user_id", environment.UserId);
            SearchHelper.Where(sql, "title", keywords);
            if (category > 0)
            {
                sql.Where("term_id=@0", category);
            }
            if (type > 0)
            {
                sql.Where("type=@0", type - 1);
            }
            if (status > 0)
            {
                sql.Where("publish_status=@0", status - 1);
            } else
            {
                sql.Where("publish_status<>@0", PUBLISH_STATUS_AUTO_SAVE);
            }
            if (string.IsNullOrWhiteSpace(language))
            {
                sql.Where("language=@0", language);
            }
            sql.OrderBy("id DESC");
            var items = db.Page<BlogModel>(page, 20, sql);
            WithCategory(items.Items);
            foreach (var item in items.Items)
            {
                var isLocal = item.ParentId > 0;
                if (!isLocal)
                {
                    isLocal = db.FindCount<BlogEntity>("parent_id=@0", item.Id) > 0;
                }
                item.IsLocalization = isLocal;
            }
            return items;
        }

        private void WithCategory(IEnumerable<BlogModel> items)
        {
            var idItems = items.Select(item => item.TermId);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Fetch<CategoryEntity>($"WHERE id IN({string.Join(',', idItems)})");
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.TermId == it.Id)
                    {
                        item.Term = it;
                        break;
                    }
                }
            }
        }

        public BlogModel Get(int id = 0, string language = "")
        {
            var model = GetBlog(id, language);
            //tags = model.IsNewRecord ? [] : TagRepository.GetTags(model.Id);
            //data = model.ToArray();
            //data["tags"] = tags;
            //data["open_rule"] = model.OpenRule;
            //data["languages"] = self.LanguageAll(model.ParentId > 0 ? model.ParentId : model.Id);
            //return array_merge(data, BlogMetaModel.GetOrDefault(id));
            return model;
        }

        public BlogEntity GetOrNew(int id = 0, string language = "")
        {
            BlogModel? model;
            if (id > 0)
            {
                model = db.Single<BlogModel>("WHERE id=@0 AND user_id=@1", id, environment.UserId);;
                if (model is null)
                {
                    throw new Exception("blog is not exist");
                }
                if (string.IsNullOrWhiteSpace(language) || model.Language == language)
                {
                    return model;
                }
                return new BlogEntity
                {
                    ParentId = model.ParentId > 0 ? model.ParentId : model.Id,
                    Language = language
                };
            }
            model = db.Single<BlogModel>(
                "WHERE user_id=@0 AND publish_status=@1 ORDER BY parent_id ASC",
                environment.UserId,
                PUBLISH_STATUS_AUTO_SAVE
                );
            if (model is null)
            {
                return new BlogEntity
                {
                    Language = localize.FirstLanguage()
                };
            }
            if (string.IsNullOrWhiteSpace(language) || model.Language == language)
            {
                return model;
            }
            return new BlogEntity
            {
                ParentId = model.ParentId > 0 ? model.ParentId : model.Id,
                Language = language
            };
        }

        /**
         * 获取所有的支持语言列表，显示在后台
         * @param int id
         * @return array
         */
        public IList<ILanguageFormatted> LanguageAll(int id)
        {
            var items = db.Fetch<BlogEntity>(
                new Sql().Select("id", "language")
                .From<BlogEntity>(db).Where("parent_id=@0 OR id=@0", id)
                );
            return localize.FormatLanguageList(items, false);
        }

        private BlogModel GetBlog(int id = 0, 
            string language = "")
        {
            if (id > 0)
            {
                return GetBlogById(id, language);
            }
            BlogModel? model;
            if (string.IsNullOrWhiteSpace(language))
            {
                
                model = db.Single<BlogModel>("user_id=@0 AND publish_status=@1 ORDER BY parent_id ASC", 
                    environment.UserId, PUBLISH_STATUS_DRAFT);
            }
            else
            {
                model = db.Single<BlogModel>("user_id=@0 AND publish_status=@1 AND language=@2",
                    environment.UserId, PUBLISH_STATUS_DRAFT, language);
            }
            if (model is null)
            {
                throw new Exception("blog is not exist");
            }
            return model;
        }

        private BlogModel GetBlogById(int id, string language = "")
        {
            var model = db.Single<BlogModel>("id=@0 AND user_id=@1", id, environment.UserId);
            if (model is null)
            {
                throw new Exception("blog is not exist");
            }
            if (string.IsNullOrWhiteSpace(language) || 
                language == model.Language)
            {
                return model;
            }
            if (model.ParentId > 0)
            {
                model = db.Single<BlogModel>("(parent_id=@0 OR id=@0) AND language=@1",
                    model.ParentId, language);
            }
            else
            {
                model = db.Single<BlogModel>("parent_id=@0 AND language=@1", model.Id,
                    language);
            }
            if (model is null)
            {
                throw new Exception("blog is not exist");
            }
            return model;
        }

        public BlogEntity Save(BlogForm data, int id = 0)
        {
            if (id > 0)
            {
                data.Id = id;
            }
            var model = data.Id > 0 ? db.Single<BlogEntity>("id=@0 AND user_id=@1", data.Id, environment.UserId) :
                new BlogEntity();
            model.UserId = environment.UserId;
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
                var parent = db.Single<BlogEntity>("id=@0 AND user_id=@1", 
                    model.ParentId, environment.UserId);
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
            ;
            if (!db.TrySave(model))
            {
                throw new Exception("save error");
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
            return model;
        }

        /// <summary>
        /// 保存为草稿箱
        /// </summary>
        /// <param name="data"></param>
        /// <param name="id"></param>
        public BlogEntity SaveDraft(BlogForm data, int id = 0)
        {
            data.PublishStatus = PUBLISH_STATUS_AUTO_SAVE;
            return Save(data, id);
        }

        public BlogEntity Update(int id, IDictionary<string, string> data)
        {
            var model = db.Single<BlogEntity>("id=@0 AND user_id=@1", id, environment.UserId);
            if (model is null)
            {
                throw new Exception("blog is not exist");
            }
            ModelHelper.BatchToggle(db, model, data, ["type", "publish_status"]);
            return model;
        }

        public void Remove(int id)
        {
            var model = db.Single<BlogEntity>("id=@0 AND user_id=@1", id, environment.UserId);
            if (model is null)
            {
                throw new Exception("blog is not exist");
            }
            db.Delete<BlogEntity>(id);
            if (model.ParentId < 1)
            {
                db.Delete<BlogEntity>("WHERE parent_id=@0", id);
            }
            meta.DeleteBatch(id);
            // event(new BlogUpdate(model.Id, 2, time()));
        }
    }
}
