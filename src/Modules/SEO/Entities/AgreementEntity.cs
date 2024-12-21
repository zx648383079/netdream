using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Repositories.Models;


namespace NetDream.Modules.SEO.Entities
{
    
    public class AgreementEntity: IIdEntity, ITimestampEntity, ILanguageEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
