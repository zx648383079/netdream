using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Repositories;
using NPoco;

namespace NetDream.Razor.Pages.Blog
{
    public class DetailModel : PageModel
    {
        private readonly BlogRepository _repository;
        public DetailModel(BlogRepository repository)
        {
            _repository = repository;
        }

        public BlogEntity Data;
        public List<NetDream.Modules.Blog.Models.CategoryModel> Categories;
        public string FullUrl;

        public void OnGet(int id)
        {
            Data = _repository.GetBlog(id);
            Categories = _repository.Categories();
            FullUrl = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
        }
    }
}
