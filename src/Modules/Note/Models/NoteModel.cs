using Modules.Note.Entities;
using NetDream.Core.Interfaces.Entities;

namespace NetDream.Modules.Note.Models
{
    public class NoteModel : NoteEntity
    {

        public IUser? User { get; set; }
    }
}
