using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Modules.Blog.Forms;
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
        public CategoryLabelItem[] Categories;
        public BlogListItem[] NewItems;
        public string FullUrl;
        public int PageIndex;

        public void OnGet([FromQuery] BlogQueryForm form)
        {
            PageIndex = form.Page;
            FullUrl = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            Items = _repository.GetList(form);
            Categories = _repository.Categories();
            NewItems = _repository.GetNewBlogs(5);
        }
    }
}
