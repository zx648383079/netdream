using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Base.Helpers
{
    public class Str
    {

        public static string Studly(string val)
        {
            var data = val.Split('-', '_', ' ');
            var res = new StringBuilder();
            foreach (var item in data)
            {
                res.Append(item.Substring(0, 1).ToUpper());
                res.Append(item.Substring(1).ToLower());
            }
            return res.ToString();
        }

        public static string Base64Encode(string val)
        {
            var bytes = Encoding.UTF8.GetBytes(val);
            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(string val)
        {
            var bytes = Convert.FromBase64String(val);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
