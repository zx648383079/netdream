using NetDream.Shared.Models;
using System;

namespace NetDream.Modules.Finance.Models
{
    public class ProjectListItem : IWithProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public float Money { get; set; }

        public float Earnings { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        public int ProductId { get; set; }
        public byte Status { get; set; }

        public int CreatedAt { get; set; }

        public ListLabelItem Product { get; set; }
    }
}
