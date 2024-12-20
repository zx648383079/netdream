using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Forms
{
    public class EquityCardForm
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string Configure { get; set; } = string.Empty;
    }
}
