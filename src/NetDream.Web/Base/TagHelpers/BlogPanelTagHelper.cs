using Microsoft.AspNetCore.Razor.TagHelpers;
using NetDream.Core.Helpers;
using NetDream.Modules.Blog.Repositories;
using System.Text;

namespace NetDream.Web.Base.TagHelpers
{
    [HtmlTargetElement("blog-panel")]
    public class BlogPanelTagHelper(BlogRepository repository) : TagHelper
    {

        public int Count = 8;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var items = repository.GetNewBlogs(Count);
            output.TagName = "div";
            var html = new StringBuilder();
            foreach (var item in items)
            {
                html.AppendFormat("<div class=\"list-item\"><a class=\"name\" href=\"/Blog/Home/Detail/{0}\">{1}</a><div class=\"time\">{2}</div></div>", item.Id, item.Title, TimeHelper.FormatAgo(item.CreatedAt));
            }
            output.Content.SetHtmlContent(html.ToString());
        }
    }
}
