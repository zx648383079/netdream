using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetDream.Modules.TradeTracker.Importers
{
    internal class IDMapperImporter
    {


        public static string FormatName(string name)
        {
            return name.Replace('（', '(').Replace('）', ')');
        }

        public static (string, string) SplitName(string name) {
            name = FormatName(name);
            if (!name.EndsWith(')')) {
                return (name, string.Empty);
            }
            var i = name.IndexOf('(');
            if (i <= 0) {
                return (name, string.Empty);
            }
            return (name[..i].Trim(), name[(i + 1)..^2].Trim());
        }
    }
}
