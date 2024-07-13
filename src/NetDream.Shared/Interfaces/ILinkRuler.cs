using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface ILinkRuler
    {
        public string Render(string content, IEnumerable<LinkExtraRule>? rules);

        public LinkExtraRule FormatUser(string word, int user);
        public LinkExtraRule FormatImage(string word, string image);
        public LinkExtraRule FormatFile(string word, string file);
        public LinkExtraRule FormatId(string word, string id);
        public LinkExtraRule FormatLink(string word, string link, IDictionary<string, object>? queries = null);
        public IList<LinkExtraRule> FormatEmoji(string content);
    }
}
