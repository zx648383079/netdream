using Microsoft.AspNetCore.Razor.TagHelpers;
using NetDream.Shared.Helpers;
using NetDream.Modules.Note.Repositories;
using System.Text;

namespace NetDream.Web.Base.TagHelpers
{
    /// <summary>
    /// 最新通知
    /// </summary>
    /// <param name="repository"></param>
    [HtmlTargetElement("notice-panel")]
    public class NoticePanelTagHelper(NoteRepository repository) : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var items = repository.GetNewList();
            output.TagName = "div";
            var html = new StringBuilder();
            foreach (var item in items)
            {
                html.AppendFormat("<div class=\"note-item\"><div class=\"item-body\">{0}</div><div class=\"item-time\">{1}</div></div>", item.Content, TimeHelper.FormatAgo(item.CreatedAt));
            }
            output.Content.SetHtmlContent(html.ToString());
        }
    }
}
