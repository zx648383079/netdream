
namespace NetDream.Modules.MicroBlog.Entities
{
    
    public class LogEntity
    {
        
        public int Id { get; set; }
        
        public byte ItemType { get; set; }
        
        public int ItemId { get; set; }
       
        public int UserId { get; set; }
        public int Action { get; set; }
        public string Ip { get; internal set; }
        public int CreatedAt { get; set; }
      
    }
}
