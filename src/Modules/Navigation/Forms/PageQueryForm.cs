using NetDream.Shared.Models;

namespace NetDream.Modules.Navigation.Forms
{
    public class PageQueryForm : QueryForm
    {
        public int User { get; set; }

        public int Site { get; set; }

        public string Domain { get; set; }
    }
}
