using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Base.Helpers
{
    public class Html
    {
        public static string ToUrl(string uri)
        {
            if (uri.IndexOf("//") < 0)
            {
                return uri;
            }
            return "/To?url=" + Str.Base64Encode(uri).Trim('=');
        }
    }
}
