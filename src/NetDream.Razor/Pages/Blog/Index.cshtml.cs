using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Razor.Entities;
using NPoco;

namespace NetDream.Razor.Pages.Blog
{
    public class IndexModel : PageModel
    {
        private readonly IDatabase _db;
        public IndexModel(IDatabase db)
        {
            _db = db;
        }

        public NPoco.Page<BlogEntity> Items;
        public List<CategoryEntity> Categories;
        public List<BlogEntity> NewItems;
        public string FullUrl;
        public int PageIndex;

        public void OnGet(int page = 1)
        {
            PageIndex = page;
            FullUrl = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
            Items = _db.Page<BlogEntity>(page, 20, "SELECT * FROM blog");
            Categories = _db.Fetch<CategoryEntity>();
            NewItems = _db.Fetch<BlogEntity>("select id, title, description, created_at from blog order by created_at desc limit @0", 5);
        }
    }
}
