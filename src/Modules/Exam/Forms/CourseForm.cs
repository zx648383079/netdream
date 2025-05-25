using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Exam.Forms
{
    public class CourseForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ParentId { get; set; }
    }
}
