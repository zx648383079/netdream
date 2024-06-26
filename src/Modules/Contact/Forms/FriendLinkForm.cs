using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Contact.Forms
{
    public class FriendLinkForm
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(250)]
        public string Url { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        [StringLength(250)]
        public string Brief { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
