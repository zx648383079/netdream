using Modules.Note.Entities;
using NetDream.Core.Interfaces.Entities;
using NPoco;

namespace NetDream.Modules.Note.Models
{
    public class NoteModel : NoteEntity
    {
        [Ignore]
        public IUser? User { get; set; }
    }
}
