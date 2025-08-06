using NetDream.Shared.Models;

namespace NetDream.Modules.Bot.Forms
{
    public class UserQueryForm  :QueryForm
    {
        public int Group { get; set; }

        public int Blacklist { get; set; }
    }
}
