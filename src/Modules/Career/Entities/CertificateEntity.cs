using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Career.Entities;
public class CertificateEntity : IIdEntity, ITimestampEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
    public byte IsGot { get; set; }
    public int GotAt { get; set; }
    public int UpdatedAt { get; set; }
    public int CreatedAt { get; set; }
    
}