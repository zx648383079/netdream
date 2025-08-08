using NetDream.Shared.Models;

namespace NetDream.Modules.Auth.Forms
{
    public class CodeQueryForm : QueryForm
    {
        public int User { get; set; }

        public int Inviter { get; set; }
    }
}
