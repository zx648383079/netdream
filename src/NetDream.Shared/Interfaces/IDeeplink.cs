using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IDeeplink
    {

        public string Decode(string link);
        public string Encode(string path, IDictionary<string, object>? queries = null);
    }
}
