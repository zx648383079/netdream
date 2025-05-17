using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Models;
using NetDream.Modules.Blog.Repositories;

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
        public CategoryListItem[] Categories;
        public string FullUrl;

        public void OnGet(int id)
        {
            Data = _repository.GetBlog(id);
            Categories = _repository.Categories();
            FullUrl = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
        }
    }
}
