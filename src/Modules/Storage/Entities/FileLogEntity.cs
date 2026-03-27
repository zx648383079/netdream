
namespace NetDream.Modules.Storage.Entities
{
    
    public class FileLogEntity
    {
        
        public int Id { get; set; }
        
        public int FileId { get; set; }
        
        public byte ItemType { get; set; }
        
        public int ItemId { get; set; }
        public string Data { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
