using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Repositories;
using NPoco;

namespace NetDream.Modules.Book.Repositories
{
    public class CategoryRepository(IDatabase db) : CRUDRepository<CategoryEntity>(db)
    {

        public CategoryModel[] GetList(string keywords = "", int page = 1)
        {
            var items = db.Fetch<CategoryModel>();
            foreach (var item in items)
            {
                var thumb = db.FindScalar<string, BookEntity>("cover", "cat_id=@0", item.Id);
                if (string.IsNullOrWhiteSpace(thumb))
                {
                    thumb = BookRepository.DEFAULT_COVER;
                }
                item.Thumb = thumb;
                item.BookCount = db.FindCount<BookEntity>("cat_id=@0", item.Id);
            }
            return [..items];
        }
    }
}
