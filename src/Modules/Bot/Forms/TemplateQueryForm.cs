using NetDream.Shared.Models;

namespace NetDream.Modules.Bot.Forms
{
    public class TemplateQueryForm : QueryForm
    {
        public int Category { get; set; }
        public int Type { get; set; }
    }
}
