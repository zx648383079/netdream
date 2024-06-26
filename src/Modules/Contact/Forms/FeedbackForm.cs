﻿using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Contact.Forms
{
    public class FeedbackForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Phone]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
