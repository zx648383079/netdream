
namespace NetDream.Modules.Book.Entities
{
    
    public class LogEntity
    {
        
        public int Id { get; set; }
        
        public int ItemType { get; set; }
        
        public int ItemId { get; set; }
        
        public int UserId { get; set; }
        public int Action { get; set; }
        public string Ip { get; internal set; }
        public int CreatedAt { get; set; }
   
    }
}
