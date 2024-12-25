using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Blog.Repositories;

namespace NetDream.Razor.Pages.Blog
{
    public class TagModel : PageModel
    {

        private readonly BlogRepository _repository;
        public TagModel(BlogRepository repository)
        {
            _repository = repository;
        }

        public Modules.Blog.Models.TagModel[] Items;

        public void OnGet()
        {
            Items = _repository.GetTags();
        }
    }
}
