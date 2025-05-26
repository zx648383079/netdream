using NetDream.Modules.Exam.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Exam.Models
{
    public class PageEvaluateModel : PageEvaluateEntity
    {
        public PageEntity Page { get; set; }
        public PageQuestionEntity[] QuestionItems { get; set; }
        public IUser? User { get; internal set; }
        public FormattedQuestion[] Data { get; internal set; }
    }
}
