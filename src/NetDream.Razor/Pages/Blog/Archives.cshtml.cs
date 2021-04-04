using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetDream.Core.Helpers;
using NetDream.Razor.Entities;
using NPoco;

namespace NetDream.Razor.Pages.Blog
{
    public class ArchivesModel : PageModel
    {
        private readonly IDatabase _db;
        public ArchivesModel(IDatabase db)
        {
            _db = db;
        }

        public List<BlogArchives> Items;

        public void OnGet()
        {
            Items = GetArchives();
        }

        private List<BlogArchives> GetArchives()
        {
            var data = _db.Fetch<BlogEntity>("select id, title, created_at from blog order by created_at desc");
            var items = new List<BlogArchives>();
            BlogArchives last = null;
            foreach (var item in data)
            {
                var date = Time.TimestampTo(item.CreatedAt);
                if (last != null && last.Year == date.Year)
                {
                    last.Items.Add(item);
                    continue;
                }
                last = new BlogArchives();
                last.Year = date.Year;
                last.Items.Add(item);
                items.Add(last);
            }
            return items;
        }
    }
}
