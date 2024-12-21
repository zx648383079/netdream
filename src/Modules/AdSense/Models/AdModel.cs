using NetDream.Modules.AdSense.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.AdSense.Models
{
    public class AdModel: AdEntity
    {
        public PositionEntity? Position { get; set; }
    }
}
