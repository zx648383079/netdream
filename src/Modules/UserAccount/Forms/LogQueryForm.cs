using NetDream.Shared.Models;

namespace NetDream.Modules.UserAccount.Forms
{
    public class LogQueryForm : QueryForm
    {
        public int User { get; set; }

        public int Type { get; set; }
    }
}
