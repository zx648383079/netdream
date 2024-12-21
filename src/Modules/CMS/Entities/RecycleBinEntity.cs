using NetDream.Shared.Interfaces.Entities;


namespace NetDream.Modules.CMS.Entities
{
    
    public class RecycleBinEntity : IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }

        
        public int SiteId { get; set; }

        
        public int ModelId { get; set; }

        
        public byte ItemType { get; set; }

        
        public int ItemId { get; set; }

        
        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Remark { get; set; } = string.Empty;

        public string Data { get; set; } = string.Empty;

        
        public int CreatedAt { get; set; }

    }
}
