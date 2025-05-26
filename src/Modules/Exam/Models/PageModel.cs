using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Models
{
    public class PageModel : PageEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 0 随机选题 1 固定题
        /// </summary>
        public byte RuleType { get; set; }
        public int StartAt { get; set; }
        public int EndAt { get; set; }
        public int LimitTime { get; set; }
        public int UserId { get; set; }
        public byte Status { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
        public int Score { get; set; }
        public int QuestionCount { get; set; }
        public int CourseId { get; set; }
        public int CourseGrade { get; set; }
        public IRuleObject[] RuleValue { get; set; }
    }

    public interface IRuleObject
    {
        public int Score { get; }
    }

    public interface IRuleQuestion : IRuleObject
    {
        public int Id { get; }
    }

    public class RuleQuestion : IRuleQuestion
    {
        public int Id { get; set; }
        public int Score { get; set; }
    }

    public class RuleRandomQuestion : IRuleObject
    {
        public int Course { get; set; }
        public byte Type { get; set; }
        public int Amount { get; set; }
        public int Score { get; set; }
    }
}
