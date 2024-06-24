using Modules.AdSense.Entities;
using NetDream.Modules.AdSense.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.AdSense.Repositories
{
    public class TemplateCompiler
    {

        public static string Render(string template, IList<FormattedAdModel> data)
        {
            return string.Empty;
        }

        public static string RenderAd(AdEntity item, PositionEntity formatted)
        {
            throw new NotImplementedException();
        }
    }
}
