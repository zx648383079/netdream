
namespace NetDream.Modules.Bot.Entities
{
    
    public class TemplateEntity
    {
        
        public int Id { get; set; }
        
        public int BotId { get; set; }
        
        public string TemplateId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;
    }
}
