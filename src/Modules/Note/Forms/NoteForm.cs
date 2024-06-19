using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Note.Forms
{
    public class NoteForm
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Content { get; set; } = string.Empty;
    }
}
