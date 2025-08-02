using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Blog.Models;
using NetDream.Modules.Blog.Repositories;

namespace NetDream.Razor.Pages.Blog
{
    public class ArchivesModel : PageModel
    {
        private readonly BlogRepository _repository;
        public ArchivesModel(BlogRepository repository)
        {
            _repository = repository;
        }

        public BlogArchiveItem[] Items;

        public void OnGet()
        {
            Items = _repository.GetArchives();
        }

    }
}
