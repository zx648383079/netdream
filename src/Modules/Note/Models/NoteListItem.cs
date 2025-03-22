using NetDream.Modules.Note.Entities;
using NetDream.Modules.Note.Repositories;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Note.Models
{
    public class NoteListItem: IWithUserModel
    {
        public int Id { get; set; }
        public string Html { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int CreatedAt { get; set; }

        public IUser? User { get; set; }

        public NoteListItem()
        {
            
        }

        public NoteListItem(NoteEntity entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;
            CreatedAt = entity.CreatedAt;
            Html = NoteRepository.RenderHtml(entity.Content);
        }
    }
}
