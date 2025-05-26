namespace NetDream.Modules.Exam.Forms
{
    public class AnalysisForm
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public byte Type { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}