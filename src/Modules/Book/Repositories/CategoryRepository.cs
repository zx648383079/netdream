using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Forms;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Book.Repositories
{
    public class CategoryRepository(BookContext db)
    {
        public IPage<CategoryEntity> GetMangeList(QueryForm form)
        {
            return db.Categories.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form);
        }

        public CategoryEntity? Get(int id)
        {
            return db.Categories.Where(i => i.Id == id).Single();
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

        public CategoryListItem[] GetList(string keywords = "")
        {
            var items = db.Categories.Search(keywords, "name")
                .OrderByDescending(i => i.Id).SelectAs().ToArray();
            foreach (var item in items)
            {
                var thumb = db.Books.Where(i => i.CatId == item.Id).Select(i => i.Cover).Single();
                if (string.IsNullOrWhiteSpace(thumb))
                {
                    thumb = BookRepository.DEFAULT_COVER;
                }
                item.Thumb = thumb;
                item.BookCount = db.Books.Where(i => i.CatId == item.Id).Count();
            }
            return items;
        }
    }
}
