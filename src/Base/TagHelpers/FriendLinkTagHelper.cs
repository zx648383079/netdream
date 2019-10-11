using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Base.TagHelpers
{
    [HtmlTargetElement("friend-link")]
    public class FriendLinkTagHelper : TagHelper
    {
        

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "friend - link");
            var html = new StringBuilder("<div>友情链接</div><div>");


            html.Append("</div>");
            output.Content.SetHtmlContent(html.ToString());
            output.TagMode = TagMode.StartTagAndEndTag;


        }
    }
}
