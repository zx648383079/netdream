using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Models
{
    public class PageEvaluateModel : PageEvaluateEntity
    {
        public PageEntity Page { get; set; }
        public PageQuestionEntity[] QuestionItems { get; set; }
    }
}
