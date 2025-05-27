using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class WarehouseLogForm
    {
        public int WarehouseId { get; set; }

        public int GoodsId { get; set; }
        public int ProductId { get; set; }
        [Required]
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public string Remark { get; set; } = string.Empty;
    }
}
