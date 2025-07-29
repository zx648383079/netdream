using NetDream.Shared.Models;

namespace NetDream.Modules.Navigation.Forms
{
    public class SiteQueryForm : QueryForm
    {
        public int Category { get; set; }

        public string Domain { get; set; } = string.Empty;

        public int User { get; set; }
    }
}
