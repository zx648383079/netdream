using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Trade.Forms
{
    public class OrderForm
    {
        [Required]
        public int ServiceId { get; set; }

        public IDictionary<string, string> Remark { get; set; }

        public int Amount { get; set; }
    }
}
