using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Document.Entities;
using NetDream.Modules.Document.Forms;
using NetDream.Modules.Document.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Document.Repositories
{
    public class CategoryRepository(DocumentContext db)
    {
        public IPage<CategoryEntity> GetList(QueryForm form)
        {
            return db.Categories.Search(form.Keywords, "Name").ToPage(form);
        }

        public IOperationResult<CategoryEntity> Get(int id)
        {
            var model = db.Categories.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<CategoryEntity>.Fail("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<CategoryEntity> Save(CategoryForm data)
        {
            var model = data.Id > 0 ? db.Categories.Where(i => i.Id == data.Id)
                 .Single() :
                 new CategoryEntity();
            if (model is null)
            {
                return OperationResult.Fail<CategoryEntity>("id error");
            }
            model.Name = data.Name;
            db.Categories.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Categories.Where(i => i.Id == id).ExecuteDelete();
        }

        public CategoryTreeItem[] LevelTree(int[] excludes)
        {
            var data = db.Categories.Where(i =>
                !excludes.Contains(i.Id) && !excludes.Contains(i.ParentId))
                .Select(i => new CategoryTreeItem()
                {
                    Id = i.Id,
                    ParentId = i.ParentId,
                    Name = i.Name,
                }).ToArray();
            return TreeHelper.Sort(data);
        }

        public ITreeItem[] Tree()
        {
            var data = db.Categories.Select(i => new CategoryTreeItem()
            {
                Id = i.Id,
                ParentId = i.ParentId,
                Name = i.Name,
            }).OrderBy(i => i.ParentId).ToArray();
            return TreeHelper.Create(data);
        }


        public IPage<CategoryEntity> Search(QueryForm form, int[] idItems)
        {
            return db.Categories.Search(form.Keywords, "name")
                .Where(i => idItems.Contains(i.Id))
                .ToPage(form);
        }
    }
}
