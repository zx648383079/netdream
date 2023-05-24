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
    public class TagModel : PageModel
    {

        private readonly BlogRepository _repository;
        public TagModel(BlogRepository repository)
        {
            _repository = repository;
        }

        public List<NetDream.Modules.Blog.Models.TagModel> Items;

        public void OnGet()
        {
            Items = _repository.GetTags();
        }
    }
}
