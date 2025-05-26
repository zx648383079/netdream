using NetDream.Modules.Exam.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace NetDream.Modules.Exam.Forms
{
    public class PageForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public byte RuleType { get; set; }
        public JsonArray RuleValue { get; set; }
        public int StartAt { get; set; }
        public int EndAt { get; set; }
        public int LimitTime { get; set; }
        public int Score { get; set; }
        public int QuestionCount { get; set; }
        public int CourseId { get; set; }
        public int CourseGrade { get; set; }

        public QuestionForm[]? QuestionItems { get; set; }
    }
}
