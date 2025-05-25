using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Exam.Forms
{
    public class MaterialForm
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
