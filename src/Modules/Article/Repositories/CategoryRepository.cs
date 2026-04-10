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

        public IPage<CategoryEntity> AdvancedGet(ModuleTargetType type, QueryForm form)
        {
            return db.Categories.Where(i => i.Type == (byte)type)
                .Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public IOperationResult<CategoryEntity> AdvancedSave(ModuleTargetType type, CategoryForm data)
        {
            var model = data.Id > 0 ? db.Categories.Where(i => i.Id == data.Id && i.Type == (byte)type)
                .SingleOrDefault() :
                new CategoryEntity();
            if (model is null)
            {
                return OperationResult.Fail<CategoryEntity>("id error");
            }
            model.Type = (byte)type;
            model.Name = data.Name;
            model.ParentId = data.ParentId;
            model.Description = data.Description;
            db.Categories.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void AdvancedRemove(ModuleTargetType type, int id)
        {
            db.Categories.Where(i => i.Id == id && i.Type == (byte)type).ExecuteDelete();
            db.SaveChanges();
        }


        public IPage<CategoryEntity> Search(ModuleTargetType type, QueryForm form, int[] idItems)
        {
            return db.Categories.Where(i => i.Type == (byte)type)
                .Search(form.Keywords, "name")
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

        public ILevelItem[] Get(ModuleTargetType type, int[] excludes)
        {
            var data = db.Categories.Where(i => i.Type == (byte)type && !excludes.Contains(i.Id) && !excludes.Contains(i.ParentId))
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

        public IOperationResult<IListLabelItem> Get(ModuleTargetType type, int id)
        {
            var res = db.Categories.Where(i => i.Id == id && i.Type == (byte)type)
                .Select(i => new ListLabelItem(i.Id, i.Name)).SingleOrDefault();
            if (res is null)
            {
                return OperationResult<IListLabelItem>.Fail("id is error");
            }
            return OperationResult<IListLabelItem>.Ok(res);
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
