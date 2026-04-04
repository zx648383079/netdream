using NetDream.Shared.Interfaces;

namespace NetDream.Modules.UserAccount.Entities
{
    /// <summary>
    /// 用户互动记录，点赞、踩等
    /// </summary>
    public class InteractLogEntity : IInteractEntity, IIdEntity, ICreatedEntity
    {

        public int Id { get; set; }

        public byte ItemType { get; set; }

        public int ItemId { get; set; }

        public int UserId { get; set; }
        public byte Action { get; set; }
        public string Data { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public int CreatedAt { get; set; }
    }
}
