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
    public class CategoryModel : PageModel
    {
        private readonly IDatabase _db;
        public CategoryModel(IDatabase db)
        {
            _db = db;
        }

        public List<CategoryEntity> Items;

        public void OnGet()
        {
            Items = _db.Fetch<CategoryEntity>();
        }
    }
}
