using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Blog.Repositories;
using NPoco;

namespace NetDream.Razor.Pages.Blog
{
    public class CategoryModel : PageModel
    {
        private readonly BlogRepository _repository;
        public CategoryModel(BlogRepository repository)
        {
            _repository = repository;
        }

        public List<Modules.Blog.Models.CategoryModel> Items;

        public void OnGet()
        {
            Items = _repository.Categories();
        }
    }
}
