using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Exam.Models
{
    public class QuestionListItem : IRuleQuestion, IQuestionModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public int CourseGrade { get; set; }
        public int MaterialId { get; set; }
        public byte Type { get; set; }
        public byte Easiness { get; set; }
        public int UserId { get; set; }
        public byte Status { get; set; }
        public int CreatedAt { get; set; }
        public ListLabelItem? Course { get; set; }
        public IUser? User { get; set; }

        public string CourseGradeFormat { get; set; }

        public int Score { get; set; }
    }
}
