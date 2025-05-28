using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class DeliveryForm
    {
        [Required]
        public int Id { get; set; }

        public LogisticsItem[] LogisticsContent { get; set; }

        public int Status { get; set; }
    }

    public class LogisticsItem
    {
        public int Status { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }
    }
}
