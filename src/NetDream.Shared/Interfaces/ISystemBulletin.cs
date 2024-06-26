using NetDream.Shared.Helpers;
using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface ISystemBulletin
    {
        public LinkRule Ruler { get; }
        public int SendAdministrator(string title, string content, byte type = 99, IEnumerable<LinkExtraRule>? extraRule = null);
    }
}
