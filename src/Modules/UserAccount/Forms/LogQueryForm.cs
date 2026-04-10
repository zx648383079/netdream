using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Modules.UserAccount.Forms
{
    public class LogQueryForm : QueryForm, ISourceQueryForm
    {
        public int User { get; set; }

        public int Type { get; set; }
    }
}
