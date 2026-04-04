using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Article.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Article.Repositories
{
    public class ArticleRepository(ArticleContext db, 
        ICategoryRepository categoryStore, 
        IUserRepository userStore) : IArticleRepository
    {

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
            return db.Articles.Where(i => i.Type == (byte)type
            && i.PublishStatus == (byte)PublishStatus.Posted && i.Status == (byte)ReviewStatus.Approved)
                .OrderByDescending(i => i.CreatedAt)
                .SelectAsLabel().ToArray();
        }

        public IOperationResult<IArticle> Get(ModuleTargetType type, int id)
        {
            var model = db.Articles.Where(i => i.Type == (byte)type && i.Id == id 
            && i.PublishStatus == (byte)PublishStatus.Posted && i.Status == (byte)ReviewStatus.Approved).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<IArticle>.Fail("id is error");
            }
            return OperationResult.Ok<IArticle>(new ArticleOpenModel(model));
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

        private static (string, string) CheckSortOrder(IQueryForm form)
        {
            return form.Sort switch
            {
                "new" => ("created_at", "desc"),
                "recommend" or "best" => ("recommend_count", "desc"),
                "hot" => ("comment_count", "desc"),
                _ => SearchHelper.CheckSortOrder(form, ["id", "created_at"]),
            };
        }

        public void Include(ModuleTargetType type, IEnumerable<IWithArticleModel> items)
        {
            var idItems = items.Select(item => item.ArticleId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Articles.Where(i => i.Type == (byte)type && idItems.Contains(i.Id)).Select(i => new ListArticleItem(i.Id, i.Title))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.ArticleId > 0 && data.TryGetValue(item.ArticleId, out var article))
                {
                    item.Article = article;
                }
            }
        }
    }
}
