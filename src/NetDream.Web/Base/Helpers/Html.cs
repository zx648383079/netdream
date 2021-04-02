using NetDream.Core.Helpers;

namespace NetDream.Web.Base.Helpers
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
