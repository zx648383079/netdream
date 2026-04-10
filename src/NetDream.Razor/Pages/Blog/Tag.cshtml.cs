using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Article.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Razor.Pages.Blog
{
    public class TagModel : PageModel
    {

        private readonly BlogRepository _repository;
        public TagModel(BlogRepository repository)
        {
            _repository = repository;
        }

        public StatisticsItem[] Items;

        public void OnGet()
        {
            Items = _repository.GetTags();
        }
    }
}
