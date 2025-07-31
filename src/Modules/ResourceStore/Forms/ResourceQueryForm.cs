using NetDream.Shared.Models;

namespace NetDream.Modules.ResourceStore.Forms
{
    public class ResourceQueryForm : QueryForm
    {
        public int User { get; set; }
        public int Category { get; set; }
    }
}
