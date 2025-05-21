using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Catering.Models
{
    public class StoreListItem : IWithUserModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public byte OpenStatus { get; set; }
        public int CreatedAt { get; set; }
        public IUser? User {  get; set; }
    }
}
