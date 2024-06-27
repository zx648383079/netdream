using Modules.Note.Entities;
using NetDream.Shared.Interfaces.Entities;
using NPoco;

namespace NetDream.Modules.Note.Models
{
    public class NoteModel : NoteEntity, IWithUserModel
    {
        [Ignore]
        public IUser? User { get; set; }
    }
}
