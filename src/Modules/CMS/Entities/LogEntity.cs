
namespace NetDream.Modules.CMS.Entities
{
    public class LogEntity
    {
        public int Id { get; set; }
        
        public int ModelId { get; set; }
        
        public byte ItemType { get; set; }
        
        public int ItemId { get; set; }
        
        public int UserId { get; set; }
        public int Action { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
