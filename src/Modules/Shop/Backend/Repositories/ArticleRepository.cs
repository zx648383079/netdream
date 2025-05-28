using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Backend.Forms;
using NetDream.Modules.Shop.Backend.Models;
using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Repositories
{
    public class ArticleRepository(ShopContext db, IClientContext client)
    {
        public IPage<ArticleListItem> GetList(QueryForm form, string category)
        {
            return GetList(form, db.ArticleCategories.Where(i => i.Name == category).Value(i => i.Id));
        }
        public IPage<ArticleListItem> GetList(QueryForm form, int category = 0)
        {
            var res = db.Articles.Search(form.Keywords, "title")
                .When(category > 0, i => i.CatId == category)
                .ToPage(form, query => query.Select(i => new ArticleListItem()
                {
                    Id = i.Id,
                    Title = i.Title,
                    CatId = i.CatId,
                    Description = i.Description,
                    Thumb = i.Thumb,
                    CreatedAt = i.CreatedAt,
                    UpdatedAt = i.UpdatedAt,
                }));
            IncludeCategory(res.Items);
            return res;
        }

        private void IncludeCategory(ArticleListItem[] items)
        {
            var idItems = items.Select(item => item.CatId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.ArticleCategories.Where(i => idItems.Contains(i.Id))
                .Select(i => new ListLabelItem(i.Id, i.Name))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.CatId > 0 && data.TryGetValue(item.CatId, out var res))
                {
                    item.Category = res;
                }
            }
        }

        public IOperationResult<ArticleEntity> Get(int id)
        {
            var model = db.Articles.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<ArticleEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<ArticleEntity> Save(ArticleForm data)
        {
            var model = data.Id > 0 ? db.Articles.Where(i => i.Id == data.Id).SingleOrDefault()
                : new ArticleEntity();
            if (model == null)
            {
                return OperationResult.Fail<ArticleEntity>("数据有误");
            }
            model.Title = data.Title;
            model.Description = data.Description;
            model.Thumb = data.Thumb;
            model.CatId = data.CatId;
            model.Brief = data.Brief;
            model.Content = data.Content;
            model.Url = data.Url;
            model.File = data.File;
            db.Articles.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Articles.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public IPage<ArticleCategoryEntity> CategoryList(QueryForm form)
        {
            return db.ArticleCategories.Search(form.Keywords, "name")
                .ToPage(form);
        }

        public IOperationResult<ArticleCategoryEntity> Category(int id)
        {
            var model = db.ArticleCategories.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<ArticleCategoryEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<ArticleCategoryEntity> CategorySave(ArticleCategoryForm data)
        {
            var model = data.Id > 0 ? db.ArticleCategories.Where(i => i.Id == data.Id).SingleOrDefault()
                : new ArticleCategoryEntity();
            if (model == null)
            {
                return OperationResult.Fail<ArticleCategoryEntity>("数据有误");
            }
            model.Name = data.Name;
            model.Description = data.Description;
            model.Position = data.Position;
            model.ParentId = data.ParentId;
            model.Keywords = data.Keywords;
            db.ArticleCategories.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void CategoryRemove(int id)
        {
            db.ArticleCategories.Where(i => i.Id == id).ExecuteDelete();
            db.Articles.Where(i => i.CatId == id).ExecuteDelete();
            db.SaveChanges();
        }

        public ITreeItem[] CategoryAll()
        {
            var data = db.ArticleCategories.OrderBy(i => i.Position)
                .Select(i => new CategoryTreeItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    ParentId = i.ParentId
                })
                .ToArray();
            return TreeHelper.Sort(data);
        }

    }
}
