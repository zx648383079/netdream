using Microsoft.AspNetCore.Razor.TagHelpers;
using NetDream.Areas.Blog.Repositories;
using NetDream.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Base.TagHelpers
{
    [HtmlTargetElement("blog-panel")]
    public class BlogPanelTagHelper: TagHelper
    {

        public int Count = 8;

        private BlogRepository blogRepository;

        public BlogPanelTagHelper(BlogRepository repository)
        {
            blogRepository = repository;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var items = blogRepository.GetNewBlogs(Count);
            output.TagName = "div";
            var html = new StringBuilder();
            foreach (var item in items)
            {
                html.AppendFormat("<div class=\"list-item\"><a class=\"name\" href=\"/Blog/{0}\">{1}</a><div class=\"time\">{2}</div></div>", item.Id, item.Title, Time.FormatAgo(item.CreatedAt));
            }
            output.Content.SetHtmlContent(html.ToString());
        }
    }
}
