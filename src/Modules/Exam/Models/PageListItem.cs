using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Exam.Models
{
    public class PageListItem : IWithUserModel, IWithCourseModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
        public int Score { get; set; }
        public int QuestionCount { get; set; }
        public int CourseId { get; set; }

        public int UserId { get; set; }
        public int CourseGrade { get; set; }

        public string CourseGradeFormat { get; set; }
        public IUser? User { get; set; }
        public ListLabelItem? Course { get; set; }
    }
}
