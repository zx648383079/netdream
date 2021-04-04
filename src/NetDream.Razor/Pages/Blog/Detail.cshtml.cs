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
    public class DetailModel : PageModel
    {
        private readonly IDatabase _db;
        public DetailModel(IDatabase db)
        {
            _db = db;
        }

        public BlogEntity Data;
        public List<CategoryEntity> Categories;
        public string FullUrl;

        public void OnGet(int id)
        {
            Data = _db.SingleById<BlogEntity>(id);
            Categories = _db.Fetch<CategoryEntity>();
            FullUrl = $"{HttpContext.Request.Path}{HttpContext.Request.QueryString}";
        }
    }
}
