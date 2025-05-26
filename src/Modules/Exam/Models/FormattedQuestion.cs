using NetDream.Modules.Exam.Entities;
using System.Text.Json.Nodes;

namespace NetDream.Modules.Exam.Models
{
    public class FormattedQuestion
    {
        public int Order { get; set; }
        public int Id { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public string Content { get; set; }

        public string Dynamic { get; set; }

        public byte Type { get; set; }

        public QuestionMaterialEntity? Material { get; set; }
        public PageQuestionEntity? Log { get; set; }

        public FormattedQuestion? Parent { get; set; }

        public JsonNode? Answer { get; set; }

        public string? Analysis { get; set; }

        public FormattedQuestionOption[]? Option { get; set; }

        public int Right { get; set; }
        public JsonNode? YourAnswer { get; set; }
        public int MaxScore { get; internal set; }
    }

    public class FormattedQuestionOption
    {
        public string Order { get; set; }
        public int Id { get; set; }
        public string Content { get; set; }

        public bool IsRight { get; set; }
    }

    public class FormattedQuestionEvaluate
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public byte Status { get; set; }
        public byte Score { get; set; }
        public byte MaxScore { get; set; }
        public string Remark { get; set; }
    }
}
