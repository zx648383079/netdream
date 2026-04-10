using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Article.Repositories;
using NetDream.Shared.Interfaces;

namespace NetDream.Razor.Pages.Blog
{
    public class CategoryModel(BlogRepository repository) : PageModel
    {
        public IListStatisticsItem[] Items;

        public void OnGet()
        {
            Items = repository.Categories();
        }
    }
}
