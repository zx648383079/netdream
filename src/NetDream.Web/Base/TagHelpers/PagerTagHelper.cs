using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NetDream.Web.Base.TagHelpers
{
    [HtmlTargetElement("pagination")]
    public class PagerTagHelper : TagHelper
    {
        public long Total { get; set; } = 0;

        public int PerPage { get; set; } = 20;

        public int Page { get; set; } = 1;

        public int PageLength { get; set; } = 7;

        private string url;

        public string Url
        {
            get { return url; }
            set {
                if (value.IndexOf('?') < 0)
                {
                    url = value + "?page=";
                    return;
                }
                url = Regex.Replace(value, @"([\?\&])page=\d+\&*", "$1") + "&page=";
            }
        }


        public bool DirectionLinks { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            var html = new StringBuilder();
            var items = initLinks(out bool canPrevious, out bool canNext);
            html.Append("<ul class=\"pagination\">");
            if (DirectionLinks && canPrevious)
            {
                html.AppendFormat("<li class=\"page-item{0}\"><a class=\"page-link\" href=\"{1}{2}\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">上一页</span></a></li>", 
                    canPrevious ? "" : " disabled",
                    Url, Page - 1
                    );
            }
            foreach (var item in items)
            {
                if (item < 1)
                {
                    html.Append("<li class=\"page-item disabled\"><a class=\"page-link\">...</a></li>");
                    continue;
                }
                html.AppendFormat("<li class=\"page-item{0}\"><a class=\"page-link\" href=\"{2}{3}\">{1}</a></li>",
                    item == Page ? " active" : "",
                    item,
                    Url, item
                    );
            }
            if (DirectionLinks && canNext)
            {
                html.AppendFormat("<li class=\"page-item{0}\"><a class=\"page-link\" href=\"{1}{2}\"  aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">下一页</span></a></li>",
                    canNext ? "" : " disabled",
                    Url, Page + 1
                    );
            }
            html.Append("</ul>");
            output.Content.SetHtmlContent(html.ToString());
        }

        private List<int> initLinks(out bool canPrevious, out bool canNext)
        {
            var total = (int)Math.Ceiling((double)Total / PerPage);
            canPrevious = Page > 1;
            canNext = Page < total;
            var items = new List<int>();
            if (total < 2)
            {
                return items;
            }
            items.Add(1);
            var lastList = (int)Math.Floor((double)PageLength / 2);
            var i = Page - lastList;
            var length = Page + lastList;
            if (i < 2)
            {
                i = 2;
                length = i + PageLength;
            }
            if (length > total - 1)
            {
                length = total - 1;
                i = Math.Max(2, length - PageLength);
            }

            if (i > 2)
            {
                items.Add(0);
            }
            for (; i <= length; i++)
            {
                items.Add(i);
            }
            if (length < total - 1)
            {
                items.Add(0);
            }
            items.Add(total);
            return items;
        }
    }
}
