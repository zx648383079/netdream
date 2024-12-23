using NetDream.Modules.Note.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Note.Models
{
    public class NoteModel : NoteEntity, IWithUserModel
    {
        public IUser? User { get; set; }

    }
}
