using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Exam.Entities;
public class UpgradeUserEntity : IIdEntity, ICreatedEntity
{
    public int Id { get; set; }
    public int UpgradeId { get; set; }
    public int UserId { get; set; }
    public int CreatedAt { get; set; }
    
}