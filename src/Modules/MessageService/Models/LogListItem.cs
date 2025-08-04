namespace NetDream.Modules.MessageService.Models
{
    public class LogListItem
    {
        public int Id { get; set; }

        public string Target { get; set; } = string.Empty;

        public string TemplateName { get; set; } = string.Empty;
        
        public byte Status { get; set; }
        public string Message { get; set; } = string.Empty;


        public int CreatedAt { get; set; }
    }
}
