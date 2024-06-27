namespace NetDream.Shared.Models
{
    public class AgreeResult
    {
        public bool IsAgree { get; set; }
        public bool IsDisagree { get; set; }

        public int AgreeCount { get; set; }
        public int DisagreeCount { get; set; }

        public AgreeResult(bool isAgree, int agreeCount, int disagreeCount)
        {
            IsAgree = isAgree;
            IsDisagree = !isAgree;
            AgreeCount = agreeCount;
            DisagreeCount = disagreeCount;
        }
    }
}
