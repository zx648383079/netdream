using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Article.Entities;
using NetDream.Modules.Article.Models;
using NetDream.Modules.Article.Repositories;

namespace NetDream.Razor.Pages.Blog
{
    public class DetailModel : PageModel
    {
        private readonly BlogRepository _repository;
        public DetailModel(BlogRepository repository)
        {
            _repository = repository;
        }

        public BlogModel Data;
        public CategoryLabelItem[] Categories;
        public string FullUrl;

        public void OnGet(int id)
        {
            Data = _repository.Get(id).Result;
            Categories = _repository.Categories();
            FullUrl = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
        }
    }
}
