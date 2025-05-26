using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Exam.Forms
{
    public class QuestionForm
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public int CourseGrade { get; set; }
        public int MaterialId { get; set; }
        public int ParentId { get; set; }
        public byte Type { get; set; }
        public byte Easiness { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Dynamic { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;

        public MaterialForm Material { get; set;}

        public QuestionForm[]? Children { get; set; }

        public OptionForm[] OptionItems { get; set; }
        public AnalysisForm[] AnalysisItems { get; set; }

        public byte Score { get; set; }
    }
}
