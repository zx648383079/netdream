using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface ISystemBulletin
    {
        public ILinkRuler Ruler { get; }
        public int SendAdministrator(string title, string content, byte type = 99, IEnumerable<LinkExtraRule>? extraRule = null);

        public int SendAt(int[] user, string title, string link);
    }
}
