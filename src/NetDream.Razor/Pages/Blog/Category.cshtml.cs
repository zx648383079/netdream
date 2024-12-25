using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Blog.Repositories;

namespace NetDream.Razor.Pages.Blog
{
    public class CategoryModel : PageModel
    {
        private readonly BlogRepository _repository;
        public CategoryModel(BlogRepository repository)
        {
            _repository = repository;
        }

        public Modules.Blog.Models.CategoryModel[] Items;

        public void OnGet()
        {
            Items = _repository.Categories();
        }
    }
}
