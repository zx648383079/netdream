using NetDream.Shared.Interfaces.Entities;


namespace NetDream.Modules.OnlineDisk.Entities
{
    
    public class DiskEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        
        public int FileId { get; set; }

        
        public int UserId { get; set; }

        
        public int LeftId { get; set; }

        
        public int RightId { get; set; }

        
        public int ParentId { get; set; }

        
        public int DeletedAt { get; set; }

        
        public int UpdatedAt { get; set; }

        
        public int CreatedAt { get; set; }

    }
}
