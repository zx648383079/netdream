using NetDream.Shared.Models;

namespace NetDream.Modules.Note.Forms
{
    public class NoteQueryForm : QueryForm
    {
        public int User { get; set; }

        public int Id { get; set; }

        public int Notice { get; set; }
    }
}
