using NetDream.Shared.Models;

namespace NetDream.Modules.Exam.Models
{
    public interface IWithQuestionModel
    {
        public int QuestionId { get; }
        public ListArticleItem? Question { set; }
    }
}