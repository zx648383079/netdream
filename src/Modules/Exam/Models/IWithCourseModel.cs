using NetDream.Shared.Models;

namespace NetDream.Modules.Exam.Models
{
    public interface IWithCourseModel
    {
        public int CourseId { get; }
        public ListLabelItem? Course { set; }
    }
}