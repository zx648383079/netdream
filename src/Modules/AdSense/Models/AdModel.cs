using Modules.AdSense.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.AdSense.Models
{
    public class AdModel: AdEntity
    {

        [Ignore]
        public PositionEntity? Position { get; set; }
    }
}
