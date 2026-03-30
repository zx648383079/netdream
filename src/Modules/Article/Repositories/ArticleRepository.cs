using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using System;
using System.Linq;

namespace NetDream.Modules.Article.Repositories
{
    public class ArticleRepository(ArticleContext db) : IArticleRepository
    {
        public const byte PUBLISH_STATUS_DRAFT = 0; // 草稿
        public const byte PUBLISH_STATUS_POSTED = 5; // 已发布
        public const byte PUBLISH_STATUS_TRASH = 9; // 垃圾箱
        public const byte PUBLISH_STATUS_AUTO_SAVE = 8; // 自动保存

        public const byte TYPE_ORIGINAL = 0; // 原创
        public const byte TYPE_REPRINT = 1; // 转载

        public const byte OPEN_PUBLIC = 0; // 公开
        public const byte OPEN_LOGIN = 1; // 需要登录
        public const byte OPEN_PASSWORD = 5; // 需要密码
        public const byte OPEN_BUY = 6; // 需要购买

        public IPage<IListArticleItem> Search(ModuleTargetType type, IQueryForm form)
        {
            throw new NotImplementedException();
        }


        public IListArticleItem[] Get(ModuleTargetType type)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<IArticle> Get(ModuleTargetType type, int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int user, ModuleTargetType type, int id)
        {
            db.Articles.Where(i => i.Id == id && i.Type == (byte)type && i.UserId == user).ExecuteDelete();
            db.SaveChanges();
        }

        public IOperationResult Add(int user, ModuleTargetType type, IArticle item)
        {
            throw new NotImplementedException();
        }

        public IOperationResult Update(int user, ModuleTargetType type, IArticle item)
        {
            throw new NotImplementedException();
        }

        private static void CheckSortOrder(QueryForm form)
        {
            switch (form.Sort)
            {
                case "new":
                    form.Sort = "created_at";
                    form.Order = "desc";
                    break;
                case "recommend":
                case "best":
                    form.Sort = "recommend_count'";
                    form.Order = "desc";
                    break;
                case "hot":
                    form.Sort = "comment_count";
                    form.Order = "desc";
                    break;
                default:
                    SearchHelper.CheckSortOrder(form, ["id", "created_at"]);
                    break;
            }
        }
    }
}
