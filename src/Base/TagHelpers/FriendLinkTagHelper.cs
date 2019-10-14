using Microsoft.AspNetCore.Razor.TagHelpers;
using NetDream.Models;
using System.Text;

namespace NetDream.Base.TagHelpers
{
    [HtmlTargetElement("friend-link")]
    public class FriendLinkTagHelper : TagHelper
    {
        public string Title = "友情链接";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "friend-link");
            var html = new StringBuilder();
            html.AppendFormat("<div>{0}</div><div>", Title);
            var items = FriendLinkModel.All();
            foreach (var item in items)
            {
                html.AppendFormat("<a href=\"{0}\" target=\"_blank\" rel=\"noopener noreferrer\">{1}</a>", item.Url, item.Name);
            }

            html.Append("</div>");
            output.Content.SetHtmlContent(html.ToString());
        }
    }
}
