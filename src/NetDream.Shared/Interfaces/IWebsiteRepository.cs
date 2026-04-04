using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IWebsiteRepository
    {
        public IWebsiteLabelItem Recognize(string link);
        public IDictionary<string, IWebsiteLabelItem> Recognize(IEnumerable<string> items);
    }
}
