namespace NetDream.Modules.Exam.Forms
{
    public class OptionForm
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int QuestionId { get; set; }
        public byte Type { get; set; }
        public byte IsRight { get; set; }
    }
}