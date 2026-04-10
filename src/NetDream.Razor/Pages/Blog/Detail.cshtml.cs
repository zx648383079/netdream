using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Article.Models;
using NetDream.Modules.Article.Repositories;
using NetDream.Shared.Interfaces;

namespace NetDream.Razor.Pages.Blog
{
    public class DetailModel(BlogRepository repository) : PageModel
    {
        public ArticleOpenModel Data;
        public IListStatisticsItem[] Categories;
        public string FullUrl;

        public void OnGet(int id)
        {
            Data = repository.Get(id).Result;
            Categories = repository.Categories();
            FullUrl = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
        }
    }
}
