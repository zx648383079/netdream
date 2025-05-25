using NetDream.Modules.Exam.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Exam.Models
{
    public class GradeListItem : CourseGradeEntity, IWithCourseModel
    {
        public ListLabelItem? Course { get; set; }
    }
}
