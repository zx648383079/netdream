using System;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Finance.Forms
{
    public class ProjectForm
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public float Money { get; set; }
        public int AccountId { get; set; }
        public float Earnings { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public float EarningsNumber { get; set; }
        public int ProductId { get; set; }
        public byte Status { get; set; }
        public byte Color { get; set; }
        public string Remark { get; set; } = string.Empty;
    }
}
