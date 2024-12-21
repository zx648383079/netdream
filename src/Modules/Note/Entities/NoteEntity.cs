using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Note.Entities
{
    
    public class NoteEntity: IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public byte IsNotice { get; set; }
        public byte Status { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
