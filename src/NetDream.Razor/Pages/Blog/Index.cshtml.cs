using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Article.Forms;
using NetDream.Modules.Article.Models;
using NetDream.Modules.Article.Repositories;
using NetDream.Shared.Interfaces;

namespace NetDream.Razor.Pages.Blog
{
    public class IndexModel(BlogRepository repository) : PageModel
    {
        public IPage<ArticleListItem> Items;
        public IListStatisticsItem[] Categories;
        public ArticleListItem[] NewItems;
        public string FullUrl;
        public int PageIndex;

        public void OnGet([FromQuery] ArticleQueryForm form)
        {
            PageIndex = form.Page;
            FullUrl = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            Items = repository.GetList(form);
            Categories = repository.Categories();
            NewItems = repository.GetNewBlogs(5);
        }
    }
}
