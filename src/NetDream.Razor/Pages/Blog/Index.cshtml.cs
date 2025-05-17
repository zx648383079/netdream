using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Models;
using NetDream.Modules.Blog.Repositories;
using NetDream.Shared.Interfaces;

namespace NetDream.Razor.Pages.Blog
{
    public class IndexModel : PageModel
    {
        private readonly BlogRepository _repository;
        public IndexModel(BlogRepository repository)
        {
            _repository = repository;
        }

        public IPage<BlogListItem> Items;
        public CategoryListItem[] Categories;
        public BlogListItem[] NewItems;
        public string FullUrl;
        public int PageIndex;

        public void OnGet(int page = 1)
        {
            PageIndex = page;
            FullUrl = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            Items = _repository.GetPage(page);
            Categories = _repository.Categories();
            NewItems = _repository.GetNewBlogs(5);
        }
    }
}
