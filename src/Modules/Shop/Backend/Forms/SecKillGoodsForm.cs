using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class SecKillGoodsForm
    {
        public int Id { get; set; }

        [Required]
        public int ActId { get; set; }
        [Required]
        public int TimeId { get; set; }
        [Required]
        public int GoodsId { get; set; }
        public float Price { get; set; }
        public int Amount { get; set; }

        public int EveryAmount { get; set; }
    }
}
