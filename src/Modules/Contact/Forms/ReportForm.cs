﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Contact.Forms
{
    public class ReportForm
    {
        public string Email { get; set; } = string.Empty;
        public byte ItemType { get; set; }
        public int ItemId { get; set; }
        public byte Type { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        public string Files { get; set; } = string.Empty;
    }
}
