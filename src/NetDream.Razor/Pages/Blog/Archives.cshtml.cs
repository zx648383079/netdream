using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Article.Models;
using NetDream.Modules.Article.Repositories;

namespace NetDream.Razor.Pages.Blog
{
    public class ArchivesModel : PageModel
    {
        private readonly BlogRepository _repository;
        public ArchivesModel(BlogRepository repository)
        {
            _repository = repository;
        }

        public ArchiveListItem[] Items;

        public void OnGet()
        {
            Items = _repository.GetArchives();
        }

    }
}
