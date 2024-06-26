using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Contact.Forms
{
    public class SubscribeForm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
