using NetDream.Modules.Exam.Entities;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Exam.Models
{
    public class QuestionModel : QuestionEntity, IQuestionModel, IRuleQuestion
    {
        public ListLabelItem? Course { get; set; }

        public IUser? User { get; set; }

        public string CourseGradeFormat { get; set; }

        public QuestionMaterialEntity? Material { get; set; }

        public QuestionOptionEntity[]? OptionItems { get; set; }

        public QuestionAnalysisEntity[]? AnalysisItems { get; set; }

        public bool Editable { get; set; }
        public QuestionModel[] Children { get; set; }

        public byte Score { get; set; }
    }
}
