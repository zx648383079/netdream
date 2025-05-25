using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Exam.Forms
{
    public class GradeForm
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public int Grade { get; set; } = 1;
    }
}
