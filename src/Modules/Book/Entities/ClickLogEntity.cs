
namespace NetDream.Modules.Book.Entities
{
    
    public class ClickLogEntity
    {
        
        public int Id { get; set; }
        
        public byte ItemType { get; internal set; }
        public string HappenDay { get; internal set; }
        public uint ItemId { get; internal set; }
        public byte Action { get; internal set; }
        public uint HappenCount { get; internal set; }

        public uint CreatedAt { get; set; }
    }
}
