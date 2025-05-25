using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Exam.Models
{
    public interface IQuestionModel : IWithCourseModel, IWithUserModel
    {
        public int Id { get; }
        public string Title { get; }
        public int CourseId { get; }
        public int CourseGrade { get; }
        public int MaterialId { get; }
        public byte Type { get; }
        public byte Easiness { get; }
        public int UserId { get; }
        public byte Status { get; }
        public int CreatedAt { get; }

        public string CourseGradeFormat { get; }
    }
}
