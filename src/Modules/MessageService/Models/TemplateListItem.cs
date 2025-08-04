namespace NetDream.Modules.MessageService.Models
{
    public class TemplateListItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }

        public string TargetNo { get; set; } = string.Empty;

        public byte Status { get; set; }


        public int CreatedAt { get; set; }
    }
}
