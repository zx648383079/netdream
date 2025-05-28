using System;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class ActivityTimeForm
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;

        public TimeSpan StartAt { get; set; }

        public TimeSpan EndAt { get; set; }
    }
}
