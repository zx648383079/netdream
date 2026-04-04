using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Article.Entities;
using NetDream.Modules.Article.Forms;
using NetDream.Modules.Article.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Article.Repositories
{
    public class CategoryRepository(ArticleContext db) : ICategoryRepository
    {

        public IPage<CategoryEntity> AdvancedGet(QueryForm form)
        {
            return db.Categories.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<CategoryEntity> AdvancedSave(CategoryForm data)
        {
            var model = data.Id > 0 ? db.Categories.Where(i => i.Id == data.Id)
                .SingleOrDefault() :
                new CategoryEntity();
            if (model is null)
            {
                return OperationResult.Fail<CategoryEntity>("id error");
            }
            model.Name = data.Name;
            model.ParentId = data.ParentId;
            model.Description = data.Description;
            db.Categories.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void AdvancedRemove(int id)
        {
            db.Categories.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }


        public IPage<CategoryEntity> Search(QueryForm form, int[] idItems)
        {
            return db.Categories.Search(form.Keywords, "name")
                .When(idItems?.Length > 0, i => idItems.Contains(i.Id))
                .ToPage(form);
        }

        public IOperationResult Add(ModuleTargetType type, IListLabelItem item)
        {
            db.Categories.Save(new CategoryEntity()
            {
                Type = (byte)type,
                Name = item.Name
            });
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult Update(ModuleTargetType type, IListLabelItem item)
        {
            db.Categories.Save(new CategoryEntity()
            {
                Id = item.Id,
                Type = (byte)type,
                Name = item.Name
            });
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public ILevelItem[] All(ModuleTargetType type)
        {
            var data = db.Categories.Where(i => i.Type == (byte)type)
                .Select(i => new CategoryTreeItem()
                {
                    Id = i.Id,
                    ParentId = i.ParentId,
                    Name = i.Name,
                }).ToArray();
            return TreeHelper.Sort(data);
        }

        public IListLabelItem[] Get(ModuleTargetType type)
        {
            return db.Categories.Where(i => i.Type == (byte)type)
                .OrderBy(i => i.Id)
                .Select(i => new ListLabelItem(i.Id, i.Name)).ToArray();
        }

        public void Remove(ModuleTargetType type, int id)
        {
            db.Categories.Where(i => i.Id == id && i.Type == (byte)type).ExecuteDelete();
            db.SaveChanges();
        }

        public ITreeItem[] Tree(ModuleTargetType type)
        {
            var data = db.Categories.Where(i => i.Type == (byte)type)
                .Select(i => new CategoryTreeItem()
            {
                Id = i.Id,
                ParentId = i.ParentId,
                Name = i.Name,
            }).OrderBy(i => i.ParentId).ToArray();
            return TreeHelper.Create(data);
        }

        public void Include(ModuleTargetType type, IEnumerable<IWithCategoryModel> items)
        {
            var idItems = items.Select(item => item.CatId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Categories.Where(i => i.Type == (byte)type && idItems.Contains(i.Id)).Select(i => new ListLabelItem(i.Id, i.Name))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.CatId > 0 && data.TryGetValue(item.CatId, out var cat))
                {
                    item.Category = cat;
                }
            }
        }
    }
}
