using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Exam.Forms
{
    public class PageForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public byte RuleType { get; set; }
        public string RuleValue { get; set; } = string.Empty;
        public int StartAt { get; set; }
        public int EndAt { get; set; }
        public int LimitTime { get; set; }
        public int Score { get; set; }
        public int QuestionCount { get; set; }
        public int CourseId { get; set; }
        public int CourseGrade { get; set; }
    }
}
