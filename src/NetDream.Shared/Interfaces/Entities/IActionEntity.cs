namespace NetDream.Shared.Interfaces.Entities
{
    public interface IActionEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public byte ItemType { get; set; }
        public byte Action { get; set; }

        public int CreatedAt { get; set; }

        
    }
}
