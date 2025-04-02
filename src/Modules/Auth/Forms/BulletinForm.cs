using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Forms
{
    public class BulletinForm
    {
        [Required]
        public int User { get; set; }

        public string Title { get; set; } = "消息";
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
