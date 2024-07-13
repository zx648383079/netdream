using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Modules.SEO.Repositories
{
    public class LinkRuler(IDeeplink deeplink) : LinkRule(deeplink), ILinkRuler
    {
        public IList<LinkExtraRule> FormatEmoji(string content)
        {
            return [];
        }
    }
}
